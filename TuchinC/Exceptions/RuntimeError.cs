using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.Exceptions
{
    public class RuntimeError(Token token,string message):Exception(message)
    {
        public readonly Token Token = token;
    }
}
