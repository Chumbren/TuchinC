using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements.Visitors;

namespace TuchinC.AST.Nodes.Statements
{
    public class Print(Token keyword, Expr expr) : Stmt(keyword)
    {
        public readonly Expr Expression = expr;
        public override void Accept(IVisitor visitor) => visitor.VisitPrintStmt(this);
    }
}
