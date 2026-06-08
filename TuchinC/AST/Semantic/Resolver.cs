using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements;
using TuchinC.AST.Semantic.Types.Analizator;
using TuchinC.CodeAnalize;
using TuchinC.Globals;

namespace TuchinC.Semantic
{
    public partial class Resolver
    {
        private readonly Dictionary<Expr, int?> _locals = [];
        private readonly TypeAnalizator _typeAnalizator; 
        private readonly Stack<Dictionary<string, bool>> _scopes = [];
        private readonly List<string> _globals = Global.GetNamesGlobal();
        private FunctionType _currentFunction = FunctionType.NONE;
        private StructType _currentStruct = StructType.NONE;

        public Resolver()
        {
            _typeAnalizator = new(this);
        }

        public IReadOnlyDictionary<Expr, int?> Locals => _locals.AsReadOnly();
        
        private void ResolveLocals(Expr expr, int? depth) => _locals.Add(expr, depth);

        public void Analize(List<Stmt?> statements)
        {
            BeginScope();
            Resolve(statements);
            EndScope();

            _typeAnalizator.Analize(statements);
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
