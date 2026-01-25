using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.Exceptions
{
    internal class Returned(object? value) : Exception()
    {
        public readonly object? Value = value;
    }
}
