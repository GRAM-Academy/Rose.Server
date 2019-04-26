using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Reflection;
using Aegis;
using Aegis.Data;
using Aegis.Calculate;
using Aegis.Network;
using Aegis.Threading;
using Rose.Engine;
using Rose.Engine.Storage;
using Rose.Engine.Cache;
using Newtonsoft.Json.Linq;

namespace Rose.Services
{
    public class Starter
    {
        private HttpServer _httpServer;
        private List<Tuple<string, RequestHandler>> _handlers = new List<Tuple<string, RequestHandler>>();

        private static List<Starter> _listStarter = new List<Starter>();
        private static Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>();

        public static TreeNode<string> Config { get; private set; }
        public static Dictionary<string, Assembly> ServiceAssemblies { get { return _assemblies.ToDictionary(v => v.Key, v => v.Value); } }

        public static IRosePreprocessor Preprocessor { get; internal set; }





        private static Assembly LoadAssembly(string name)
        {
            try
            {
                Assembly assembly;
                if (_assemblies.TryGetValue(name, out assembly) == false)
                {
                    assembly = Assembly.LoadFrom(name);
                    _assemblies.Add(name, assembly);

                    Logger.Info($"{name}({assembly.GetName().Version.ToString(3)})");
                }

                return assembly;
            }
            catch (Exception e)
            {
                throw new AegisException(RoseResult.InvalidAssembly, e.Message);
            }
        }


        public static void Start(TreeNode<string> roseConfig)
        {
            Logger.Info("Rose.Engine version {0}", Settings.EngineVersion.ToString(1));
            Logger.Info("{0} version {1}", Framework.ExecutingAssembly.GetName().Name, Framework.ExecutingVersion.ToString(3));
            Logger.Info("");


            Config = roseConfig.DeepClone();

            var workerThreadCount = Config.GetValue("engine/workerThreadCount", "4").ToInt32();
            if (workerThreadCount < 1 || workerThreadCount > 8)
                throw new AegisException(RoseResult.InvalidArgument, "The range of 'workerThreadCount' is from 1 to 8.");
            SpinWorker.WorkerThreadCount = workerThreadCount;


            Logger.Info("StorageEngine initializing...");
            StorageEngine.Initialize(Config.GetNode("engine/storage"));


            lock (_listStarter)
            {
                _listStarter.Clear();


                //  Preprocessor
                {
                    var assemblyName = Config.GetValue("services/preprocessor/assembly", null);
                    Assembly assembly;
                    if (assemblyName == null ||
                        Path.GetFileName(Framework.ExecutingAssembly.Location).Equals(assemblyName, StringComparison.OrdinalIgnoreCase))
                        assembly = Framework.ExecutingAssembly;
                    else
                        assembly = LoadAssembly(assemblyName);


                    var handlerName = Config.GetValue("services/preprocessor/handler", null);
                    if (handlerName != null)
                    {
                        Preprocessor = assembly.CreateInstance(handlerName) as IRosePreprocessor;
                        if (Preprocessor == null)
                            throw new AegisException(RoseResult.InvalidHandler, "There is no '{0}' class or '{0}' is not derive from {1}.", handlerName, nameof(IRosePreprocessor));
                    }
                }


                //  Named objects
                var propNamedObjects = Config.TryGetNode("services/globalObjects");
                if (propNamedObjects != null)
                {
                    foreach (var target in propNamedObjects.Childs)
                    {
                        var assemblyName = target.GetValue("assembly");
                        var targetObject = target.GetValue("targetObject");
                        var objectName = target.GetValue("objectName");
                        var data = target.GetNode("data");

                        Assembly assembly;
                        if (assemblyName == null || assemblyName == "")
                            assembly = Framework.ExecutingAssembly;
                        else
                            assembly = LoadAssembly(assemblyName);

                        var handlerInstance = assembly.CreateInstance(targetObject, false,
                                                                      BindingFlags.NonPublic | BindingFlags.Instance,
                                                                      null, null, null, null) as GlobalObjectBase;
                        if (handlerInstance == null)
                            throw new AegisException(RoseResult.InvalidHandler, "There is no '{0}' class or '{0}' is not derive from {1}.", targetObject, nameof(IRosePreprocessor));

                        handlerInstance.Name = objectName;
                        handlerInstance.Data = data.DeepClone();
                        NamedObjectManager.Add(objectName, handlerInstance);
                        handlerInstance.Initialize();
                    }
                }


                Logger.Info("");
                Logger.Info("Registering network handlers...");


                //  Http Network
                foreach (var httpHandler in Config.GetNode("engine").Childs.Where(v => v.Name == "httpHandlers"))
                {
                    var handlers = new List<Tuple<string, RequestHandler>>();
                    var starter = new Starter();
                    _listStarter.Add(starter);


                    //  처리기 등록
                    foreach (var targetHandler in httpHandler.Childs.Where(v => v.Value == null))
                    {
                        var handlerName = targetHandler.GetValue("handler");
                        var assemblyName = targetHandler.GetValue("assembly");
                        var route = targetHandler.GetValue("route");


                        Assembly assembly;
                        if (assemblyName == null ||
                            Path.GetFileName(Framework.ExecutingAssembly.Location).Equals(assemblyName, StringComparison.OrdinalIgnoreCase))
                        {
                            assembly = Framework.ExecutingAssembly;
                        }
                        else
                        {
                            assembly = LoadAssembly(assemblyName);
                            Logger.Info("Routing on {0}", route);
                        }

                        var handlerInstance = assembly.CreateInstance(handlerName) as RequestHandler;
                        if (handlerInstance == null)
                            throw new AegisException(RoseResult.InvalidHandler, "There is no '{0}' class or '{0}' is not derive from {1}.", handlerName, nameof(RequestHandler));

                        handlerInstance.Config = targetHandler.DeepClone();
                        handlers.Add(new Tuple<string, RequestHandler>(route, handlerInstance));

                        Logger.Info("+ {0}", handlerName);
                    }


                    Logger.Info("");
                    var prefix = httpHandler.GetValue("prefix");
                    starter.StartHttpNetwork(prefix, handlers);
                }
            }
        }


        public static void Stop()
        {
            lock (_listStarter)
            {
                foreach (var starter in _listStarter)
                    starter.Release();
                _listStarter.Clear();
            }


            SchemeCatalog.Clear();
            Logger.Info("SchemeCatalog released.");

            StorageEngine.Release();
            Logger.Info("StorageEngine released.");
        }


        private void StartHttpNetwork(string address, List<Tuple<string, RequestHandler>> handlers)
        {
            Logger.Info("Starting HttpSession on {0}", address);

            _httpServer = new HttpServer();
            _httpServer.DispatchWithWorkerThread = true;
            _httpServer.AddPrefix(address);
            foreach (var handler in handlers)
            {
                _handlers.Add(handler);
                _httpServer.Route(handler.Item1, handler.Item2);
            }

            _httpServer.InvalidRouteHandler = OnInvalidRouteRequested;
            _httpServer.Start();
        }


        public void Release()
        {
            if (_httpServer != null)
            {
                if (_httpServer.HttpListener.IsListening
                    && _httpServer.HttpListener.Prefixes.Count() > 0)
                    Logger.Info("HttpSession({0}) closed.", _httpServer.HttpListener.Prefixes.First());

                _httpServer.Stop();
                _httpServer = null;
            }


            _handlers.Clear();
        }


        private static void OnInvalidRouteRequested(HttpRequestData request)
        {
            string result = (new JObject()
            {
                { "resultCode", RoseResult.InvalidRoute },
                { "message", "Invalid route requested." },
            }).ToString(Newtonsoft.Json.Formatting.None);


            byte[] buffer = request.ContentEncoding.GetBytes(result);
            request.Context.Response.ContentLength64 = buffer.Length;
            request.Context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            request.Context.Response.OutputStream.Close();
        }
    }
}
