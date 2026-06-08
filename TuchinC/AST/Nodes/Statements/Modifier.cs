using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Lexical;

namespace TuchinC.AST.Nodes.Statements
{
    public enum ModifierType
    {
        Public,
        Private,
        Extern
    }
    public class Modifier(Token token, ModifierType name)
    {
        public readonly Token Token = token;
        public readonly ModifierType Name = name;
    }
}
