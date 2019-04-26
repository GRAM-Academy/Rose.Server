using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Newtonsoft.Json.Linq;

namespace Rose.Engine.QueryParser
{
    internal class Where
    {
        public List<object> Tokens { get; private set; } = new List<object>();





        public Where(JToken json)
        {
            if (json is JValue)
            {
                string jsonText = GetStringFromJValue(json as JValue);
                Tokens.Add(new ConditionToken(jsonText));
            }
            else if (json is JArray)
            {
                foreach (var item in json)
                {
                    if (item is JValue)
                    {
                        string jsonText = GetStringFromJValue(item as JValue);
                        if (jsonText == "and" || jsonText == "or")
                            Tokens.Add(jsonText);
                        else
                            Tokens.Add(new ConditionToken(jsonText));
                    }
                    else if (item is JArray)
                    {
                        Where where = new Where(item);
                        Tokens.Add(where);
                    }
                }
            }
            else
                throw new AegisException(RoseResult.InvalidArgument, "Invalid where clause.");
        }


        private string GetStringFromJValue(JValue json)
        {
            string jsonText = json.ToString(Newtonsoft.Json.Formatting.None);
            return jsonText.Substring(1, jsonText.Length - 2);
        }
    }
}
