using TuchinC.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.Generators;
using TuchinC.AST.Nodes.Statements;

namespace TuchinC.Globals.Functions
{
    internal class PrintGlobal : IGeneratorFunction
    {

        public int Arity => 1;

        public Function Generate(List<Stmt?> param)
        {
            throw new NotImplementedException();
        }

        public override string ToString() => "<native fn>['print']";

    }
}
