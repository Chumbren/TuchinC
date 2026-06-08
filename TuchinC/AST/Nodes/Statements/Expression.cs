using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements.Visitors;

namespace TuchinC.AST.Nodes.Statements
{
    public class Expression(Expr expr) : Stmt(expr.Keyword)
    {
        public readonly Expr Value = expr;
        public override void Accept(IVisitor visitor) => visitor.VisitExpressionStmt(this);
    }
}
