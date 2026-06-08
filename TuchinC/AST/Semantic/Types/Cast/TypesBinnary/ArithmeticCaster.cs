using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Lexical;

namespace TuchinC.AST.Semantic.Types.Cast.TypesBinnary
{
    public class ArithmeticCaster
    {
        /// <summary>
        /// Выполняет кастинг для арифметических операций (+, -, *, /, %, и т.д.)
        /// </summary>
        public static ValueType Cast(ValueType left, ValueType right, TokenType @operator)
        {
            // Сложение с участием строки или char
            if (@operator == TokenType.PLUS)
            {
                if (left.IsString() || right.IsString())
                    return ValueType.GetToString();

                if (left.IsChar() && right.IsChar())
                    return ValueType.GetToString();

                if (left.IsChar() && right.IsString())
                    return ValueType.GetToString();

                if (left.IsString() && right.IsChar())
                    return ValueType.GetToString();
            }

            // Чисто числовые операции
            if (IsNumberType(left.Type) && IsNumberType(right.Type))
            {
                TypeValue resultType = GetResultType(left.Type, right.Type);
                return new ValueType(resultType);
            }

            return ValueType.ToEmpty();
        }

        /// <summary>
        /// Получение результирующего типа для арифметической операции
        /// </summary>
        private static TypeValue GetResultType(TypeValue left, TypeValue right)
        {

            if (IsDoubleType(left) || IsDoubleType(right))
            {
                if (left == TypeValue.Double128 || right == TypeValue.Double128)
                    return TypeValue.Double128;
                if (left == TypeValue.Double64 || right == TypeValue.Double64)
                    return TypeValue.Double64;
                return TypeValue.Double32;
            }

            if (IsIntegerType(left) && IsIntegerType(right))
            {
                if (left == TypeValue.Int64 || right == TypeValue.Int64)
                    return TypeValue.Int64;
                if (left == TypeValue.Int32 || right == TypeValue.Int32)
                    return TypeValue.Int32;
                if (left == TypeValue.Int16 || right == TypeValue.Int16)
                    return TypeValue.Int16;
                return TypeValue.Int8;
            }

            if (IsIntegerType(left) && IsDoubleType(right))
                return right;
            if (IsDoubleType(left) && IsIntegerType(right))
                return left;

            return TypeValue.Int32; 
        }

        private static bool IsIntegerType(TypeValue type)
        {
            return type == TypeValue.Int8 ||
                   type == TypeValue.Int16 ||
                   type == TypeValue.Int32 ||
                   type == TypeValue.Int64;
        }

        private static bool IsDoubleType(TypeValue type)
        {
            return type == TypeValue.Double32 ||
                   type == TypeValue.Double64 ||
                   type == TypeValue.Double128;
        }

        private static bool IsNumberType(TypeValue type)
        {
            return IsIntegerType(type) || IsDoubleType(type);
        }
    }
}
