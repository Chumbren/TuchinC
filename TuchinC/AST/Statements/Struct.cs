using TuchinC.AST.Statements.Visitors;
using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Statements
{
    public enum StructType
    {
        NONE,
        STRUCT
    }

    public class Struct(Token name,List<Function> body):Stmt
    {
        public Token Name = name;

        public List<Function> Body = body;

        public override void Accept(IVisitor visitor) => visitor.VisitClassStmt(this);
    }
}
