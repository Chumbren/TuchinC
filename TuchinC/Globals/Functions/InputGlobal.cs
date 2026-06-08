using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Nodes.Statements;
using TuchinC.Generators;
using TuchinC.Semantic;
namespace TuchinC.Globals.Functions
{
    internal class InputGlobal :IGeneratorFunction 
    {
        public int Arity => 1;

        public Function Generate(List<Stmt?> args)
        {
            throw new NotImplementedException();
        }

        public override string ToString() => "<native fn>['clock']";
    }
}
