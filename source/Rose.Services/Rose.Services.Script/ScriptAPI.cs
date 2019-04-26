using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Newtonsoft.Json.Linq;
using Rose.Engine;
using Rose.Engine.Cache;
using Rose.Engine.Execute;

namespace Rose.Services.Script
{
    public class ScriptAPI
    {
        private static string ResponseString(int resultCode, string message, JToken resultToken)
        {
            JObject result = new JObject()
            {
                { "resultCode", resultCode },
                { "message", message },
                { "result", resultToken ?? "{}" }
            };

            return result.ToString(Newtonsoft.Json.Formatting.None);
        }


        public void Log(string format, params object[] args)
        {
            Logger.Info(format, args);
        }


        #region Version
        public string AegisVersion()
        {
            return Framework.AegisVersion.ToString(3);
        }


        public string RoseEngineVersion()
        {
            return Settings.EngineVersion.ToString(3);
        }


        public string RoseServerVersion()
        {
            return Framework.ExecutingVersion.ToString(3);
        }
        #endregion


        #region RWLock
        public string Collection_EnterReaderLock(string schemeName, string collectionName)
        {
            try
            {
                Scheme scheme = SchemeCatalog.GetScheme(schemeName);
                Collection collection = scheme.GetCollection(collectionName);

                collection.ReaderLock.Enter();
                return ResponseString(RoseResult.Ok, "Ok", null);
            }
            catch (Exception e)
            {
                return ResponseString(RoseResult.UnknownError, e.Message, null);
            }
        }


        public string Collection_LeaveReaderLock(string schemeName, string collectionName)
        {
            try
            {
                Scheme scheme = SchemeCatalog.GetScheme(schemeName);
                Collection collection = scheme.GetCollection(collectionName);

                collection.ReaderLock.Leave();
                return ResponseString(RoseResult.Ok, "Ok", null);
            }
            catch (Exception e)
            {
                return ResponseString(RoseResult.UnknownError, e.Message, null);
            }
        }


        public string Collection_EnterWriterLock(string schemeName, string collectionName)
        {
            try
            {
                Scheme scheme = SchemeCatalog.GetScheme(schemeName);
                Collection collection = scheme.GetCollection(collectionName);

                collection.WriterLock.Enter();
                return ResponseString(RoseResult.Ok, "Ok", null);
            }
            catch (Exception e)
            {
                return ResponseString(RoseResult.UnknownError, e.Message, null);
            }
        }


        public string Collection_LeaveWriterLock(string schemeName, string collectionName)
        {
            try
            {
                Scheme scheme = SchemeCatalog.GetScheme(schemeName);
                Collection collection = scheme.GetCollection(collectionName);

                collection.WriterLock.Leave();
                return ResponseString(RoseResult.Ok, "Ok", null);
            }
            catch (Exception e)
            {
                return ResponseString(RoseResult.UnknownError, e.Message, null);
            }
        }
        #endregion


        #region Scheme / Collection
        public Scheme GetScheme(string schemeName, bool createIfNotExists)
        {
            return SchemeCatalog.GetScheme(schemeName, createIfNotExists);
        }


        public Collection CreateCollection(string schemeName, string collectionName, bool justInCache)
        {
            var scheme = SchemeCatalog.GetScheme(schemeName, false);
            return scheme.CreateCollection(collectionName, justInCache);
        }


        public Collection GetCollection(string schemeName, string collectionName, bool raiseException)
        {
            var scheme = SchemeCatalog.GetScheme(schemeName, false);
            return scheme.GetCollection(collectionName, raiseException);
        }
        #endregion


        #region Execute & CRUD
        public string Execute(string query)
        {
            try
            {
                var executor = new Executer(query);
                var result = executor.Execute().ToString(Newtonsoft.Json.Formatting.Indented);
                return ResponseString(RoseResult.Ok, "Ok", result);
            }
            catch (Exception e)
            {
                return ResponseString(RoseResult.UnknownError, e.Message, null);
            }
        }


        public static JToken Select(string collection,
                                    string where, string sortKey, string orderBy,
                                    int rangeStart, int rangeCount)
        {
            try
            {
                JProperty propWhere = new JProperty("where", where);
                JProperty propSort = new JProperty("sortKey", sortKey);
                JProperty propRange = new JProperty("range", new JArray(rangeStart, rangeCount));
                JObject jsonRequest = new JObject()
                {
                    { "cmd", "select" },
                    { "collection", collection },
                    propWhere, propSort, propRange
                };

                if (where == null)
                    propWhere.Remove();
                if (sortKey == null)
                    propSort.Remove();

                var result = (new Executer()).Execute(jsonRequest);
                return ResponseString(RoseResult.Ok, "Ok", result);
            }
            catch (Exception e)
            {
                return ResponseString(RoseResult.UnknownError, e.Message, null);
            }
        }


        public static JToken Insert(string collection, string uniqueFor, string onDuplicate, JToken data)
        {
            try
            {
                JProperty propUniqueFor = new JProperty("uniqueFor", uniqueFor);
                JProperty propOnDuplicate = new JProperty("onDuplicate", onDuplicate);
                JObject jsonRequest = new JObject()
                {
                    { "cmd", "insert" },
                    { "collection", collection },
                    propUniqueFor, propOnDuplicate,
                    { "data", data }
                };

                if (propUniqueFor == null)
                    propUniqueFor.Remove();
                if (propOnDuplicate == null)
                    propOnDuplicate.Remove();

                var result = (new Executer()).Execute(jsonRequest);
                return ResponseString(RoseResult.Ok, "Ok", result);
            }
            catch (Exception e)
            {
                return ResponseString(RoseResult.UnknownError, e.Message, null);
            }
        }


        public static JToken Update(string collection, string where, JToken data)
        {
            try
            {
                JProperty propWhere = new JProperty("where", where);
                JObject jsonRequest = new JObject()
                {
                    { "cmd", "update" },
                    { "collection", collection },
                    { "data", data },
                    propWhere
                };

                if (where == null)
                    propWhere.Remove();

                var result = (new Executer()).Execute(jsonRequest);
                return ResponseString(RoseResult.Ok, "Ok", result);
            }
            catch (Exception e)
            {
                return ResponseString(RoseResult.UnknownError, e.Message, null);
            }
        }


        public static JToken Delete(string collection, string where)
        {
            try
            {
                JProperty propWhere = new JProperty("where", where);
                JObject jsonRequest = new JObject()
                {
                    { "cmd", "delete" },
                    { "collection", collection },
                    propWhere
                };

                if (where == null)
                    propWhere.Remove();

                var result = (new Executer()).Execute(jsonRequest);
                return ResponseString(RoseResult.Ok, "Ok", result);
            }
            catch (Exception e)
            {
                return ResponseString(RoseResult.UnknownError, e.Message, null);
            }
        }
        #endregion


        public void Response(RequestContext context, string result)
        {
            //  httpResponse는 한 번만 사용할 수 있다.
            var httpResponse = context.HttpListenerContext.Response;
            if (httpResponse.ContentLength64 != 0)
                return;


            //  결과 전송
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(result);
                httpResponse.ContentLength64 = buffer.Length;
                httpResponse.OutputStream.Write(buffer, 0, buffer.Length);
                httpResponse.OutputStream.Close();
            }
            catch (HttpListenerException e) when ((uint)e.ErrorCode == 1229)
            {
            }
            catch (Exception e)
            {
                Logger.Err("------------------------------");
                Logger.Err(e.ToString());
            }
        }
    }
}
