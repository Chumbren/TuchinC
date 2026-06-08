using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Statements.Visitors;

namespace TuchinC.AST.Nodes.Statements
{
    public class Block(Token keyword, List<Stmt?> statements):Stmt(keyword)
    {

        public readonly List<Stmt?> Statements = statements;
        public override void Accept(IVisitor visitor) => visitor.VisitBlockStmt(this);

    }
}
