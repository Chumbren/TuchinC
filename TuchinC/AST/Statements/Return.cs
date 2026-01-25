using TuchinC.AST.Expressions;
using TuchinC.AST.Statements.Visitors;
using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Statements
{
    public class Return(Token keyword, Expr? value) : Stmt
    {
       public readonly Token Keyword = keyword;
       public readonly Expr? Value = value;

       public override void Accept(IVisitor visitor) => visitor.VisitReturnStmt(this);
    }
}
