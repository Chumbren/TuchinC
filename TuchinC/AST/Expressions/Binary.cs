using System.Collections.Generic;
using TuchinC.AST.Expressions.Visitors;
using TuchinC.Lexical;



namespace TuchinC.AST.Expressions {
    public class Binary(Expr left, Token _operator, Expr right) : Expr
    {

        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitBinaryExpr(this);

        public readonly Expr Left = left;
        public readonly Token Operator = _operator;
        public readonly Expr Right = right;

    }

}