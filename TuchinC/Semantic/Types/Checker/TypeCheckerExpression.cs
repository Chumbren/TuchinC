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
            CheckCountType(2);
            ValueType right = _types.Pop();
            ValueType left = _types.Pop();

            if (!CheckBinnary(ref left, ref right, expr.Operator.Type))
                Tuchin.Error(expr.Operator,
                     new NotCheckBinnaryOperationException(expr.Operator.Lexeme, left, right
                     ));

            _types.Push(CastBinnaryType(ref left, ref right, expr.Operator.Type));

            return null;
        }

        

        public static bool CheckBinnary(ref ValueType left, ref ValueType right, TokenType @operator)
            => CheckBinnaryPrimitive(ref left, ref right) && 
                (CheckBinnaryBoolean(ref left, ref right) || 
                CheckBinnaryAlpha(ref left, ref right, @operator) ||
                CheckBinnaryNumber(ref left, ref right, @operator));


        public static bool CheckBinnaryPrimitive(ref ValueType left, ref ValueType right)
            => left.IsPrimitive() && right.IsPrimitive();
        public static bool CheckBinnaryBoolean(ref ValueType left, ref ValueType right)
            => left.IsBoolean() && right.IsBoolean();

        public static bool CheckBinnaryAlpha(ref ValueType left,  ref ValueType right, TokenType @operator)
            => left.IsAlpha() && @operator == TokenType.PLUS && (right.IsAlpha() || right.IsNumber());
        
        public static bool CheckBinnaryNumber(ref ValueType left, ref ValueType right, TokenType @operator)
            => left.IsNumber() && (CheckBinnaryAlpha(ref right, ref left, @operator) ||  right.IsNumber());


        private static ValueType CastBinnaryType(ref ValueType left, ref ValueType right, TokenType @operator)
        {
            if (left.IsChar())
                return CastBinnaryChar(ref left, ref right, @operator);
            else if (left.IsString())
                return CastBinnaryString(ref left, ref right, @operator);
            else if (left.IsNumber())
                return CastBinnaryNumber(ref left, ref right, @operator);

            throw new ArgumentOutOfRangeException(right.Type.ToString(),
                    "Тип не поддерживается");
        }

        private static ValueType CastBinnaryChar(ref ValueType left, ref ValueType right, TokenType @operator)
        {
            if (@operator == TokenType.PLUS)
            {
                if (right.IsChar() || right.IsString())
                    return ValueType.GetToString();
                else if (right.IsInt8())
                    return ValueType.ToByte();
                else if (right.IsInt16())
                    return ValueType.ToShort();
                else if (right.IsInt32())
                    return ValueType.ToInt();
                else if (right.IsInt64())
                    return ValueType.ToLong();
                else if (right.IsFloat())
                    return ValueType.ToFloat();
                else if (right.IsDouble())
                    return ValueType.ToDouble();
                else if (right.IsDecimal())
                    return ValueType.ToDecimal();

            }

            

            throw new ArgumentOutOfRangeException(right.Type.ToString(),
                "Тип не поддерживается");
        }

        private static ValueType CastBinnaryString(ref ValueType left, ref ValueType right, TokenType @operator)
        {
            if (@operator == TokenType.PLUS)
                return ValueType.GetToString();


            throw new ArgumentOutOfRangeException(right.Type.ToString(),
                "Операция не поддерживается");
        }

        private static ValueType CastBinnaryNumber(ref ValueType left, ref ValueType right, TokenType @operator)
        {
            if(@operator == TokenType.PLUS)
            {
                if (right.IsChar())
                    return left;
                else if (right.IsString())
                    return ValueType.GetToString();
               
            }

            if (right.IsInt8())
                return ValueType.ToByte();
            else if (right.IsInt16())
                return ValueType.ToShort();
            else if (right.IsInt32())
                return ValueType.ToInt();
            else if (right.IsInt64())
                return ValueType.ToLong();
           else if (right.IsFloat())
                return ValueType.ToFloat();
            else if (right.IsDouble())
                return ValueType.ToDouble();
            else if (right.IsDecimal())
                return ValueType.ToDecimal();
            else
                throw new ArgumentOutOfRangeException(right.Type.ToString(),
                    "Тип не поддерживается");
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



        public object? VisitLogicalExpr(Logical expr)
        {
            CheckCountType(2);
            ValueType right = _types.Pop();
            ValueType left = _types.Pop();

            if (!CheckLogical(ref left, ref right))
                Tuchin.Error(expr.Operator,
                     new NotCheckBinnaryOperationException(expr.Operator.Lexeme, left, right
                     ));

            _types.Push(ValueType.ToBoolean());

            return null;
        }

        private static bool CheckLogical(ref ValueType left, ref ValueType right)
            => left.IsPrimitive() || right.IsPrimitive() ||
                !left.IsAlpha() || !right.IsAlpha() ||
                left.IsBoolean() || right.IsBoolean();


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
            ValueType value = _types.Peek();

            if (CheckUnary(ref value, expr.Operator))
                Tuchin.Error(expr.Operator,
                  new NotCheckUnaryOperationException(expr.Operator.Lexeme, value));



            return null;
        }


        private static bool CheckUnary(ref ValueType value, Token @operator)
            => (@operator.Type == TokenType.NOT_BIT && !value.IsNumber()) ||
                (@operator.Type == TokenType.MINUS && !value.IsNumber()) ||
                (@operator.Type == TokenType.BANG && value.IsBoolean()) ||
                (@operator.Type == TokenType.PLUS && value.IsNumber());
 

        public object? VisitVariableExpr(Variable expr)
        {
            throw new NotImplementedException();
        }
    }
}
