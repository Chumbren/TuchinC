using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.Objects.Globals.Functions;

namespace TuchinC.Objects.Globals
{
    internal static class Global
    {
        private readonly static Dictionary<string, object> _globals = new(){
            ["print"] = new PrintGlobal(),
            ["input"] = new InputGlobal(),
            ["clock"] = new ClockGlobal(),
        };


        public static List<string> GetNamesGlobal() => [.. _globals.Select(el => el.Key)];
        
        public static Dictionary<string, object> GetGlobals() => _globals; 
    }
}
