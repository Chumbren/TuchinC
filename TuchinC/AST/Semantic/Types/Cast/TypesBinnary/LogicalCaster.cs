using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Lexical;

namespace TuchinC.AST.Semantic.Types.Cast.TypesBinnary
{
    public class LogicalCaster
    {
        /// <summary>
        /// Проверяет возможность выполнения логической операции
        /// </summary>
        public static bool CanCast(ValueType left, ValueType right, TokenType @operator)
        {
            // Операции сравнения (==, !=, <, >, <=, >=)
            if (IsComparisonOperator(@operator))
            {
                return CanCompare(left, right);
            }

            // Логические операции (&&, ||)
            if (IsLogicalOperator(@operator))
            {
                return left.IsBoolean() && right.IsBoolean();
            }

            // Унарная операция NOT (!)
            if (@operator == TokenType.BANG)
            {
                return left.IsBoolean();
            }

            return false;
        }

        private static bool IsLogicalOperator(TokenType @operator)
        {
            return @operator == TokenType.AND ||
                   @operator == TokenType.VLINE_VLINE;
        }

        private static bool IsComparisonOperator(TokenType @operator)
        {
            return @operator == TokenType.EQUAL_EQUAL ||
                   @operator == TokenType.BANG_EQUAL ||
                   @operator == TokenType.LESS ||
                   @operator == TokenType.GREATER ||
                   @operator == TokenType.LESS_EQUAL ||
                   @operator == TokenType.GREATER_EQUAL;
        }

        private static bool CanCompare(ValueType left, ValueType right)
        {
            // Числа можно сравнивать
            if (left.IsNumber() && right.IsNumber())
                return true;

            // Логические значения можно сравнивать
            if (left.IsBoolean() && right.IsBoolean())
                return true;

            // Символы можно сравнивать
            if (left.IsChar() && right.IsChar())
                return true;

            

            

          

            return false;
        }
    }
}
