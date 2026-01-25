using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions
{

    public class ArrowFunction(List<Token> _params, ISyntaxTree body) : Expr
    {
        public readonly List<Token> Params = _params;
        public readonly ISyntaxTree Body = body;
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitArrowFunction(this);
    }

}
