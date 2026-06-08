using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions;

namespace TuchinC.AST.Nodes.Expressions.Calles
{
    internal class FunctionCall(Expr calle, Token paren,List<Expr> args):Call(paren, calle)
    {
       public readonly List<Expr> Arguments = args;
    }
}
