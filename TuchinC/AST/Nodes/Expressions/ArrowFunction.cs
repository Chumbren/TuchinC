using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes;
using TuchinC.AST.Nodes.Expressions.Visitors;
using TuchinC.AST.Nodes.Statements;

namespace TuchinC.AST.Nodes.Expressions
{

    public class ArrowFunction(Token keyword, List<Token> @params, SyntaxTree body): Expr(keyword)
    {
        public readonly List<Token> Params = @params;
        public readonly SyntaxTree? Body = body;
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitArrowFunction(this);
    }

}
