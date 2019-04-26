using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Rose.Client
{
    [DebuggerDisplay("ResultCode={ResultCode} Message={Message}")]
    public class RoseResult
    {
        public const int Ok = 0;

        //  Request Error
        public const int InvalidReqest = 101;
        public const int UnknownCommand = 102;
        public const int DataSizeTooLarge = 103;


        //  Collection Error
        public const int InvalidRoute = 110;
        public const int InvalidCollectionName = 111;
        public const int InvalidSchemeName = 112;


        //  Etc Error
        public const int InvalidArgument = 131;
        public const int InvalidHandler = 132;
        public const int NameLengthTooLong = 133;
        public const int DuplicateKey = 134;
        public const int DuplicateName = 135;
        public const int InvalidAssembly = 136;


        public const int UnknownError = 190;
        public const int NetworkError = 191;
        public const int ServerError = 192;





        public int ResultCode { get; private set; }
        public double ProcessTime { get; private set; }
        public string Message { get; private set; }
        public JToken Response { get; private set; }



        public RoseResult(string result)
        {
            Response = JObject.Parse(result).DeepClone();

            JProperty prop;
            prop = Response.GetProperty("resultCode", false);
            if (prop != null)
                ResultCode = int.Parse((string)prop.Value);
            else
                ResultCode = 0;


            prop = Response.GetProperty("message", false);
            if (prop != null)
                Message = (string)prop.Value;
            else
                Message = null;


            prop = Response.GetProperty("processingTime", false);
            if (prop != null)
                ProcessTime = (double)prop.Value;
        }


        public RoseResult(int resultCode, string message)
        {
            ResultCode = resultCode;
            Message = message;
            Response = new JObject()
            {
                { "resultCode", ResultCode },
                { "message", Message }
            };
        }
    }
}
