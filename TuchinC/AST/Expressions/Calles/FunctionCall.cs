using TuchinC.AST.Expressions;
using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Expressions.Calles
{
    internal class FunctionCall(Expr calle, Token paren,List<Expr> args):Call(calle,paren)
    {
       public readonly List<Expr> Arguments = args;
    }
}
