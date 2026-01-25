using TuchinC.Semantic;
using TuchinC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = TuchinC.Semantic.Environment;
using TuchinC.Interpreters;

namespace TuchinC.Objects.Callable
{
    public class LoxCollection(List<object?> collection) : ILoxCallable
    {
        public int Count() => collection.Count;
        public int Arity() => 1;


        public object? Call(Interpreter interpreter, List<object?> args)
            => args[0] is not null ? collection[Convert.ToInt32(args[0])] : null;

        public override string ToString() 
        {
            StringBuilder builder = new("[ ");
            for (int i = 0;i < collection.Count ; i++)
                builder.Append($"{collection[i]}"+(i < collection.Count-1?", ":' '));
            
            builder.Append(']');

            return builder.ToString();
        }
    }
}
