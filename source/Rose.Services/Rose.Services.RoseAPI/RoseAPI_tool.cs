using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Data.Json;
using Rose.Engine.Cache;
using Newtonsoft.Json.Linq;

namespace Rose.Services
{
    public partial class RoseAPI
    {
        private object _seqNoLock = new object();

        public static JToken sequenceNumber(string sequenceName, long increase)
        {
            var scheme = SchemeCatalog.GetScheme("rose");
            var collection = scheme.GetCollection("sequenceNumbers");
            long seqNo;


            using (collection.WriterLock)
            {
                //  sequenceName 이름의 데이터 확인
                var token = Select("rose", "sequenceNumbers", $"{sequenceName} >= 0", null, null, 0, 1);
                if (token == null || token.Count() == 0)
                {
                    //  없으면 생성
                    Insert(scheme.Name, collection.Name, null, new JObject()
                    {
                        { sequenceName, (long)0 }
                    });

                    token = Select("rose", "sequenceNumbers", $"{sequenceName} >= 0", null, null, 0, 1);
                }


                //  값 증가
                seqNo = (long)token.First().GetProperty(sequenceName).Value + increase;
                token.First().Replace(new JObject()
                {
                    { sequenceName, seqNo }
                });
            }

            return new JObject()
            {
                { sequenceName, seqNo }
            };
        }


        [DispatchMethod("sequenceNumber")]
        private void sequenceNumber(RequestHandlerArgument arg)
        {
            JToken json = JToken.Parse(arg.MessageBody);
            var scheme = SchemeCatalog.GetScheme("rose");
            var collection = scheme.GetCollection("sequenceNumbers");
            string sequenceName = (string)json.GetProperty("name");
            long increase = (long)(json.GetProperty("condition/increase", false)?.Value ?? 1);
            var result = sequenceNumber(sequenceName, increase);


            arg.Response(result);
        }


        public static JToken now()
        {
            DateTime now = DateTime.Now;
            return new JObject()
            {
                { "string", now.ToString("yyyy/M/d HH:mm:ss") },
                { "OADate", now.ToOADate() }
            };
        }


        [DispatchMethod("now")]
        private void now(RequestHandlerArgument arg)
        {
            arg.Response(now());
        }


        public static JToken random()
        {
            int value = Aegis.Calculate.Randomizer.NextNumber();
            return new JObject()
            {
                { "value", value }
            };
        }


        [DispatchMethod("random")]
        private void random(RequestHandlerArgument arg)
        {
            arg.Response(random());
        }
    }
}
