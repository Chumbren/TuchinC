using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions
{
    public class Logical(Expr left,Token _operator, Expr right) : Expr
    {

        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitLogicalExpr(this);

        public readonly Expr Left = left;
        public readonly Token Operator = _operator;
        public readonly Expr Right = right;
    }
}
