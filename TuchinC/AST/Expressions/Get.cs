using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions
{
    public class Get(Expr @object, Token name) : Expr
    {
        public readonly Expr Object = @object;
        public readonly Token Name = name;
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitGetExpr(this);
    }
}
