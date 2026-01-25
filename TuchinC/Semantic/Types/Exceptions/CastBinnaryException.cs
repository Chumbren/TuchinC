using System;
using System.Collections.Generic;
using System.Text;

namespace TuchinC.Semantic.Types.Exceptions
{
    internal class CastBinnaryException(ValueType type1, ValueType type2)
        :TypeException($"Не удалось преобразовать '{type1}' в {type2}")
    {}
}
