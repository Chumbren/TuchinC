using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.Interpreters;
using TuchinC.Objects.Callable;
using TuchinC.Semantic;
namespace TuchinC.Objects.Globals.Functions
{
    internal class InputGlobal :ILoxCallable 
    {
        private const int ARITY = 1;
        public int Arity() => ARITY;

        public object? Call(Interpreter interpreter, List<object?> args)
        {
            if (args.Count == 0)
                throw new ArgumentOutOfRangeException(args.ToString() ,
                    "Ожидался 1 аргумент но переданно ноль");

            var arg = args.FirstOrDefault();
            Console.WriteLine(arg != null ? arg.ToString() : "nil");
            
            var input = Console.ReadLine();
            return input;

        }
        public override string ToString() => "<native fn>['clock']";
    }
}
