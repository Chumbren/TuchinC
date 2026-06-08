using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions;

namespace TuchinC.AST.Nodes.Expressions.Calles
{
    internal class IteratorCall(Expr calle, Token paren, Expr index) : Call(paren, calle)
    {
        public readonly Expr Index = index;
    }
}
