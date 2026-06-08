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
    public class Return(Token keyword, Expr? value) : Stmt(keyword)
    {
       public readonly Expr? Value = value;

       public override void Accept(IVisitor visitor) => visitor.VisitReturnStmt(this);
    }
}
