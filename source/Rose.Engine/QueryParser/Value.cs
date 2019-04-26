using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Rose.Engine.QueryParser
{
    [DebuggerDisplay("Value={_value}")]
    internal class Value : Argument
    {
        private object _value;





        public Value(object val)
        {
            _value = val;
        }


        public object GetValue()
        {
            return _value;
        }


        public int CompareTo(Argument target)
        {
            return (_value as IComparable).CompareTo(target.GetValue());
        }
    }
}
