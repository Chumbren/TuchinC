using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Semantic.Types;

namespace TuchinC.AST.Semantic.Types.Exceptions
{
    internal class NotCheckBinnaryOperationException(string @operator, ValueType left, ValueType right)
        : TypeException($"Не удалось применить оператор '{@operator}' " +
            $"к типу '{$"'{(left.Type == TypeValue.Identifier ? left.Name : left)}'"}'" + ' ' +
            $"и {$"'{(right.Type == TypeValue.Identifier ? right.Name : right)}'"}")
    { }
}
