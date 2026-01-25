using System;
using System.Collections.Generic;
using System.Text;

namespace TuchinC.Semantic.Types.Exceptions
{
    public class TypeException(string message):Exception($"Ошибка приведения типов tuchin: {message}") 
    { }
}
