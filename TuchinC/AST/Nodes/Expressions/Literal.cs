using System.Collections.Generic;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions.Visitors;

namespace TuchinC.AST.Nodes.Expressions {
    public class Literal(Token @this, object? value) : Expr(@this)
    {
        public readonly ValueType Type = ValueType.ToPrimitive(value);
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitLiteralExpr(this);

        public readonly object? Value = value;

    }

}