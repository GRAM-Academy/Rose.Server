using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rose.Engine
{
    public static class Settings
    {
        public static readonly string EngineName = Assembly.GetExecutingAssembly().GetName().Name;
        public static readonly Version EngineVersion = Assembly.GetExecutingAssembly().GetName().Version;
        public static readonly Encoding Encoding = Encoding.UTF8;


        public static int MaxSchemeCount { get; internal set; }
        public static int MaxCollectionCount { get; internal set; }
        public static int MaxCollectionSize { get; internal set; }
        public static int MaxObjectSize { get; internal set; }

        public static StringComparer StringComparer { get; internal set; } = StringComparer.OrdinalIgnoreCase;
        public static StringComparison StringComparison { get; internal set; } = StringComparison.OrdinalIgnoreCase;
    }
}
