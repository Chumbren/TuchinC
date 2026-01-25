using TuchinC.AST.Statements.Visitors;
using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Statements
{
    public class Use : Stmt
    {
        public readonly Token Path;

        public Use(List<Token> names) 
        {
            int line = 0;
            StringBuilder builder = new();
            foreach (Token token in names)
            {
                if (line == 0)
                    line = token.Line;

                builder.Append($"{token.Lexeme}_");
            }

            string path = builder.ToString()[..(builder.Length - 2)];

            Path = new Token(TokenType.IDENTIFIER,path,null,line);
        }

        public override void Accept(IVisitor visitor) => visitor.VisitImportStmt(this);
    }
}
