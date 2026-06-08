using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions.Visitors;

namespace TuchinC.AST.Nodes.Expressions
{
    public class Ternary(Expr @if,Expr that,Expr @else) : Expr(@if.Keyword)
    {
        public readonly Expr If = @if;
        public readonly Expr That = that;
        public readonly Expr Else = @else;

        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitTernaryExpr(this);
    }
}
