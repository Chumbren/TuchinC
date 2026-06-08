using System.Collections.Generic;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions.Visitors;

namespace TuchinC.AST.Nodes.Expressions {
    public class Unary(Token @operator,Expr right) : Expr(right.Keyword)
    {

        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitUnaryExpr(this);

        public readonly Expr Right = right;
        public readonly Token Operator = @operator;

    }

}