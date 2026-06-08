using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Expressions.Visitors;

namespace TuchinC.AST.Nodes.Expressions.Calles
{
    public abstract class Call(Token paren, Expr calle) : Expr(paren)
    {
        public Expr Calle = calle;
       
        public Token Paren = paren;

        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitCallExpr(this);
    }
}
