using System;
using System.Collections.Generic;
using System.Text;

namespace TuchinC.AST.Semantic.Types
{
    public readonly struct Value
    {
        private readonly TypeValue? _varieble;
        private readonly Function? _function;
        
        public Value(TypeValue value)
        {
            _varieble = value;
            _function = null;
        }

        public Value(Function function)
        {
            _function = function;
            _varieble = null;
        }

        public readonly bool IsFunction() => _function != null;
        public readonly bool IsVarieble() => _varieble != null;

        public readonly bool TryGetVariebleType(out TypeValue value)
        {
            if(!IsVarieble() || _varieble == null)
            {
                value = TypeValue.None;
                return false;
            }

            value = (TypeValue)_varieble;
            return true;
        }

    }
}
