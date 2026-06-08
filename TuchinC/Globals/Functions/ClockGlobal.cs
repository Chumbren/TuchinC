using TuchinC.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.Generators;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Statements;

namespace TuchinC.Globals.Functions
{
    internal class ClockGlobal : IGeneratorFunction
    {
        public int Arity => 0;

        public Function Generate(List<Stmt?> args)
        {
            if (args.Count > 0)
                throw new ArgumentOutOfRangeException(nameof(args), args.Count, $"Превышенно число аргументов функции clock. {args.Count} > {Arity}");

            List<Modifier> modifiers = [
                new Modifier(new(TokenType.IDENTIFIER), ModifierType.Public), 
                new Modifier(new(TokenType.IDENTIFIER), ModifierType.Extern)];

            return new(new Token(TokenType.IDENTIFIER, "clock", null, -1), [], modifiers);

        }

        public override string ToString() => "<native fn>['clock']";
    }
}
