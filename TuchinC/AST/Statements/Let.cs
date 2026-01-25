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
    public class Let(Token name,ValueType type, Expr? init) : Stmt
    {
        public readonly Token Name = name;
        public readonly ValueType Type = type;
        public readonly Expr? Initializer = init;

        public override void Accept(IVisitor visitor) => visitor.VisitLetStmt(this);
    }
}
