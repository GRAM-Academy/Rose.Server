using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Aegis;
using Aegis.Data;
using Rose.Engine;
using IronPython.Hosting;
using Newtonsoft.Json.Linq;

/*
 *  VS에서 Debugging할 때 Working Directory를 지정하게 되면
 *  Starter에서 로드된 Dll과 VS Debugger가 사용하는 Dll이 달라 오동작을 한다.
 *  이런 문제를 막기위해 Script 관련 기능은 모두 이 모듈에서 실행하도록 한다.
 */
namespace Rose.Services.Script
{
    public class ScriptManager : GlobalObjectBase
    {
        public dynamic FnRoseEntry { get; private set; }
        public dynamic FnBeforeRequestHandling { get; private set; }
        public dynamic FnBeforeResponseHandling { get; private set; }


        private ScriptAPI _scriptApi = new ScriptAPI();
        private Microsoft.Scripting.Hosting.ScriptEngine _scriptEngine;
        private Microsoft.Scripting.Hosting.ScriptScope _scope;





        private ScriptManager()
        {
        }


        public override void Initialize()
        {
            Func<TreeNode<string>, JObject> nodeToJObject = (node) =>
            {
                JObject obj = new JObject();
                TreeNode<string>.TreeNodeToJson(node, obj);
                return obj;
            };


            _scriptEngine = Python.CreateEngine();
            _scope = _scriptEngine.CreateScope();



            //  Execute main script
            {
                string entryFile = Data.GetValue("entry");
                var src = _scriptEngine.CreateScriptSourceFromFile(entryFile);

                Logger.Info("Main script({0}) loading...", Path.GetFileName(entryFile));
                src.Execute(_scope);


                //  Setting global variables
                SetVariable("ScriptName", Name);                //  globalObjects의 이름
                SetVariable("RoseApi", _scriptApi);             //  ScriptAPI instance
                SetVariable("ServerConfig", nodeToJObject(Starter.Config));    //  Config 전체
                SetVariable("ScriptConfig", nodeToJObject(Data));              //  globalObjects에 정의된 data 항목


                //  Predefined function
                FnRoseEntry = GetVariable("roseEntry");
                FnBeforeRequestHandling = GetVariable("beforeRequestHandling");     //  Request 핸들러
                FnBeforeResponseHandling = GetVariable("beforeResponseHandling");   //  Response 핸들러


                //  Call entry function
                if (FnRoseEntry != null)
                    FnRoseEntry();
            }
        }


        public bool SetVariable(string name, object value, bool exceptionWhenInvalidName = false)
        {
            dynamic tmp;
            if (_scope.TryGetVariable(name, out tmp) == false)
            {
                if (exceptionWhenInvalidName == true)
                    throw new AegisException(RoseResult.InvalidArgument, $"'{name}' is not exists variable.");

                return false;
            }

            _scope.SetVariable(name, value);
            return true;
        }


        public dynamic GetVariable(string name, bool exceptionWhenInvalidName = false)
        {
            dynamic var;
            if (_scope.TryGetVariable(name, out var) == false)
            {
                if (exceptionWhenInvalidName == true)
                    throw new AegisException(RoseResult.InvalidArgument, $"'{name}' is not exists variable.");
            }

            return var;
        }


        public bool Call_BeforeRequestHandling(HttpListenerContext httpContext, ref string messageBody)
        {
            if (FnBeforeRequestHandling != null)
            {
                RequestContext context = new RequestContext(httpContext);
                var ret = FnBeforeRequestHandling(context, messageBody);
                if (ret == null)
                    return false;

                messageBody = ret;
            }

            return true;
        }


        public void Call_BeforeResponseHandling(HttpListenerResponse httpResponse, ref string response)
        {
            if (FnBeforeResponseHandling != null)
            {
                var ret = FnBeforeResponseHandling(httpResponse, response);
                if (ret != null)
                    response = ret;
            }
        }
    }
}