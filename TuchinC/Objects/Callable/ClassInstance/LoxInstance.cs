using TuchinC.Lexical;
using TuchinC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = TuchinC.Semantic.Environment;
using TuchinC.Objects.Callable;

namespace TuchinC.Objects.Callable.ClassInstance
{
    public class LoxInstance(LoxClass @struct)
    {
        private readonly Dictionary<string, object?> _fields = [];
        private readonly LoxClass @struct = @struct;

        public object? Get(Token name)  
        {

            if (_fields.TryGetValue(name.Lexeme, out object? value))
                return value;

            LoxFunction? method = @struct.FindMethod(name.Lexeme);
            if (method != null) return method.Bind(this);


            throw new RuntimeError(name, $"В экземпляре отсутствует свойство '{name.Lexeme}'.");
        }

        

        public void Set(Token name, object? value) 
        {

            if (_fields.TryGetValue(name.Lexeme, out object? _))
                _fields[name.Lexeme] = value;

            throw new RuntimeError(name, $"В экземпляре отсутствует свойство '{name.Lexeme}'.");
        } 

        public override string ToString() => $"{@struct.Name} instance";
    }
}
