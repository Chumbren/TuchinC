using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;

namespace TuchinC.AST.Nodes
{
    public abstract class SyntaxTree(Token keyword) 
    {
        public readonly Token Keyword = keyword;
    }
}
