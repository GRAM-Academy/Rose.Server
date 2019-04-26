using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Calculate;
using Rose.Engine.Cache;
using Rose.Engine.Execute;
using Newtonsoft.Json.Linq;

namespace Rose.Services
{
    public partial class RoseAPI : RequestHandler
    {
        public RoseAPI()
        {
            try
            {
                //  Collection이 없으면 생성
                var scheme = SchemeCatalog.GetScheme("rose", true);
                if (scheme.GetCollection("sequenceNumbers", false) == null)
                    scheme.CreateCollection("sequenceNumbers", false);
            }
            catch (Exception)
            {
            }
        }


        public static JToken Select(string scheme, string collection,
                                    string where, string sortKey, string orderBy,
                                    int rangeStart, int rangeCount)
        {
            JProperty propWhere = new JProperty("where", where);
            JProperty propSort = new JProperty("sortKey", sortKey);
            JProperty propRange = new JProperty("range", new JArray(rangeStart, rangeCount));
            JProperty propOrderBy = new JProperty("orderBy", orderBy);
            JObject jsonRequest = new JObject()
            {
                { "cmd", "select" },
                { "scheme", scheme },
                { "collection", collection },
                { "condition", new JObject()
                    {
                        propWhere, propSort, propOrderBy, propRange
                    }
                }
            };

            if (where == null)
                propWhere.Remove();
            if (sortKey == null)
                propSort.Remove();

            return (new Executer()).Execute(jsonRequest);
        }


        public static JToken Insert(string scheme, string collection, string notExists, JToken data)
        {
            JProperty propNotExists = new JProperty("notExists", notExists);
            JObject jsonRequest = new JObject()
            {
                { "cmd", "insert" },
                { "scheme", scheme },
                { "collection", collection },
                { "condition", new JObject()
                    {
                        propNotExists
                    }
                },
                { "data", data }
            };

            if (notExists == null)
                propNotExists.Remove();

            return (new Executer()).Execute(jsonRequest);
        }


        public static JToken Update(string scheme, string collection, string where, JToken data)
        {
            JProperty propWhere = new JProperty("where", where);
            JObject jsonRequest = new JObject()
            {
                { "cmd", "update" },
                { "scheme", scheme },
                { "collection", collection },
                { "data", data },
                { "condition", new JObject()
                    {
                        propWhere
                    }
                }
            };

            if (where == null)
                propWhere.Remove();

            return (new Executer()).Execute(jsonRequest);
        }


        public static JToken Delete(string scheme, string collection, string where)
        {
            JProperty propWhere = new JProperty("where", where);
            JObject jsonRequest = new JObject()
            {
                { "cmd", "delete" },
                { "scheme", scheme },
                { "collection", collection },
                { "condition", new JObject()
                    {
                        propWhere
                    }
                }
            };

            if (where == null)
                propWhere.Remove();

            return (new Executer()).Execute(jsonRequest);
        }
    }
}
