using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rose.Engine.Utils
{
    public class Helpers
    {
        public static string Base64Encoding(string source, Encoding encoding)
        {
            byte[] arr = encoding.GetBytes(source);
            return Convert.ToBase64String(arr);
        }


        public static string Base64Decoding(string source, Encoding encoding)
        {
            byte[] arr = Convert.FromBase64String(source);
            return encoding.GetString(arr);
        }
    }
}
