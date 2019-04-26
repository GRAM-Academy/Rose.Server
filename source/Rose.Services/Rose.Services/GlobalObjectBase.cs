using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Data;
using Newtonsoft.Json.Linq;

namespace Rose.Services
{
    public class GlobalObjectBase
    {
        public string Name { get; internal set; }
        public TreeNode<string> Data { get; internal set; }





        protected GlobalObjectBase()
        {
        }


        public virtual void Initialize()
        {
        }
    }
}
