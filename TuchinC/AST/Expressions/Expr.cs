using System.Collections.Generic;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions {
    public abstract class Expr:ISyntaxTree
    {
        public abstract T Accept<T>(IVisitor<T> visitor);
    }
}