using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions
{
    public class Ternary(Expr _if,Expr that,Expr _else) : Expr
    {
        public readonly Expr If = _if;
        public readonly Expr That = that;
        public readonly Expr Else = _else;

        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitTernaryExpr(this);
    }
}
