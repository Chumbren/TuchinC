using TuchinC.AST.Expressions;
using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Expressions.Calles
{
    internal class IteratorCall(Expr calle, Token paren, Expr index) : Call(calle, paren)
    {
        public readonly Expr Index = index;
    }
}
