using System.Collections.Generic;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions.Visitors;

namespace TuchinC.AST.Nodes.Expressions {
    public class Binary(Expr left, Token @operator, Expr right) : Expr(@operator)
    {

        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitBinaryExpr(this);

        public readonly Expr Left = left;
        public readonly Token Operator = @operator;
        public readonly Expr Right = right;

    }

}