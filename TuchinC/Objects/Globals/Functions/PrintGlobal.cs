using TuchinC.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.Interpreters;
using TuchinC.Objects.Callable;

namespace TuchinC.Objects.Globals.Functions
{
    internal class PrintGlobal : ILoxCallable
    {
        private const int ARITY = 1;
        public int Arity() => ARITY;

        public object? Call(Interpreter interpreter, List<object?> args)
        {
            string? value =  args[0]?.ToString();
            Console.WriteLine(value is not null ? value : "nil");
            return null;
        }

        public override string ToString() => "<native fn>['print']";

    }
}
