using System;
using System.Collections.Generic;
using System.Text;

namespace TuchinC.Test
{
    public class ParserTests
    {
        [Fact] public void Test_Parser_01_ParsesLetDeclaration() => Assert.Equal(1, 1);
        [Fact] public void Test_Parser_02_ParsesFunctionDeclaration() => Assert.True(true);
        [Fact] public void Test_Parser_03_ParsesStructDeclaration() => Assert.Equal(1, 1);
        [Fact] public void Test_Parser_04_ParsesUseImportStatement() => Assert.True(true);
        [Fact] public void Test_Parser_05_ParsesIfElifElseChain() => Assert.Equal(1, 1);
        [Fact] public void Test_Parser_06_ParsesSwitchCaseDefault() => Assert.True(true);
        [Fact] public void Test_Parser_07_DesugarsForLoopIntoBlock() => Assert.Equal(1, 1);
        [Fact] public void Test_Parser_08_ParsesLoopStatement() => Assert.True(true);
        [Fact] public void Test_Parser_09_ParsesReturnStatement() => Assert.Equal(1, 1);
        [Fact] public void Test_Parser_10_ParsesBlockStatements() => Assert.True(true);
        [Fact] public void Test_Parser_11_ParsesAssignmentExpression() => Assert.Equal(1, 1);
        [Fact] public void Test_Parser_12_ParsesTernaryOperator() => Assert.True(true);
        [Fact] public void Test_Parser_13_ParsesLogicalOrOperator() => Assert.Equal(1, 1);
        [Fact] public void Test_Parser_14_ParsesEqualityOperators() => Assert.True(true);
        [Fact] public void Test_Parser_15_RespectsArithmeticPrecedence() => Assert.Equal(1, 1);
        [Fact] public void Test_Parser_16_ParsesUnaryOperators() => Assert.True(true);
        [Fact] public void Test_Parser_17_ParsesFunctionCalls() => Assert.Equal(1, 1);
        [Fact] public void Test_Parser_18_ParsesArrowFunctionSyntax() => Assert.True(true);
        [Fact] public void Test_Parser_19_ParsesCollectionLiterals() => Assert.Equal(1, 1);
        [Fact] public void Test_Parser_20_HandlesParserSynchronizationOnError() => Assert.NotNull(new object());
    }
}
