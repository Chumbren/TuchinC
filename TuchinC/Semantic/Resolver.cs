using TuchinC.AST.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.Interpreters;
using TuchinC.Objects.Globals;
using TuchinC.Types.Checker;

namespace TuchinC.Semantic
{
    internal partial class Resolver(Interpreter interpreter)
    {
        private readonly TypeChecker _typeChecker = new(); 
        private readonly Interpreter _interpreter = interpreter;
        private readonly Stack<Dictionary<string, bool>> _scopes = [];
        private readonly List<string> _globals = Global.GetNamesGlobal();
        private FunctionType _currentFunction = FunctionType.NONE;
        private StructType _currentClass = StructType.NONE;

        

        internal void Analize(List<Stmt?> statements)
        {
            BeginScope();
            Resolve(statements);
            EndScope();
        }

        private void Resolve(List<Stmt?> statements)
        {
            foreach (Stmt? statement in statements)
                Resolve(statement);
        }

        private void BeginScope() => _scopes.Push([]);

        private void EndScope() => _scopes.Pop();

        
    }
}
