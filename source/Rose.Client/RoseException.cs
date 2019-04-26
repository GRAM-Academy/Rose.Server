using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rose.Client
{

    public class RoseException : Exception
    {
        public int ResultCodeNo { get; private set; }



        public RoseException()
        {
        }


        public RoseException(string message)
            : base(message)
        {
        }


        public RoseException(int resultCode)
        {
            ResultCodeNo = resultCode;
        }


        public RoseException(int resultCode, string message)
            : base(message)
        {
            ResultCodeNo = resultCode;
        }


        public RoseException(Exception innerException, string message)
            : base(message, innerException)
        {
        }


        public RoseException(Exception innerException, int resultCode)
            : base("", innerException)
        {
            ResultCodeNo = resultCode;
        }


        public RoseException(int resultCode, Exception innerException, string message)
            : base(message, innerException)
        {
            ResultCodeNo = resultCode;
        }


        public RoseException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }


        public RoseException(int resultCode, string message, params object[] args)
            : base(string.Format(message, args))
        {
            ResultCodeNo = resultCode;
        }


        public RoseException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }


        public RoseException(int resultCode, Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            ResultCodeNo = resultCode;
        }


        public override string ToString()
        {
            string msg = string.Format("{0}\r\nResultCodeNo={1}\r\n{2}",
                base.Message, ResultCodeNo, StackTrace);

            return msg;
        }
    }
}
