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

namespace Rose.Engine.QueryParser
{
    internal class QuerySelect : QueryBase
    {
        public readonly SortKey[] SortKeys;
        public readonly int RangeStart, RangeCount;

        public QuerySelect(Execute.Executer executer, JObject query)
            : base(executer, query)
        {
            //  sortKey 절
            var sortKey = JsonQuery.GetProperty("sortKey", false)?.Value;
            if (sortKey != null)
            {
                if (sortKey is JValue)
                {
                    var item = ((string)sortKey).Split(' ');
                    if (item.Count() == 0 || item.Count() > 2)
                        throw new AegisException(RoseResult.InvalidArgument, "'sortKey' is must have one or two arguments.");

                    string orderBy = (item.Count() >= 2 ? item[1] : "asc");
                    if (orderBy != "asc" && orderBy != "desc")
                        throw new AegisException(RoseResult.InvalidArgument, $"'orderBy' is must be 'asc' or 'desc'.");

                    SortKeys = new SortKey[1];
                    SortKeys[0] = new SortKey()
                    {
                        Key = item[0],
                        OrderBy = orderBy
                    };
                }
                else if (sortKey is JArray)
                {
                    int index = 0;
                    SortKeys = new SortKey[sortKey.Count()];

                    foreach (var key in sortKey)
                    {
                        var item = ((string)key).Split(' ');
                        if (item.Count() == 0 || item.Count() > 2)
                            throw new AegisException(RoseResult.InvalidArgument, "'sortKey' is must have one or two arguments.");

                        string orderBy = (item.Count() >= 2 ? item[1] : "asc");
                        if (orderBy != "asc" && orderBy != "desc")
                            throw new AegisException(RoseResult.InvalidArgument, $"'orderBy' is must be 'asc' or 'desc'.");

                        SortKeys[index++] = new SortKey()
                        {
                            Key = item[0],
                            OrderBy = orderBy
                        };
                    }
                }
            }


            //  range 절
            var range = JsonQuery.GetProperty("range", false)?.Value as JArray;
            if (range != null)
            {
                if (range.Count() != 2)
                    throw new AegisException(RoseResult.InvalidArgument, $"'range' is must have two arguments.");

                RangeStart = range.ElementAt(0).ToString().ToInt32();
                RangeCount = range.ElementAt(1).ToString().ToInt32();
                if (RangeStart < 0 || RangeCount < 0)
                    throw new AegisException(RoseResult.InvalidArgument, $"Argument of 'range' is not valid.");
            }
            else
            {
                RangeStart = -1;
                RangeCount = -1;
            }
        }


        public override List<DataObject> Execute()
        {
            using (Collection.ReaderLock)
            {
                List<DataObject> result = GetResult(Where).Values.ToList();


                //  sort 절
                if (SortKeys != null)
                {
                    foreach (var sort in SortKeys)
                    {
                        if (sort.OrderBy == "asc")
                        {
                            result = result.OrderBy(v => v.GetValue(sort.Key))
                                           .ToList();
                        }
                        else
                        {
                            result = result.OrderByDescending(v => v.GetValue(sort.Key))
                                           .ToList();
                        }
                    }
                }


                //  range 절
                if (RangeStart >= 0)
                {
                    int startIndex = RangeStart, count = RangeCount;


                    //  start값이 배열범위를 넘을경우 result는 없다.
                    if (startIndex >= result.Count())
                        result.Clear();
                    else
                    {
                        //  count값이 배열크기를 넘지 않도록 보정
                        if (startIndex + count >= result.Count())
                            count = result.Count() - startIndex;

                        result = result.GetRange(startIndex, count);
                    }
                }

                return result;
            }
        }
    }


    internal struct SortKey
    {
        public string Key { get; set; }
        public string OrderBy { get; set; }
    }
}
