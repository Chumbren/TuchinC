using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes;
using TuchinC.AST.Nodes.Statements.Visitors;

namespace TuchinC.AST.Nodes.Statements
{
    public abstract class Stmt(Token keyword):SyntaxTree(keyword)
    {
        public abstract void Accept(IVisitor visitor);
    }
}
