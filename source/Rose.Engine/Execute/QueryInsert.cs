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
    internal class QueryInsert : QueryBase
    {
        private readonly JArray Data = new JArray();
        private readonly string UniqueFor = null;
        private readonly string OnDuplicate = null;





        public QueryInsert(Execute.Executer executer, JObject query)
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


                    foreach (var item in resultSet)
                        Data.Add(JObject.Parse(item.Data));
                }
                else
                    throw new AegisException(RoseResult.InvalidArgument, $"{data} is not valid name.");
            }
            else if (dataToken is JObject)
                Data.Add(dataToken);
            else if (dataToken is JArray)
            {
                foreach (var item in dataToken)
                    Data.Add(item);
            }


            UniqueFor = (string)JsonQuery.GetProperty("uniqueFor", false)?.Value ?? null;
            OnDuplicate = (string)JsonQuery.GetProperty("onDuplicate", false)?.Value ?? null;
            if (OnDuplicate == null)
                OnDuplicate = "ignore";

            if (OnDuplicate != "ignore" && OnDuplicate != "update")
                throw new AegisException(RoseResult.InvalidArgument, "onDuplicate value must be 'ignore' or 'update'.");
        }


        public override List<DataObject> Execute()
        {
            List<DataObject> dataList = new List<DataObject>();
            using (Collection.WriterLock)
            {
                foreach (JObject item in Data)
                {
                    //  Unique 확인
                    if (UniqueFor != null && UniqueFor != "")
                    {
                        JToken valueToken = item.GetProperty(UniqueFor, false)?.Value;
                        if (valueToken != null)
                        {
                            Dictionary<string, DataObject> result;
                            if (valueToken.Type == JTokenType.String)
                            {
                                result = ScanFromData(Collection.GetObjects(UniqueFor),
                                                      new ConditionToken($"{UniqueFor} == '{valueToken}'"));
                            }
                            else
                            {
                                result = ScanFromData(Collection.GetObjects(UniqueFor),
                                                      new ConditionToken($"{UniqueFor} == {valueToken}"));
                            }


                            //  Key 중복시 처리
                            if (result.Count() > 0)
                            {
                                if (OnDuplicate == "ignore")
                                    continue;

                                if (OnDuplicate == "update")
                                {
                                    foreach (var duplicatedItem in result.Values)
                                        duplicatedItem.ReplaceData(item);

                                    continue;
                                }
                            }
                        }
                    }


                    var obj = Collection.AddData(item);
                    dataList.Add(obj);
                }

                Storage.StorageEngine.Engine.InsertData(Collection, dataList);
                return null;
            }
        }
    }
}
