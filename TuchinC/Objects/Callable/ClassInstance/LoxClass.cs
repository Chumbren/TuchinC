using TuchinC.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.Interpreters;
using TuchinC.Objects.Callable;

namespace TuchinC.Objects.Callable.ClassInstance
{
    public class LoxClass(string name, Dictionary<string, LoxFunction> methods) : ILoxCallable
    {
        private readonly Dictionary<string, LoxFunction> Methods = methods;
        public readonly string Name = name;
        public int Arity()
        {
            LoxFunction? init = FindMethod("init");
            if(init == null) return 0;
            return init.Arity();
        }

        public object? Call(Interpreter interpreter, List<object?> args)
        {
            LoxInstance instance = new(this);
            FindMethod("init")?.Bind(instance).Call(interpreter, args);

            return instance;
        }


        public LoxFunction? FindMethod(string name)
        {
            Methods.TryGetValue(name, out LoxFunction? method);
            return method;
        }

        public override string ToString() => Name;

    }
}
