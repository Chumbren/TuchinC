using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.Lexical;

namespace TuchinC.Semantic.Types.Exceptions
{
    internal class NotCheckUnaryOperationException(string @operator, ValueType type )
        :TypeException($"Не удалось применить оператор '{@operator}' к типу '{type}'")
    {}
}
