using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Nodes;

namespace TuchinC.Generators
{

    internal interface IGenerator<TReturn> where TReturn : SyntaxTree
    {
        TReturn Generate();
    }
    
    internal interface IGeneratorWithParam<TReturn, KParam> where TReturn: SyntaxTree
    {
        TReturn Generate(KParam param);
    }
}
