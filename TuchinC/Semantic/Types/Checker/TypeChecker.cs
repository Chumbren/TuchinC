using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using TuchinC.Semantic.Types;

namespace TuchinC.Types.Checker
{
    internal partial class TypeChecker
    {
        private readonly Stack<ValueType> _types = [];


        private void CheckCountType(int range)
        {
            if (_types.Count < range)
                throw new ArgumentOutOfRangeException(range.ToString(), 
                    "Нарушена целостность стека типов или состояние системы типов");
        }
    }
}
