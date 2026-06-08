using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Statements.Visitors;

namespace TuchinC.AST.Nodes.Statements
{
    public enum StructType
    {
        NONE,
        STRUCT
    }

    public class Struct(Token name,List<Function> body, List<Modifier> modifiers):Stmt(name)
    {
        public readonly Token Name = name;

        public readonly List<Function> Body = body;

        public readonly List<Modifier> Modifiers = modifiers;

        public Struct(Token name, List<Function> body):this(name, body, [])
        { }

        public override void Accept(IVisitor visitor) => visitor.VisitClassStmt(this);

    }
}
