using System.Collections.Generic;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes;
using TuchinC.AST.Nodes.Expressions.Visitors;

namespace TuchinC.AST.Nodes.Expressions {
    public abstract class Expr(Token keyword):SyntaxTree(keyword)
    {
        public abstract T Accept<T>(IVisitor<T> visitor);
    }
}