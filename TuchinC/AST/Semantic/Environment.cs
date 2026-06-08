using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.Exceptions;
namespace TuchinC.AST.Semantic
{
    public class Environment(Environment? enclosing = null)
    {
        public readonly Environment? Enclosing = enclosing;
        private readonly Dictionary<string, object?> _values = [];


        public void GetPair()
        {
            foreach (var item in _values)
            {
                Console.WriteLine($"pair {item.Key}: key:{item.Value?.GetType()}");
            }
        }


        public void Define(string name, object? value) => _values.Add(name, value);

        public object? Get(string name)
        {
            if (_values.TryGetValue(name, out object? value))
                return value;

            return null;
        }

        public object? Get(Token name)
        {
            if (_values.TryGetValue(name.Lexeme, out object? value))
                return value;

            if (Enclosing != null) return Enclosing.Get(name);

            throw new RuntimeError(name,
                $"Неизвестная переменая '{name.Lexeme}' .");

        }

        internal object? GetAt(int? distance, string name)
        {
            var ancestor = Ancestor(distance);

            if (ancestor == null) return null;


            if (!ancestor._values.TryGetValue(name, out object? value))
                throw new ArgumentOutOfRangeException($"Отсуствует элемент {name}!");


            return value;
        }

        internal Environment? Ancestor(int? distance)
        {
            Environment? environment = this;
            for (int i = 0; i < distance; i++)
                environment = environment?.Enclosing;


            return environment;
        }

        public void Assign(Token name, object? value)
        {
            if (_values.ContainsKey(name.Lexeme))
            {
                _values[name.Lexeme] = value;
                return;
            }

            if (Enclosing != null)
            {
                Enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeError(name,
                $"Переменная '{name.Lexeme}' не определена .");
        }

        internal void AssignAt(int? distance, Token name, object? value) =>
            Ancestor(distance)?._values[name.Lexeme] = value;
    }
}
