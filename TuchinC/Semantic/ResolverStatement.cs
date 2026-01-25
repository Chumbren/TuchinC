using TuchinC.AST;
using TuchinC.AST.Expressions;
using TuchinC.AST.Statements;
using TuchinC.AST.Statements.Visitors;
using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.Semantic
{
    internal partial class Resolver : IVisitor
    {
        private void Resolve(Stmt? stmt) => stmt?.Accept(this);


        public void VisitBlockStmt(Block stmt)
        {
            BeginScope();
            Resolve(stmt.Statements);
            EndScope();
        }

        public void VisitExpressionStmt(Expression stmt) => Resolve(stmt.Value);


        public void VisitFunctionStmt(Function stmt)
        {
            Declare(stmt.Name);
            Define(stmt.Name);

            ResolveFunction(stmt, FunctionType.FUNCTION);
        }

        private void ResolveFunction(
            Function function, FunctionType type)
        {
            FunctionType enclosingFunction = _currentFunction;
            _currentFunction = type;
            
            BeginScope();

            ResolveFunctionParams(function.Params);
            ResolveFunctionBody(function.Body);

            EndScope();
            _currentFunction = enclosingFunction;
        }



        private void ResolveFunctionParams(List<Token> @params)
        {
            foreach (Token param in @params)
            { 
                Declare(param);
                Define(param);
            }
        }

        private void ResolveFunctionBody(ISyntaxTree node)
        {
            if (node is Block block)
                Resolve(block.Statements);
            else if (node is Stmt statement)
                Resolve(statement);
            else
                Resolve((Expr)node);
        }

        public void VisitIfStmt(If stmt)
        {
            Resolve(stmt.Condition);

            foreach (var elif in stmt.ElifBranches)
                Resolve(elif.ThenBranch);

            Resolve(stmt.ThenBranch);
            if (stmt.ElseBranch != null)
                Resolve(stmt.ElseBranch);
        }

        public void VisitImportStmt(Use stmt)
        {}

        public void VisitReturnStmt(Return stmt)
        {
            if (_currentFunction == FunctionType.NONE)
                Tuchin.Error(stmt.Keyword, "Оператор 'return' не может находится выше функции");

            if (stmt.Value != null)
            {
                if(_currentFunction == FunctionType.INITIALIZER)
                {
                    Tuchin.Error(stmt.Keyword, "Иницилизатор не может возращать значения");
                }
                
                Resolve(stmt.Value);
            }
        }

        public void VisitLetStmt(Let stmt)
        {
            Declare(stmt.Name);
            if (stmt.Initializer != null)
                Resolve(stmt.Initializer);
            Define(stmt.Name);
        }

        public void VisitLoopStmt(Loop stmt)
        {
            if(stmt.Condition is not null)
                Resolve(stmt.Condition);
            Resolve(stmt.Body);
        }



        private void Declare(Token name)
        {
            if (_scopes.Count == 0) return;

            Dictionary<string, bool> scope = _scopes.Peek();
            
            if(scope.TryGetValue(name.Lexeme, out _))
            {
                Tuchin.Error(name, "Ошибка! переменая уже существует");
                return;
            }

            scope.Add(name.Lexeme, false);
        }

        private void Define(Token name)
        {
            if (_scopes.Count == 0) return;


            var scope = _scopes.Peek();
            scope[name.Lexeme] = true;
        }

        private void ResolveLocal(Expr expr, Token name)
        {
            int current = _scopes.Count-1;
            foreach (var scope in _scopes)
            {
                if (scope.ContainsKey(name.Lexeme))
                {
                    _interpreter.Resolve(expr, _scopes.Count - 1 - current);
                    return;
                }
                current--;
            }
        }

        public void VisitSwitchStmt(Switch stmt)
        {
            Resolve(stmt.Condition);

            foreach (var @case in stmt.Cases)
                Resolve(@case.Body);
        }

        public void VisitClassStmt(Struct stmt)
        {
            StructType enclosingClass = _currentClass;
            _currentClass = StructType.STRUCT;

            Declare(stmt.Name);
            Define(stmt.Name);

            BeginScope();

            ResolveThisClass();

            ResolveMethodsClass(stmt.Body);

            EndScope();

            _currentClass = enclosingClass;
        }


        private void ResolveMethodsClass(in List<Function> body)
        {
            foreach (var method in body)
            {
                FunctionType declaration = FunctionType.METHOD;

                if (method.Name.Lexeme == "init")
                    declaration = FunctionType.INITIALIZER;

                ResolveFunction(method, declaration);
            }
        }

        private void ResolveThisClass() => _scopes.Peek().Add("this", true);

    }
}
