using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rose.Engine.QueryParser
{
    internal class ReferenceValue : Argument
    {
        public string Expr { get; private set; }





        public ReferenceValue(string expr)
        {
            Expr = expr;
        }


        public object GetValue()
        {
            return 0;
        }


        public int CompareTo(Argument target)
        {
            return (GetValue() as IComparable).CompareTo(target.GetValue());
        }
    }
}
