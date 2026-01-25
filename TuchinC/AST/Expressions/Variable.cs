using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.AST.Expressions
{
    public class Variable(Token name) : Expr
    {
        public readonly Token Name = name;

        public override int GetHashCode() => Name.Lexeme.GetHashCode();
        public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitVariableExpr(this);
    }
    
}
