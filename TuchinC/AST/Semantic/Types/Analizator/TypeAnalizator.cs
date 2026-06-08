using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using TuchinC.AST.Nodes;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements;
using TuchinC.CodeAnalize;
using TuchinC.CodeAnalize.Emiters;
using TuchinC.Semantic;

namespace TuchinC.AST.Semantic.Types.Analizator
{
    internal partial class TypeAnalizator(Resolver resolver): EmitWaiter<ValueType>, IAnalizator 
    {
        private readonly Stack<Dictionary<string, Value>> _scopes = [];
        private readonly Resolver _resolver = resolver;
        public void Analize(List<Stmt?> stmts)
        {
            _scopes.Push([]);
            foreach (var stmt in stmts)
                stmt?.Accept(this);
            _scopes.Pop();
        }


        private void AddType(string name, Value value)
        {
            var scope = Peek();
            scope.Add(name, value);
        }

        private void AddVaribleType(string name, TypeValue type)
        {
            var scope = Peek();
            scope.Add(name, new Value(type));
        }

        private bool TryGetType(string name, out Value value)
        {
            foreach (var scope in _scopes)
            {
                if (scope.TryGetValue(name, out value))
                    return true;
            }

            value = new Value();

            return false;
        } 

        private bool TryGetPrimitiveType(string name, out TypeValue type)
        {
            type = TypeValue.None;
            return TryGetType(name, out Value value) && value.TryGetVariebleType(out type);
        } 

        private bool TryAssign(string name, Value value)
        {
            foreach (var scope in _scopes)
            {
                if (scope.TryGetValue(name, out _))
                {
                    scope[name] = value;
                    return true;
                }
            }

            return false;
        }

        private void Push() => _scopes.Push([]);
        private void Pop() => _scopes.Pop();
        private Dictionary<string, Value> Peek() => _scopes.Count>0?_scopes.Peek():[];
    }
}
