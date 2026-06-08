using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions.Visitors;

namespace TuchinC.AST.Nodes.Expressions
{
    public class This(Token token) : Expr(token)
    {
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitThisExpr(this);
    }
}

