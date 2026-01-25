using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Expressions;
using TuchinC.AST.Expressions.Calles;
using TuchinC.AST.Expressions.Visitors;
using TuchinC.Lexical;
using TuchinC.Semantic.Types;
using TuchinC.Semantic.Types.Exceptions;

namespace TuchinC.Types.Checker
{
    internal partial class TypeChecker : IVisitor<object?>
    {

        public void Check(Expr expr) => expr.Accept(this);

        public object? VisitArrowFunction(ArrowFunction expr)
        {
            throw new NotImplementedException();
        }

        public object? VisitAssignExpr(Assign expr)
        {
            throw new NotImplementedException();
        }

        public object? VisitBinaryExpr(Binary expr)
        {
            throw new NotImplementedException();
        }

        public object? VisitCallExpr(Call expr)
        {
            throw new NotImplementedException();
        }

        public object? VisitCollectionExpr(Collection expr)
        {
            throw new NotImplementedException();
        }

        public object? VisitGetExpr(Get expr)
        {
            throw new NotImplementedException();
        }

        public object? VisitGroupingExpr(Grouping expr) => null;

        public object? VisitLiteralExpr(Literal expr)
        {
            _types.Push(expr.Type);
            return null;
        }



        public object? VisitLogicalExpr(Logical logical)
        {
            CheckCountType(2);
            ValueType right = _types.Pop();
            ValueType left = _types.Pop();
             

            return null;
        }

        public object? VisitSetExpr(Set expr)
        {
            throw new NotImplementedException();
        }

        public object? VisitTernaryExpr(Ternary expr)
        {
            throw new NotImplementedException();
        }

        public object? VisitThisExpr(This expr)
        {
            throw new NotImplementedException();
        }

        public object? VisitUnaryExpr(Unary expr)
        {
            CheckCountType(1);
            ValueType value = _types.Pop();
            if (expr.Operator.Type == TokenType.NOT_BIT)
                CheckUnary(expr.Operator, value, value.IsNumber());
            else if (expr.Operator.Type == TokenType.MINUS)
                CheckUnary(expr.Operator,  value, value.IsNumber());
            else if (expr.Operator.Type == TokenType.PLUS)
                CheckUnary(expr.Operator, value, value.IsNumber());
            else if (expr.Operator.Type == TokenType.NOT_BIT)
                CheckUnary(expr.Operator, value, value.IsBoolean());

            return null;
        }


        private static void CheckUnary(Token @operator, ValueType type, bool condition)
        {
            if (!condition)
                Tuchin.Error(@operator,
                    new NotCheckUnaryOperationException(@operator.Lexeme, type));
        }
        public object? VisitVariableExpr(Variable expr)
        {

            throw new NotImplementedException();
        }
    }
}
