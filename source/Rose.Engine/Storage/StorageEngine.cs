using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;

namespace Rose.Engine.Storage
{
    public abstract class StorageEngine
    {
        public static StorageEngine Engine { get; private set; }





        public static void Initialize(Aegis.Data.TreeNode<string> config)
        {
            var type = config.GetValue("type");
            if (type == null || type == "" || type == "none")
                Engine = new NullDB();
            else if (type == "mysql")
                Engine = new MySqlDB();
            else if (type == "mysql_async")
                Engine = new MySqlDBAsync();
            else
                throw new AegisException(RoseResult.InvalidArgument, "Invalid argument at 'rose/engine/storage/type'.");


            Engine.CheckStorage(config);
            Engine.InitEngine(config);
        }


        public static void Release()
        {
            if (Engine.QueuedJobCount > 0)
            {
                Logger.Info("Waiting for delayed write operation.");
                while (Engine.QueuedJobCount > 0)
                    System.Threading.Thread.Sleep(10);
                Logger.Info("Done.");
            }

            Engine?.ReleaseEngine();
        }


        public static void CheckRoseStorage(Aegis.Data.TreeNode<string> config)
        {
            StorageEngine engine;
            var type = config.GetValue("type");
            if (type == null || type == "" || type == "none")
                engine = new NullDB();
            else if (type == "mysql")
                engine = new MySqlDB();
            else
                throw new AegisException(RoseResult.InvalidArgument, "Invalid argument at 'rose/engine/storage/type'.");

            engine.CheckStorage(config);
        }


        public static void CreateRoseStorage(Aegis.Data.TreeNode<string> config)
        {
            StorageEngine engine;
            var type = config.GetValue("type");
            if (type == null || type == "" || type == "none")
                engine = new NullDB();
            else if (type == "mysql")
                engine = new MySqlDB();
            else
                throw new AegisException(RoseResult.InvalidArgument, "Invalid argument at 'rose/engine/storage/type'.");

            engine.CreateStorage(config);
        }


        protected abstract void InitEngine(Aegis.Data.TreeNode<string> config);
        protected abstract void ReleaseEngine();
        public abstract int QueuedJobCount { get; }

        protected abstract void CheckStorage(Aegis.Data.TreeNode<string> config);
        protected abstract void CreateStorage(Aegis.Data.TreeNode<string> config);


        internal abstract void CreateScheme(Cache.Scheme scheme);
        internal abstract void DeleteScheme(Cache.Scheme scheme);


        internal abstract void CreateCollection(Cache.Collection collection);
        internal abstract void DeleteCollection(Cache.Collection collection);


        internal abstract void AddIndex(Cache.Collection collection, string indexName);
        internal abstract void DeleteIndex(Cache.Collection collection, string indexName);


        internal abstract void InsertData(Cache.Collection collection, List<Cache.DataObject> dataList);
        internal abstract void UpdateData(Cache.Collection collection, List<Cache.DataObject> dataList);
        internal abstract void DeleteData(Cache.Collection collection, List<Cache.DataObject> dataList);


        internal abstract string ReadData(Cache.Collection collection, string objectId);
    }
}
