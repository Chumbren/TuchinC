using TuchinC.AST.Statements.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Statements
{
    public abstract class Stmt:ISyntaxTree
    {
        public abstract void Accept(IVisitor visitor);
    }
}
