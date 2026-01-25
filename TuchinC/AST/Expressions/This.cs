using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions
{
    public class This(Token token) : Expr
    {
        public readonly Token Keyword = token;

        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitThisExpr(this);
    }
}

