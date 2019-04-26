using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Aegis;
using Aegis.Data.Json;
using Newtonsoft.Json.Linq;
using Rose.Engine.Cache;
using Rose.Engine.Execute;

namespace Rose.Engine.QueryParser
{
    internal abstract class QueryBase
    {
        public readonly JObject JsonQuery;
        public readonly string Command;
        public readonly string SchemeName, CollectionName;
        public readonly Collection Collection;
        public readonly Where Where;
        protected readonly Executer ParentExecuter;





        protected QueryBase(Executer executer, JObject query)
        {
            string[] target = ((string)query.GetProperty("collection").Value).Split('.');
            if (target.Count() != 2)
                throw new AegisException(RoseResult.InvalidArgument, "Collection must be in {scheme_name}.{collection_name} format.");

            ParentExecuter = executer;
            JsonQuery = (JObject)query.DeepClone();
            Command = ((string)JsonQuery.GetProperty("cmd").Value).Trim();
            SchemeName = target[0].Trim();
            CollectionName = target[1].Trim();
            Collection = SchemeCatalog.GetScheme(SchemeName)?.GetCollection(CollectionName);
            if (Collection == null)
                throw new AegisException(RoseResult.InvalidArgument, $"Invalid scheme name({SchemeName}).");

            var where = JsonQuery.GetProperty("where", false)?.Value;
            if (where != null)
                Where = new Where(where);
        }


        public abstract List<DataObject> Execute();


        protected Dictionary<string, DataObject> ScanFromData(Dictionary<string, DataObject> source, ConditionToken token)
        {
            Dictionary<string, DataObject> result = new Dictionary<string, DataObject>();
            Argument left = token.Left;
            Argument right = token.Right;


            //  계산하기 편하도록 값(Value)은 오른쪽에 배치
            if (left is Value && (right is Value) == false)
                ExchangeArgument(ref left, ref right);


            //  양쪽 모두 값인 경우
            if (left is Value && right is Value)
            {
                int comp = left.CompareTo(right);
                if (IsTrue(comp, token.ComparisonOperator))
                    return source;

                return result;
            }


            //  key에 해당하는 ObjectId 목록을 가져온다.
            string key = (left as ReferenceValue).Expr;
            List<string> objectIdList = Collection.GetObjects(key).Keys.ToList();


            foreach (var objectId in objectIdList)
            {
                //  source에 포함된 Object만을 대상으로 한다.
                if (source.ContainsKey(objectId) == false)
                    continue;


                //  Collection Key에 해당하는 데이터 가져오기
                DataObject obj = Collection.CachedObjects.GetData(objectId);
                JValue leftVal = null, compVal = null;


                //  값과 비교
                if (right is Value)
                {
                    leftVal = obj.GetValue(key);
                    compVal = new JValue(right.GetValue());
                }
                else if (right is ReferenceValue)
                {
                    leftVal = obj.GetValue(key);
                    compVal = obj.GetValue((right as ReferenceValue).Expr);
                }

                int comp = leftVal.CompareTo(compVal);
                if (IsTrue(comp, token.ComparisonOperator))
                    result.Add(obj.ObjectId, obj);
            }

            return result;
        }


        private void ExchangeArgument(ref Argument left, ref Argument right)
        {
            Argument tmp = left;
            left = right;
            right = tmp;
        }


        private bool IsTrue(int comp, string op)
        {
            if (op == "==" && comp == 0)
                return true;

            if (op == "!=" && comp != 0)
                return true;

            if (op == "<=" && comp <= 0)
                return true;

            if (op == "<" && comp < 0)
                return true;

            if (op == ">=" && comp >= 0)
                return true;

            if (op == ">" && comp > 0)
                return true;

            return false;
        }


        protected Dictionary<string, DataObject> GetResult(Where where)
        {
            Dictionary<string, DataObject> result = Collection.CachedObjects.GetAllData();
            if (where == null)
                return result;


            string compOP = "";
            foreach (var item in where.Tokens.Select((token, index) => new { index, token }))
            {
                if (item.token is ConditionToken)
                {
                    //  쿼리는 홀수 위치에 있을 수 없다.
                    if (item.index % 2 == 1)
                        throw new AegisException(RoseResult.InvalidArgument, $"Invalid query in the where clause.");

                    if (compOP == "" || compOP == "and")
                    {
                        result = ScanFromData(result, item.token as ConditionToken);
                    }
                    else if (compOP == "or")
                    {
                        //  Collection의 데이터로 Scan 후 Merge 수행
                        Dictionary<string, DataObject> records = Collection.CachedObjects.GetAllData();
                        ScanFromData(records, item.token as ConditionToken)
                            .ToList()
                            .ForEach(v => result[v.Key] = v.Value);
                    }
                }
                else if (item.token is string)
                {
                    //  operator는 짝수 위치에 있을 수 없다.
                    if (item.index % 2 == 0)
                        throw new AegisException(RoseResult.InvalidArgument, $"Invalid operator('{item.token}') in the where clause.");

                    compOP = (string)item.token;
                }
                else if (item.token is Where)
                {
                    //  쿼리는 홀수 위치에 있을 수 없다.
                    if (item.index % 2 == 1)
                        throw new AegisException(RoseResult.InvalidArgument, $"Invalid query in the where clause.");

                    if (compOP == "" || compOP == "and")
                    {
                        result = GetResult(item.token as Where);
                    }
                    else if (compOP == "or")
                    {
                        //  Merge 수행
                        GetResult(item.token as Where)
                            .ToList()
                            .ForEach(v => result[v.Key] = v.Value);
                    }
                }
            }

            return result;
        }
    }
}
