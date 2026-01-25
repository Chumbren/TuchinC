using TuchinC.AST.Statements.Visitors;
using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Statements
{
    public enum FunctionType
    {
        NONE,
        FUNCTION,
        METHOD,
        INITIALIZER
    }

    public class Function(Token name,List<Token> _params, ISyntaxTree body) : Stmt
    {
        public readonly Token Name = name;
        public readonly List<Token> Params = _params;
        public readonly ISyntaxTree Body = body;
        public override void Accept(IVisitor visitor) => visitor.VisitFunctionStmt(this);
    }
}
