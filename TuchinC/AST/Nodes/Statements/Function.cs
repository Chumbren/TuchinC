using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes;
using TuchinC.AST.Nodes.Statements.Visitors;

namespace TuchinC.AST.Nodes.Statements
{
    public enum FunctionType
    {
        NONE,
        FUNCTION,
        METHOD,
        INITIALIZER,
        SIGNATURE
    }

    public readonly struct Param(Token name, ValueType type)
    {
        public readonly Token Name = name;
        public readonly ValueType Type = type;
    }
    
    public class Function(Token name,List<Param> @params, List<Modifier> modifiers, ValueType? @return = null, SyntaxTree? body = null) : Stmt(name)
    {
        public readonly Token Name = name;
        public readonly List<Param> Params = @params;
        public readonly SyntaxTree? Body = body;
        public readonly ValueType @return = @return == null ? ValueType.ToEmpty() : (ValueType)@return;
        public readonly List<Modifier> Modifiers = modifiers;


        public Function(Token name, List<Param> @params, SyntaxTree body):this(name, @params, [], null, body) 
        { }

        public override void Accept(IVisitor visitor) => visitor.VisitFunctionStmt(this);
    }
}
