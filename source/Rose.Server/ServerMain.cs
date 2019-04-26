using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Aegis;
using Aegis.Data;
using Aegis.Threading;

namespace Rose.Server
{
    public static class ServerMain
    {
        public static TreeNode<string> Config { get; private set; }
        public static event Action FinalizedHandler;





        public static void StartServer(System.Windows.Forms.TextBox tb)
        {
            if (tb != null)
                LogMedia.SetTextBoxLogger(tb);


            AegisTask.Run(() =>
            {
                //  Initialize AegisFramework
                Framework.Initialized += Framework_Initialized;
                Framework.Running += Framework_Running;
                Framework.Finalizing += Framework_Finalizing;
                Framework.Finalized += Framework_Finalized;
                Framework.Initialize(false);
            });
        }


        public static void StopServer()
        {
            AegisTask.Run(() =>
            {
                Aegis.Framework.Release();
            });
        }


        public static void CreateStorage()
        {
            try
            {
                var config = TreeNode<string>.LoadFromXml(File.ReadAllText(@".\RoseServerConfig.xml"), "RoseServerConfig").GetNode("rose");
                Rose.Engine.Storage.StorageEngine.CreateRoseStorage(config.GetNode("engine/storage"));
            }
            catch (Exception e)
            {
                Logger.Err(e.Message);
            }
        }


        private static bool Framework_Initialized(string[] arg)
        {
            try
            {
                var config = TreeNode<string>.LoadFromXml(File.ReadAllText(@".\RoseServerConfig.xml"), "RoseServerConfig").GetNode("rose");
                Config = config;

                SetLogger();

                FormMain.Instance.SetServiceName(Config.GetValue("serviceName"));
                Services.Starter.Start(Config);

                Logger.Info("");
                Logger.Info("{0} started.", Framework.ExecutingAssembly.GetName().Name);
            }
            catch (AegisException e) when (e.HResult == Rose.Engine.RoseResult.StorageNotInitialized)
            {
                Logger.Err(e.Message);
                Logger.Err("Please initialize the database first.");
            }
            catch (AegisException e)
            {
                if (e.InnerException != null)
                    Logger.Err($"[ResultCode={e.ResultCodeNo}] {e.InnerException.Message}");
                else
                    Logger.Err($"[ResultCode={e.ResultCodeNo}] {e.Message}");

                return false;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    Logger.Err($"[HResult={e.HResult}] {e.InnerException.Message}");
                else
                    Logger.Err($"[HResult={e.HResult}] {e.Message}");

                return false;
            }

            return true;
        }


        private static void Framework_Running()
        {
            while (true)
            {
                if (Aegis.Framework.WaitForRunning(1000))
                    break;
            }
        }


        private static void Framework_Finalizing(CloseReason reason)
        {
            Services.Starter.Stop();
            Logger.Info("{0} stopped.", Framework.ExecutingAssembly.GetName().Name);
        }


        private static void Framework_Finalized()
        {
            LogMedia.ReleaseAllLogger();

            FinalizedHandler?.Invoke();
        }


        private static void SetLogger()
        {
            string path = Config.GetValue("logFilePath", null);
            string prefix = Config.GetValue("logFilePrefix", "");

            if (path != null)
                LogMedia.SetTextFileLogger(path, prefix);
        }
    }
}
