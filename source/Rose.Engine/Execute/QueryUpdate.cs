using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Data.Json;
using Newtonsoft.Json.Linq;
using Rose.Engine.Cache;

namespace Rose.Engine.QueryParser
{
    internal class QueryUpdate : QueryBase
    {
        private readonly JToken Data;





        public QueryUpdate(Execute.Executer executer, JObject query)
            : base(executer, query)
        {
            //  data 절
            JToken dataToken = JsonQuery.GetProperty("data").Value;
            if (dataToken is JValue)
            {
                string data = (string)dataToken;
                if (data.Length > 0 && data[0] == '@')
                {
                    List<DataObject> resultSet;
                    if (ParentExecuter.ResultSets.TryGetValue(data, out resultSet) == false)
                        throw new AegisException(RoseResult.InvalidArgument, $"{data} is not exist name.");


                    if (resultSet.Count() > 1)
                    {
                        JArray arr = new JArray();
                        foreach (var item in resultSet)
                            arr.Add(JObject.Parse(item.Data));
                        Data = arr;
                    }
                    else if (resultSet.Count() == 1)
                        Data = JObject.Parse(resultSet.First().Data);
                    else
                        Data = JArray.Parse("[]");
                }
                else
                    throw new AegisException(RoseResult.InvalidArgument, $"{data} is not valid name.");
            }
            else if (dataToken is JObject)
                Data = dataToken;
            else if (dataToken is JArray)
                Data = dataToken.DeepClone();
        }


        public override List<DataObject> Execute()
        {
            List<DataObject> result;
            using (Collection.ReaderLock)
            {
                result = GetResult(Where).Values.ToList();
                foreach (var item in result)
                    item.ReplaceData(Data);
            }

            Storage.StorageEngine.Engine.UpdateData(Collection, result);
            return null;
        }
    }
}
