using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions.Visitors;

namespace TuchinC.AST.Nodes.Expressions
{
    public class Collection(Token keyword, List<Expr> elements) : Expr(keyword)
    {
        public readonly List<Expr> Elements = elements;
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitCollectionExpr(this);
    }
}
