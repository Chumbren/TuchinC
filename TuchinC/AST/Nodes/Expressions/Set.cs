using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions.Visitors;

namespace TuchinC.AST.Nodes.Expressions
{
    public class Set(Token name, Expr @object, Expr value) : Expr(name)
    {
        public readonly Expr Object = @object;
        public readonly Token Name = name;
        public readonly Expr Value = value;
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitSetExpr(this);
    }
}
