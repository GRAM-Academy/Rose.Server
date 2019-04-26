using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rose.Engine.Cache;

namespace Rose.Engine.QueryParser
{
    internal class QueryDelete : QueryBase
    {
        public QueryDelete(Execute.Executer executer, JObject query)
            : base(executer, query)
        {
        }


        public override List<DataObject> Execute()
        {
            List<DataObject> result;
            using (Collection.WriterLock)
            {
                result = GetResult(Where).Values.ToList();
                Collection.RemoveData(result);
            }

            Storage.StorageEngine.Engine.DeleteData(Collection, result);
            return null;
        }
    }
}
