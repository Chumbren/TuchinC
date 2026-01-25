using TuchinC.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.Interpreters;

namespace TuchinC.Objects.Callable
{
    public interface ILoxCallable
    {
        int Arity();
        object? Call(Interpreter interpreter, List<object?> args);
    }
}
