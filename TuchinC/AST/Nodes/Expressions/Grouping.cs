using System.Collections.Generic;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions.Visitors;

namespace TuchinC.AST.Nodes.Expressions {
    public class Grouping(Token keyword, Expr Expression) : Expr(keyword)
    {
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitGroupingExpr(this);

        public readonly Expr Expression = Expression;

    }

}