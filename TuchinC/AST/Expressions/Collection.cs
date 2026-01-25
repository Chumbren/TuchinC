using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions
{
    public class Collection(List<Expr> elements) : Expr
    {
        public readonly List<Expr> Elements = elements;
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitCollectionExpr(this);
    }
}
