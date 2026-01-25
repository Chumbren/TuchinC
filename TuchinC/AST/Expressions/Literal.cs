using System.Collections.Generic;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions {
    public class Literal(object? value) : Expr
    {
        public readonly ValueType Type = ValueType.ToPrimitive(value);
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitLiteralExpr(this);

        public readonly object? Value = value;

    }

}