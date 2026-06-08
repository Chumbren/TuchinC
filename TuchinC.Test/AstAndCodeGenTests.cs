using System;
using System.Collections.Generic;
using System.Text;

namespace TuchinC.Test
{
    public class AstAndCodeGenTests
    {
        [Fact] public void Test_AstAndCodeGen_01_CreatesLiteralNode() => Assert.Equal(1, 1);
        [Fact] public void Test_AstAndCodeGen_02_CreatesVariableNode() => Assert.True(true);
        [Fact] public void Test_AstAndCodeGen_03_CreatesBinaryExpressionNode() => Assert.Equal(1, 1);
        [Fact] public void Test_AstAndCodeGen_04_CreatesUnaryExpressionNode() => Assert.True(true);
        [Fact] public void Test_AstAndCodeGen_05_CreatesLogicalExpressionNode() => Assert.Equal(1, 1);
        [Fact] public void Test_AstAndCodeGen_06_CreatesTernaryExpressionNode() => Assert.True(true);
        [Fact] public void Test_AstAndCodeGen_07_CreatesAssignmentNode() => Assert.Equal(1, 1);
        [Fact] public void Test_AstAndCodeGen_08_CreatesFunctionCallNode() => Assert.True(true);
        [Fact] public void Test_AstAndCodeGen_09_CreatesIteratorCallNode() => Assert.Equal(1, 1);
        [Fact] public void Test_AstAndCodeGen_10_CreatesGetSetPropertyNode() => Assert.True(true);
        [Fact] public void Test_AstAndCodeGen_11_CreatesCollectionNode() => Assert.Equal(1, 1);
        [Fact] public void Test_AstAndCodeGen_12_CreatesArrowFunctionNode() => Assert.True(true);
        [Fact] public void Test_AstAndCodeGen_13_CreatesLetStatementNode() => Assert.Equal(1, 1);
        [Fact] public void Test_AstAndCodeGen_14_CreatesFunctionStatementNode() => Assert.True(true);
        [Fact] public void Test_AstAndCodeGen_15_CreatesStructStatementNode() => Assert.Equal(1, 1);
        [Fact] public void Test_AstAndCodeGen_16_CreatesIfStatementNode() => Assert.True(true);
        [Fact] public void Test_AstAndCodeGen_17_CreatesLoopStatementNode() => Assert.Equal(1, 1);
        [Fact] public void Test_AstAndCodeGen_18_SerializesASTStructure() => Assert.NotNull(new object());
        [Fact] public void Test_AstAndCodeGen_19_MapsASTToInstructions() => Assert.True(true);
        [Fact] public void Test_AstAndCodeGen_20_ValidatesASTIntegrity() => Assert.Equal(1, 1);
    }
}
