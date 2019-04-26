using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Calculate;
using Aegis.Data.Json;
using Newtonsoft.Json.Linq;
using Rose.Engine.Cache;

namespace Rose.Services
{
    public partial class RoseAPI
    {
        [DispatchMethod("schemeList")]
        private void schemeList(RequestHandlerArgument arg)
        {
            JObject json = JObject.Parse(arg.MessageBody);
            var response = new JArray();


            SchemeCatalog.GetSchemesInfo(ref response);
            arg.Response(response);
        }


        [DispatchMethod("collectionList")]
        private void collectionList(RequestHandlerArgument arg)
        {
            JObject json = JObject.Parse(arg.MessageBody);
            string schemeName = (string)json.GetProperty("scheme").Value;
            var scheme = SchemeCatalog.GetScheme(schemeName);
            var response = new JArray();


            scheme.GetCollectionsInfo(ref response);
            arg.Response(response);
        }


        [DispatchMethod("createScheme")]
        private void createScheme(RequestHandlerArgument arg)
        {
            var json = JsonConverter.Parse(arg.MessageBody);
            string schemeName = (string)json.GetProperty("scheme").Value;

            SchemeCatalog.CreateScheme(schemeName);
            arg.Response(null);
        }


        [DispatchMethod("dropScheme")]
        private void dropScheme(RequestHandlerArgument arg)
        {
            var json = JsonConverter.Parse(arg.MessageBody);
            string schemeName = (string)json.GetProperty("scheme").Value;

            SchemeCatalog.DeleteScheme(schemeName);
            arg.Response(null);
        }


        [DispatchMethod("createCollection")]
        private void createCollection(RequestHandlerArgument arg)
        {
            var json = JsonConverter.Parse(arg.MessageBody);
            var collection = (string)json.GetProperty("collection").Value;
            string[] target = collection.Split('.');
            if (target.Count() != 2)
                throw new AegisException(Engine.RoseResult.InvalidArgument, "Collection must be in {scheme_name}.{collection_name} format.");

            bool justInCache = ((string)json.GetProperty("justInCache", false)?.Value ?? "false").ToBoolean();
            var scheme = SchemeCatalog.GetScheme(target[0].Trim());

            scheme.CreateCollection(target[1].Trim(), justInCache);
            arg.Response(null);
        }


        [DispatchMethod("dropCollection")]
        private void dropCollection(RequestHandlerArgument arg)
        {
            var json = JsonConverter.Parse(arg.MessageBody);
            var collection = Collection.GetCollection((string)json.GetProperty("collection").Value);

            collection.ParentScheme.DeleteCollection(collection.Name);
            arg.Response(null);
        }


        [DispatchMethod("addIndex")]
        private void addIndex(RequestHandlerArgument arg)
        {
            var json = JsonConverter.Parse(arg.MessageBody);
            var collection = Collection.GetCollection((string)json.GetProperty("collection").Value);
            string indexKey = (string)json.GetProperty("indexKey").Value;

            collection.AddIndex(indexKey);
            arg.Response(null);
        }


        [DispatchMethod("dropIndex")]
        private void dropIndex(RequestHandlerArgument arg)
        {
            var json = JsonConverter.Parse(arg.MessageBody);
            var collection = Collection.GetCollection((string)json.GetProperty("collection").Value);
            string indexKey = (string)json.GetProperty("indexKey").Value;

            collection.DeleteIndex(indexKey);
            arg.Response(null);
        }
    }
}
