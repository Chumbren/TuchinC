using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions
{
    public class Set(Expr @object,Token name, Expr value) : Expr
    {
        public readonly Expr Object = @object;
        public readonly Token Name = name;
        public readonly Expr Value = value;
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitSetExpr(this);
    }
}
