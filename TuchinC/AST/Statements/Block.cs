using TuchinC.AST.Statements.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Statements
{
    public class Block(List<Stmt?> statements):Stmt
    {

        public readonly List<Stmt?> Statements = statements;
        public override void Accept(IVisitor visitor) => visitor.VisitBlockStmt(this);

    }
}
