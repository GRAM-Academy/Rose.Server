using System;
using System.Linq;
using System.Collections.Generic;
using Aegis;
using Rose.Engine;
using Rose.Engine.Cache;
using Rose.Engine.Execute;
using Newtonsoft.Json.Linq;

namespace Rose.Services
{
    public partial class RoseAPI
    {
        [DispatchMethod("hello")]
        private void hello(RequestHandlerArgument arg)
        {
            string executingAssemblyName = Framework.ExecutingAssembly.GetName().Name;
            JArray services = new JArray();
            JObject jsonResult = new JObject()
            {
                { "ROSE", "Rapid Online Service Engine" },
                { "Framework", new JObject()
                    {
                        { "AEGISFramework version", Framework.AegisVersion.ToString(3) },
                        { $"{Settings.EngineName} version", Settings.EngineVersion.ToString(3) },
                        { $"{executingAssemblyName} version", Framework.ExecutingVersion.ToString(3) }
                    }
                },
                { "Service Modules", services }
            };


            //  등록된 Assembly의 버전정보
            var dict = new Dictionary<string, string>();
            foreach (var item in Starter.ServiceAssemblies)
                dict.Add(item.Key, item.Value.GetName().Version.ToString(3));

            //  JObject 형식으로 변환
            var token = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(dict));
            services.Add(token);


            arg.Response(jsonResult);
        }


        [DispatchMethod("select")]
        private void select(RequestHandlerArgument arg)
        {
            var executor = new Executer(arg.MessageBody);
            var result = executor.Execute();
            arg.Response(result);
        }


        [DispatchMethod("insert")]
        private void insert(RequestHandlerArgument arg)
        {
            var executor = new Executer(arg.MessageBody);
            var result = executor.Execute();
            arg.Response(result);
        }


        [DispatchMethod("update")]
        private void update(RequestHandlerArgument arg)
        {
            var executor = new Executer(arg.MessageBody);
            var result = executor.Execute();
            arg.Response(result);
        }


        [DispatchMethod("delete")]
        private void delete(RequestHandlerArgument arg)
        {
            var executor = new Executer(arg.MessageBody);
            var result = executor.Execute();
            arg.Response(result);
        }
    }
}
