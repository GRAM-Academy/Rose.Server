using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Rose.Client
{
    public static class JsonExtensions
    {
        public static JObject ToJObject(object obj)
        {
            return JObject.Parse(JsonConvert.SerializeObject(obj));
        }


        public static JProperty GetProperty(this JToken src, string path, bool exceptionWhenResultIsNull = true)
        {
            JToken currentToken = null;
            foreach (var key in path.Split(new char[] { '/', '\\' }))
            {
                if (currentToken == null)
                {
                    if (src is JValue || src is JArray)
                        return null;
                    currentToken = src[key];
                }
                else
                {
                    if (currentToken is JValue || currentToken is JArray)
                        return null;
                    currentToken = currentToken[key];
                }
            }


            if (exceptionWhenResultIsNull == true && currentToken == null)
                throw new Exception(string.Format("'{0}' key is not contains.", path));

            if (currentToken != null)
                return (currentToken.Parent as JProperty);
            return null;
        }
    }
}
