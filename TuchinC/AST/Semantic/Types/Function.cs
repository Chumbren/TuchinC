using System;
using System.Collections.Generic;
using System.Text;

namespace TuchinC.AST.Semantic.Types
{
    public readonly struct Function(string name, List<ValueType> args, TypeValue @return)
    {
        public readonly string Name = name;
        public readonly List<ValueType> Args = args;
        public readonly TypeValue ReturnType = @return;
    }
}
