﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rose.Engine.QueryParser
{
    internal interface Argument
    {
        object GetValue();
        int CompareTo(Argument target);
    }
}
