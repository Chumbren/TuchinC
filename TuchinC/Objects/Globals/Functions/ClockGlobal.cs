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
    internal class ClockGlobal : ILoxCallable
    {
        private const int ARITY = 0;
        public int Arity() => ARITY;

        public object? Call(Interpreter interpreter, List<object?> args)
            => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000.0;

        public override string ToString() => "<native fn>['clock']";
    }
}
