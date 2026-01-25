using TuchinC.AST.Expressions;
using TuchinC.AST.Expressions.Calles;
using TuchinC.AST.Statements;
using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.Semantic
{
    internal partial class Resolver : IVisitor<object?>
    {

        private void Resolve(Expr? expr) 
        {
            expr?.Accept(this);
            if(expr != null) _typeChecker.Check(expr);
        }

        public object? VisitArrowFunction(ArrowFunction expr)
        {
            int i = 0;
            while (_scopes.Count>0 && _scopes.Peek().TryGetValue($"ArrowFunction{++i}<>",out _));

            string name = $"ArrowFunction{i}<>";
            Token token = new(TokenType.IDENTIFIER, name, null, 0);
            Function generated = new(token, expr.Params, expr.Body);

            Resolve(generated);
            return null;
        }

        public object? VisitAssignExpr(Assign expr)
        {
            ResolveVaryble(expr.Name);
            Resolve(expr.Value);
            ResolveLocal(expr,expr.Name);
            return null;
        }

        public object? VisitBinaryExpr(Binary expr)
        {
            Resolve(expr.Left);
            Resolve(expr.Right);

            return null;
        }

        public object? VisitCallExpr(Call expr)
        {

            Resolve(expr.Calle);
            if (expr is FunctionCall function)
            {
                ResolveFunctionCall(function);
            }
            else if (expr is IteratorCall iterator)
            {
                ResolveIteratorCall(iterator);
            }
                

            return null;
        }

        private void ResolveFunctionCall(FunctionCall function)
        {
            foreach (Expr argument in function.Arguments)
                Resolve(argument);
        }

        private void ResolveIteratorCall(IteratorCall iterator) => Resolve(iterator.Index);

        public object? VisitCollectionExpr(Collection expr)
        {
            foreach (Expr element in expr.Elements)
                Resolve(element);

            return null;
        }

        public object? VisitGroupingExpr(Grouping expr) 
        {
            Resolve(expr.Expression);
            return null;
        }

        public object? VisitLiteralExpr(Literal expr) => null;

        public object? VisitLogicalExpr(Logical logical)
        {
            Resolve(logical.Left);
            Resolve(logical.Right);
            return null;
        }

        public object? VisitTernaryExpr(Ternary expr)
        {
            Resolve(expr.If);
            Resolve(expr.That);
            Resolve(expr.Else);
            return null;
        }

        public object? VisitUnaryExpr(Unary expr)
        {
            Resolve(expr.Right);
            return null;
        }

        public object? VisitVariableExpr(Variable expr)
        {
            ResolveVaryble(expr.Name);
            ResolveLocal(expr, expr.Name);
            return null;
        }

        private void ResolveVaryble(Token name)
        {
            foreach (var scope in _scopes)
            {
                if (scope.ContainsKey(name.Lexeme))
                {
                    if (scope[name.Lexeme] == false)
                    {
                        Tuchin.Error(name,
                         "Не возможно прочитать локальную переменую в её собственном иницилизаторе");
                    }
                    return;
                }
            }

            if (!_globals.Contains(name.Lexeme)) 
            { 
                Tuchin.Error(name,
                          $"Переменная '{name.Lexeme}' отсутствует");
            }

        }


        public object? VisitGetExpr(Get expr)
        {
            Resolve(expr.Object);
            return null;
        }

        public object? VisitSetExpr(Set expr)
        {
            Resolve(expr.Value);
            Resolve(expr.Object);
            return null;
        }

        public object? VisitThisExpr(This expr)
        {
            if(_currentClass == StructType.NONE)
            {
                Tuchin.Error(expr.Keyword, "Нельзя использовать 'this' вне структуры");
                return null;
            }


            ResolveLocal(expr,expr.Keyword);
            return null;
        }
    }
}
