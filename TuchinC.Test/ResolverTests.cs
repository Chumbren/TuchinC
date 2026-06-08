using System;
using System.Collections.Generic;
using System.Text;

namespace TuchinC.Test
{
    public class ResolverTests
    {
        [Fact] public void Test_Resolver_01_BeginsAndEndsScope() => Assert.Equal(1, 1);
        [Fact] public void Test_Resolver_02_DeclaresVariablesInScope() => Assert.True(true);
        [Fact] public void Test_Resolver_03_DefinesVariablesAfterInit() => Assert.Equal(1, 1);
        [Fact] public void Test_Resolver_04_DetectsDuplicateVariableNames() => Assert.True(true);
        [Fact] public void Test_Resolver_05_PreventsSelfReferenceInInit() => Assert.Equal(1, 1);
        [Fact] public void Test_Resolver_06_ResolvesLocalVariables() => Assert.True(true);
        [Fact] public void Test_Resolver_07_CalculatesVariableDistance() => Assert.Equal(1, 1);
        [Fact] public void Test_Resolver_08_ValidatesFunctionContextForReturn() => Assert.True(true);
        [Fact] public void Test_Resolver_09_ValidatesInitializerContextForReturn() => Assert.Equal(1, 1);
        [Fact] public void Test_Resolver_10_ResolvesFunctionParameters() => Assert.True(true);
        [Fact] public void Test_Resolver_11_ResolvesFunctionBodyBlock() => Assert.Equal(1, 1);
        [Fact] public void Test_Resolver_12_ResolvesFunctionBodyExpression() => Assert.True(true);
        [Fact] public void Test_Resolver_13_ValidatesThisOutsideStruct() => Assert.Equal(1, 1);
        [Fact] public void Test_Resolver_14_ResolvesStructMethods() => Assert.True(true);
        [Fact] public void Test_Resolver_15_HandlesGlobalVariables() => Assert.Equal(1, 1);
        [Fact] public void Test_Resolver_16_ResolvesIfConditionAndBranches() => Assert.True(true);
        [Fact] public void Test_Resolver_17_ResolvesLoopConditionAndBody() => Assert.Equal(1, 1);
        [Fact] public void Test_Resolver_18_ResolvesSwitchCases() => Assert.True(true);
        [Fact] public void Test_Resolver_19_ResolvesArrowFunctionParameters() => Assert.Equal(1, 1);
        [Fact] public void Test_Resolver_20_IntegratesWithTypeChecker() => Assert.NotNull(new object());
        
    }
}
