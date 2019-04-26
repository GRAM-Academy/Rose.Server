using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Aegis.Data.Json;

namespace Rose.Engine.Cache
{
    internal class DataObject
    {
        public readonly string ObjectId;
        public string Data { get; private set; }
        public Stopwatch LastAccessTime { get; set; }   //  #! Cache out용으로 사용 예정





        public static DataObject NewObject(string objectId, JToken data)
        {
            return new DataObject(objectId, data.ToString(Newtonsoft.Json.Formatting.None));
        }


        public static DataObject NewObject(string objectId, string data)
        {
            return new DataObject(objectId, data);
        }


        private DataObject(string objectId, string data)
        {
            ObjectId = objectId;
            Data = data;
        }


        public bool ContainsKey(string key)
        {
            lock (this)
            {
                JToken data = JToken.Parse(Data);
                if (data.GetProperty(key, false) != null)
                    return true;
            }

            return false;
        }


        public JValue GetValue(string key)
        {
            lock (this)
            {
                JToken data = JToken.Parse(Data);
                JToken val = data.GetProperty(key, false)?.Value;
                if (val == null)
                    return null;

                if (val is JValue)
                    return (val as JValue);

                return null;
            }
        }


        public void ReplaceData(JToken data)
        {
            lock (this)
            {
                Data = data.DeepClone().ToString(Newtonsoft.Json.Formatting.None);
            }
        }
    }
}
