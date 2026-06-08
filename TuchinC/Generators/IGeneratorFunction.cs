using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST;
using TuchinC.AST.Nodes.Statements;

namespace TuchinC.Generators
{
    internal interface IGeneratorFunction:IGeneratorWithParam<Function, List<Stmt?>>
    {
        int Arity { get; }
    }
}
