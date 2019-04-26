using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Data.Json;
using Aegis.Threading;
using Newtonsoft.Json.Linq;
using Rose.Engine.QueryParser;
using Rose.Engine.Cache;

namespace Rose.Engine.Execute
{
    public class Executer
    {
        public readonly StringBuilder CommandText = new StringBuilder();
        internal Dictionary<string, List<DataObject>> ResultSets { get; private set; }





        public Executer()
        {
        }


        public Executer(string commandText)
        {
            CommandText.Append(commandText);
        }


        public JToken Execute()
        {
            JObject json = JObject.Parse(CommandText.ToString());
            return Execute(json);
        }


        public JToken Execute(JObject jsonQuery)
        {
            ResultSets = new Dictionary<string, List<DataObject>>();


            //  단일쿼리와 멀티쿼리가 섞여있는지 확인
            int singleQueryCount = 0, multiQueryCount = 0;
            foreach (var item in jsonQuery.Children())
            {
                if (item is JProperty == false)
                    continue;

                var key = (item as JProperty).Name;
                if (key[0] == '@')
                    ++multiQueryCount;
                else
                    ++singleQueryCount;
            }
            if (singleQueryCount > 0  && multiQueryCount > 0)
                throw new AegisException(RoseResult.InvalidArgument, "Single query and Multi query cannot be used together.");


            //  단일쿼리 처리
            if (singleQueryCount > 0)
            {
                var result = ExecuteOneQuery(jsonQuery);
                ResultSets.Add("@", result);
            }
            //  멀티쿼리 처리
            else
            {
                foreach (var item in jsonQuery.Children())
                {
                    if (item is JProperty == false)
                        continue;

                    var key = (item as JProperty).Name;
                    var value = (item as JProperty).Value;
                    if (key.Length == 0 || key == "@")
                        throw new AegisException(RoseResult.InvalidArgument, $"Invalid reference name('{key}').");

                    //  이름 중복여부 확인
                    if (ResultSets.ContainsKey(key) == true)
                        throw new AegisException(RoseResult.InvalidArgument, $"'{key}' is duplicated name.");

                    var result = ExecuteOneQuery(value as JObject);
                    ResultSets.Add(key, result);
                }
            }


            JToken resultToken = null;
            foreach (var item in ResultSets)
            {
                if (item.Value == null)
                    continue;

                JArray result = new JArray();
                foreach (var data in item.Value)
                    result.Add(JToken.Parse(data.Data));
                resultToken = result;
            }

            return resultToken;
        }


        private List<DataObject> ExecuteOneQuery(JObject jsonQuery)
        {
            QueryBase queryBase;
            string command = ((string)jsonQuery.GetProperty("cmd").Value).Trim();

            if (command == "select") queryBase = new QuerySelect(this, jsonQuery);
            else if (command == "insert") queryBase = new QueryInsert(this, jsonQuery);
            else if (command == "update") queryBase = new QueryUpdate(this, jsonQuery);
            else if (command == "delete") queryBase = new QueryDelete(this, jsonQuery);
            else
                throw new AegisException(RoseResult.UnknownCommand);


            var ret = queryBase.Execute();
            return ret;
        }
    }
}
