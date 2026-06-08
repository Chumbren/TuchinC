using System;
using System.Collections.Generic;
using System.Text;

namespace TuchinC.AST.Semantic.Types.Cast.TypesBinnary
{
    public class CastDeclaration
    {
        /// <summary>
        /// Проверяет возможность приведения типа
        /// </summary>
        public static bool CanCast(TypeValue sourceType, TypeValue targetType)
        {
            // Если типы совпадают
            if (targetType == sourceType)
                return true;

            // Числовые типы: targetType должен быть МЕНЬШЕ sourceType
            if (IsNumberType(targetType) && IsNumberType(sourceType))
                return (byte)targetType <= (byte)sourceType;

            // Char в String
            if (targetType == TypeValue.Char && sourceType == TypeValue.String)
                return true;


            return false;
        }

        /// <summary>
        /// Проверка на числовой тип
        /// </summary>
        private static bool IsNumberType(TypeValue type)
        {
            return type == TypeValue.Int8 ||
                   type == TypeValue.Int16 ||
                   type == TypeValue.Int32 ||
                   type == TypeValue.Int64 ||
                   type == TypeValue.Double32 ||
                   type == TypeValue.Double64 ||
                   type == TypeValue.Double128;
        }
    }
}
