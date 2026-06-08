using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements.Visitors;
using TuchinC.AST.Semantic.Types;

namespace TuchinC.AST.Nodes.Statements
{
    public class Let(Token name, TypeValue type, Expr? init) : Stmt(name)
    {
        private TypeValue _type = type;
        public readonly Token Name = name;
        public TypeValue Type => _type;
        public readonly Expr? Initializer = init;


        public bool IsShadowType() => _type == TypeValue.None;

        public void SetType(TypeValue type) 
        {
            if (!IsShadowType())
                return;

            _type = type;
        } 

        public override void Accept(IVisitor visitor) => visitor.VisitLetStmt(this);
    }
}
