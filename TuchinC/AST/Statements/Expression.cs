using TuchinC.AST.Expressions;
using TuchinC.AST.Statements.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Statements
{
    public class Expression(Expr expr) : Stmt
    {
        public readonly Expr Value = expr;
        public override void Accept(IVisitor visitor) => visitor.VisitExpressionStmt(this);
    }
}
