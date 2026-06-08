using System;
using System.Collections.Generic;
using System.Text;

namespace TuchinC.AST.Semantic.Types.Exceptions
{
    internal class CastBinnaryException(ValueType current, ValueType match)
        :TypeException($"Не удалось преобразовать '{current}' в {match}")
    {}
}
