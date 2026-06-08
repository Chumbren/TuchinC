using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Semantic.Types;

namespace TuchinC.AST.Semantic.Types.Exceptions
{
    internal class NotCheckUnaryOperationException(string @operator, ValueType type )
        :TypeException($"Не удалось применить оператор '{@operator}' к типу " +
            $"'{(type.Type == TypeValue.Identifier?type.Name: type)}'")
    {}
}
