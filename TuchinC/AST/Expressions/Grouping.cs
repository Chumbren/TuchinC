using System.Collections.Generic;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions {
    public class Grouping(Expr Expression) : Expr
    {

        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitGroupingExpr(this);

        public readonly Expr Expression = Expression;

    }

}