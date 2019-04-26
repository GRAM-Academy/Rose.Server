using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aegis;
using Aegis.Data;
using Aegis.Data.MySQL;
using Aegis.Calculate;
using Rose.Engine.Cache;
using Newtonsoft.Json.Linq;

namespace Rose.Engine.Storage
{
    internal class MySqlDBAsync : StorageEngine
    {
        private ConnectionPool _poolSelect, _poolWork;
        private Queue<DBCommand> _queueQuery;
        private Thread _thread;

        public override int QueuedJobCount { get { return _queueQuery?.Count ?? 0; } }




        protected override void InitEngine(TreeNode<string> config)
        {
            string ipAddress = config.GetValue("ipAddress", null);
            int port = config.GetValue("port", "3306").ToInt32();
            string userId = config.GetValue("userId", null);
            string userPwd = config.GetValue("userPwd", null);
            string dbName = config.GetValue("dbName", null);
            string charSet = config.GetValue("charSet", "utf8");
            int minDBCCount = config.GetValue("minDBCCount", "2").ToInt32();
            int maxDBCCount = config.GetValue("maxDBCCount", "4").ToInt32();


            if (ipAddress == null || userId == null || userPwd == null || dbName == null)
                throw new AegisException(RoseResult.InvalidArgument, "Not enough arguments to connect MySql.");


            //  Select용 Pool
            _poolSelect = new ConnectionPool(ipAddress, port, charSet, dbName, userId, userPwd);
            _poolSelect.MaxDBCCount = maxDBCCount;
            _poolSelect.IncreasePool(minDBCCount);

            //  Insert / Update / Delete용 pool
            _poolWork = new ConnectionPool(ipAddress, port, charSet, dbName, userId, userPwd);
            _poolWork.MaxDBCCount = 1;
            _poolWork.IncreasePool(1);

            _queueQuery = new Queue<DBCommand>();
            _thread = new Thread(Run);
            _thread.Start();



            //  Load schemes
            using (var cmd = _poolSelect.NewCommand())
            {
                cmd.CommandText.Append("select schemeName from schemes;");
                cmd.Query();
                while (cmd.Reader.Read())
                    SchemeCatalog.AddScheme(cmd.Reader.GetString(0));
            }


            //  Load collections
            List<Collection> collections = new List<Collection>();
            using (var cmd = _poolSelect.NewCommand())
            {
                cmd.CommandText.Append("select collectionName, schemeName, indexes from collections;");
                cmd.Query();
                while (cmd.Reader.Read())
                {
                    string collectionName = cmd.Reader.GetString(0);
                    string schemeName = cmd.Reader.GetString(1);
                    string indexes = cmd.Reader.GetString(2);


                    var scheme = SchemeCatalog.GetScheme(schemeName);
                    if (scheme == null)
                    {
                        Logger.Err("{0} scheme is not exists.", schemeName);
                        continue;
                    }

                    var collection = scheme.AddCollection(collectionName);
                    collections.Add(collection);


                    //  Add indexes
                    if (indexes.Length > 1)
                    {
                        JArray indexArray = JArray.Parse(indexes);
                        foreach (var index in indexArray)
                        {
                            string idx = (string)index;
                            if (idx.Length > 0)
                                collection.AddIndexForInit(idx);
                        }
                    }
                }
            }


            //  Load collection data
            foreach (var collection in collections)
            {
                string tableName = $"{collection.ParentScheme.Name}_{collection.Name}";
                Logger.Info("Loading {0}.{1} collection.", collection.ParentScheme.Name, collection.Name);

                using (var cmd = _poolSelect.NewCommand())
                {
                    cmd.CommandText.Append($"select objectId, data from {tableName};");
                    cmd.Query();
                    while (cmd.Reader.Read())
                    {
                        string objectId = cmd.Reader.GetString(0);
                        string data = cmd.Reader.GetString(1);

                        collection.AddData(objectId, data);
                    }
                }
            }
        }


        protected override void ReleaseEngine()
        {
            _poolWork?.Release();
            _poolWork = null;

            _poolSelect?.Release();
            _poolSelect = null;
        }


        protected override void CheckStorage(TreeNode<string> config)
        {
            string ipAddress = config.GetValue("ipAddress", null);
            int port = config.GetValue("port", "3306").ToInt32();
            string userId = config.GetValue("userId", null);
            string userPwd = config.GetValue("userPwd", null);
            string dbName = config.GetValue("dbName", null);
            string charSet = config.GetValue("charSet", "utf8");
            int minDBCCount = config.GetValue("minDBCCount", "2").ToInt32();
            int maxDBCCount = config.GetValue("maxDBCCount", "4").ToInt32();


            if (ipAddress == null || userId == null || userPwd == null || dbName == null)
                throw new AegisException(RoseResult.InvalidArgument, "Not enough arguments to connect MySql.");


            var pool = new ConnectionPool(ipAddress, port, charSet, dbName, userId, userPwd);
            using (var cmd = pool.NewCommand())
            {
                cmd.CommandText.Append("show databases;");
                cmd.CommandText.Append("show tables;");
                cmd.Query();


                //  데이터베이스 확인
                List<string> databaseNames = new List<string>();
                while (cmd.Reader.Read())
                    databaseNames.Add(cmd.Reader.GetString(0));

                if (databaseNames.Contains(dbName) == false)
                    throw new AegisException(RoseResult.ServerError, $"'{dbName}' is not initialized.");



                //  주요 테이블 확인
                List<string> tableNames = new List<string>();

                cmd.Reader.NextResult();
                while (cmd.Reader.Read())
                    tableNames.Add(cmd.Reader.GetString(0));

                if (tableNames.Contains("schemes") == false ||
                    tableNames.Contains("collections") == false)
                {
                    throw new AegisException(RoseResult.ServerError, $"'{dbName}' is not initialized.");
                }
            }
            pool.Release();
        }


        protected override void CreateStorage(TreeNode<string> config)
        {
            string ipAddress = config.GetValue("ipAddress", null);
            int port = config.GetValue("port", "3306").ToInt32();
            string userId = config.GetValue("userId", null);
            string userPwd = config.GetValue("userPwd", null);
            string dbName = config.GetValue("dbName", null);
            string charSet = config.GetValue("charSet", "utf8");
            int minDBCCount = config.GetValue("minDBCCount", "2").ToInt32();
            int maxDBCCount = config.GetValue("maxDBCCount", "4").ToInt32();


            if (ipAddress == null || userId == null || userPwd == null)
                throw new AegisException(RoseResult.InvalidArgument, "Not enough arguments to connect MySql.");

            var pool = new ConnectionPool(ipAddress, port, charSet, null, userId, userPwd);
            using (var cmd = pool.NewCommand())
            {
                cmd.CommandText.Append($"create database if not exists {dbName};");
                cmd.CommandText.Append($"use {dbName};");
                cmd.CommandText.Append("");
                cmd.CommandText.Append("drop table if exists schemes;");
                cmd.CommandText.Append("create table schemes");
                cmd.CommandText.Append("(");
                cmd.CommandText.Append("    schemeName varchar(50) not null,");
                cmd.CommandText.Append("    primary key(schemeName)");
                cmd.CommandText.Append(") engine=InnoDB default charset=utf8;");
                cmd.CommandText.Append("");
                cmd.CommandText.Append("drop table if exists collections;");
                cmd.CommandText.Append("create table collections");
                cmd.CommandText.Append("(");
                cmd.CommandText.Append("    collectionName varchar(50) not null,");
                cmd.CommandText.Append("    schemeName varchar(50) not null,");
                cmd.CommandText.Append("    indexes text not null,");
                cmd.CommandText.Append("    primary key(collectionName, schemeName)");
                cmd.CommandText.Append(") engine=InnoDB default charset=utf8;");
                cmd.Query();
            }
            pool.Release();
        }


        internal override void CreateScheme(Scheme scheme)
        {
            var cmd = _poolWork.NewCommand();
            cmd.CommandText.Append("insert into schemes(schemeName) values(@schemeName);");
            cmd.BindParameter("@schemeName", scheme.Name);
            PostQuery(cmd);
        }


        internal override void DeleteScheme(Scheme scheme)
        {
            var cmd = _poolWork.NewCommand();
            cmd.CommandText.Append("delete from collections where schemeName=@schemeName;");
            cmd.CommandText.Append("delete from schemes where schemeName=@schemeName;");
            cmd.BindParameter("@schemeName", scheme.Name);
            PostQuery(cmd);
        }


        internal override void CreateCollection(Collection collection)
        {
            if (collection.JustInCache)
                return;

            var cmd = _poolWork.NewCommand();
            string tableName = $"{collection.ParentScheme.Name}_{collection.Name}";

            cmd.CommandText.Append("insert into collections(collectionName, schemeName, indexes) ");
            cmd.CommandText.Append("values(@collectionName, @schemeName, '[]');");
            cmd.BindParameter("@collectionName", collection.Name);
            cmd.BindParameter("@schemeName", collection.ParentScheme.Name);

            cmd.CommandText.Append($"drop table if exists {tableName};");
            cmd.CommandText.Append($"create table {tableName} (");
            cmd.CommandText.Append(" objectId varchar(50) primary key,");
            cmd.CommandText.Append(" data longtext not null");
            cmd.CommandText.Append(") engine=InnoDB default charset=utf8;");
            PostQuery(cmd);
        }


        internal override void DeleteCollection(Collection collection)
        {
            if (collection.JustInCache)
                return;

            var cmd = _poolWork.NewCommand();
            string tableName = $"{collection.ParentScheme.Name}_{collection.Name}";
            cmd.CommandText.Append($"drop table {tableName};");
            cmd.CommandText.Append("delete from collections where collectionName=@collectionName and schemeName=@schemeName;");
            cmd.BindParameter("@collectionName", collection.Name);
            cmd.BindParameter("@schemeName", collection.ParentScheme.Name);
            PostQuery(cmd);
        }


        internal override void AddIndex(Collection collection, string indexName)
        {
            if (collection.JustInCache)
                return;

            JArray indexArray = new JArray();
            string indexes = "";
            foreach (string index in collection.IndexMap.GetIndexNames())
                indexArray.Add(index);
            indexes = indexArray.ToString(Newtonsoft.Json.Formatting.None);


            var cmd = _poolWork.NewCommand();
            cmd.CommandText.Append($"update collections set indexes=@indexes ");
            cmd.CommandText.Append($"where collectionName=@collectionName and schemeName=@schemeName;");
            cmd.BindParameter("@indexes", indexes);
            cmd.BindParameter("@collectionName", collection.Name);
            cmd.BindParameter("@schemeName", collection.ParentScheme.Name);
            PostQuery(cmd);
        }


        internal override void DeleteIndex(Collection collection, string indexName)
        {
            if (collection.JustInCache)
                return;

            JArray indexArray = new JArray();
            string indexes = "";
            foreach (string index in collection.IndexMap.GetIndexNames())
                indexArray.Add(index);
            indexes = indexArray.ToString(Newtonsoft.Json.Formatting.None);


            var cmd = _poolWork.NewCommand();
            cmd.CommandText.Append($"update collections set indexes=@indexes ");
            cmd.CommandText.Append($"where collectionName=@collectionName and schemeName=@schemeName;");
            cmd.BindParameter("@indexes", indexes);
            cmd.BindParameter("@collectionName", collection.Name);
            cmd.BindParameter("@schemeName", collection.ParentScheme.Name);
            PostQuery(cmd);
        }


        internal override void InsertData(Collection collection, List<DataObject> dataList)
        {
            if (collection.JustInCache)
                return;

            if (dataList.Count() == 0)
                return;

            var cmd = _poolWork.NewCommand();
            string tableName = $"{collection.ParentScheme.Name}_{collection.Name}";
            int idx = 0;

            foreach (var data in dataList)
            {
                cmd.CommandText.Append($"insert into {tableName}(objectId, data) values(@objectId_{idx}, @data_{idx});");
                cmd.BindParameter($"objectId_{idx}", data.ObjectId);
                cmd.BindParameter($"data_{idx}", data.Data);
                ++idx;
            }
            PostQuery(cmd);
        }


        internal override void UpdateData(Collection collection, List<DataObject> dataList)
        {
            if (collection.JustInCache)
                return;

            if (dataList.Count() == 0)
                return;

            var cmd = _poolWork.NewCommand();
            string tableName = $"{collection.ParentScheme.Name}_{collection.Name}";
            int idx = 0;

            foreach (var data in dataList)
            {
                cmd.CommandText.Append($"update {tableName} set data=@data_{idx} where objectId=@objectId_{idx};");
                cmd.BindParameter($"@data_{idx}", data.Data);
                cmd.BindParameter($"@objectId_{idx}", data.ObjectId);
                ++idx;
            }
            PostQuery(cmd);
        }


        internal override void DeleteData(Collection collection, List<DataObject> dataList)
        {
            if (collection.JustInCache)
                return;

            if (dataList.Count() == 0)
                return;

            var cmd = _poolWork.NewCommand();
            string tableName = $"{collection.ParentScheme.Name}_{collection.Name}";
            int idx = 0;

            foreach (var data in dataList)
            {
                cmd.CommandText.Append($"delete from {tableName} where objectId=@objectId_{idx};");
                cmd.BindParameter($"@objectId_{idx}", data.ObjectId);
                ++idx;
            }
            PostQuery(cmd);
        }


        internal override string ReadData(Collection collection, string objectId)
        {
            if (collection.JustInCache)
                return null;

            using (var cmd = _poolSelect.NewCommand())
            {
                string tableName = $"{collection.ParentScheme.Name}_{collection.Name}";

                cmd.CommandText.Append($"select data from {tableName} where objectId=@objectId;");
                cmd.BindParameter("@objectId", objectId);
                cmd.Query();

                if (cmd.Reader.Read() == false)
                {
                    Logger.Debug("dbg Read failed {0} from MySql.", objectId);
                    return null;
                }

                Logger.Debug("dbg Read {0} from MySql.", objectId);
                return cmd.Reader.GetString(0);
            }
        }


        private void PostQuery(DBCommand cmd)
        {
            lock (_queueQuery)
                _queueQuery.Enqueue(cmd);
        }


        private void Run()
        {
            while (_poolWork != null)
            {
                if (_queueQuery.Count == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                while (_queueQuery.Count > 0)
                {
                    DBCommand cmd;

                    lock (_queueQuery)
                        cmd = _queueQuery.Dequeue();

                    cmd.QueryNoReader();
                    cmd.Dispose();
                }
            }
        }
    }
}
