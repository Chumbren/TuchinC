using TuchinC.AST.Expressions;
using TuchinC.AST.Statements.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Statements
{
    public class Loop(Expr? condition,Stmt? body) : Stmt
    {
        public readonly Expr? Condition = condition;
        public readonly Stmt? Body = body;

        public override void Accept(IVisitor visitor) =>visitor.VisitLoopStmt(this);
    }
}
