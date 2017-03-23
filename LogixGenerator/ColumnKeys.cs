using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixGenerator
{
    internal static class TagColumnKeys
    {
        internal const string Rack = "Rack";
        internal const string Slot = "Slot";
        internal const string Channel = "Channel";
        internal const string LoopType = "Loop_Type";
        internal const string LoopNumber = "Loop_Number";
        internal const string LoopSuffix = "Loop_Suffix";
        internal const string Tag = "Tag";
        internal const string Description = "Description";
        internal const string Enable = "Enable";
    }

    internal static class DIColumnKeys
    {
        internal const string Enable = "Enable";
        internal const string Deb = "Deb (ms)";
    }

    internal static class AIColumnKeys
    {
        internal const string Units = "Units";
        internal const string EULow = "EU_Low";
        internal const string EUHigh = "EU_High";

        internal const string LLSP = "LL_SP";
        internal const string LLEnable = "LL_Enable";
        internal const string LLDeb = "LL_Deb (ms)";

        internal const string LSP = "L_SP";
        internal const string LEnable = "L_Enable";
        internal const string LDeb = "L_Deb (ms)";

        internal const string HSP = "H_SP";
        internal const string HEnable = "H_Enable";
        internal const string HDeb = "H_Deb (ms)";

        internal const string HHSP = "HH_SP";
        internal const string HHEnable = "HH_Enable";
        internal const string HHDeb = "HH_Deb (ms)";
    }

    class StringHelp
    {
        public static string StringCheckZero(string s)
        {
            return (s == null || s == "") ? "0" : s;
        }
        public static bool StringEmpty(string s)
        {
            return s == null || s == "";
        }
    }
}
