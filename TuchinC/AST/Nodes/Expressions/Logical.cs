using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions.Visitors;

namespace TuchinC.AST.Nodes.Expressions
{
    public class Logical(Expr left,Token @operator, Expr right):Expr(@operator)
    {
        public readonly Expr Left = left;
        public readonly Token Operator = @operator;
        public readonly Expr Right = right;
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitLogicalExpr(this);

    }
}
