using System.Collections.Generic;
using TuchinC.AST.Expressions.Visitors;
using TuchinC.Lexical;



namespace TuchinC.AST.Expressions {
    public class Unary(Token Operator,Expr Right) : Expr
    {

        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitUnaryExpr(this);

        public readonly Expr Right = Right;
        public readonly Token Operator = Operator;

    }

}