using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements.Visitors;

namespace TuchinC.AST.Nodes.Statements
{
    public class Loop(Token keyword, Expr? condition,Stmt? body) : Stmt(keyword)
    {
        public readonly Expr? Condition = condition;
        public readonly Stmt? Body = body;

        public override void Accept(IVisitor visitor) =>visitor.VisitLoopStmt(this);
    }
}
