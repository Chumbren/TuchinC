using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions.Calles
{
    public abstract class Call(Expr calle,Token paren) : Expr
    {
        public Expr Calle = calle;
       
        public Token Paren = paren;

        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitCallExpr(this);
    }
}
