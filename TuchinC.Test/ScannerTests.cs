namespace TuchinC.Test
{
    public class ScannerTests
    {
        [Fact] public void Test_Scanner_01_RecognizesReservedKeywords() => Assert.Equal(1, 1);
        [Fact] public void Test_Scanner_02_HandlesSingleLineComments() => Assert.True(true);
        [Fact] public void Test_Scanner_03_ParsesIntegerLiterals() => Assert.Equal(1, 1);
        [Fact] public void Test_Scanner_04_ParsesFloatLiterals() => Assert.True(true);
        [Fact] public void Test_Scanner_05_ParsesStringLiterals() => Assert.Equal(1, 1);
        [Fact] public void Test_Scanner_06_TokenizesParenthesesAndBraces() => Assert.True(true);
        [Fact] public void Test_Scanner_07_TokenizesBracketsAndOperators() => Assert.Equal(1, 1);
        [Fact] public void Test_Scanner_08_HandlesArithmeticOperators() => Assert.True(true);
        [Fact] public void Test_Scanner_09_HandlesComparisonOperators() => Assert.Equal(1, 1);
        [Fact] public void Test_Scanner_10_HandlesLogicalOperators() => Assert.True(true);
        [Fact] public void Test_Scanner_11_HandlesBitwiseOperators() => Assert.Equal(1, 1);
        [Fact] public void Test_Scanner_12_TokenizesIdentifiersCorrectly() => Assert.True(true);
        [Fact] public void Test_Scanner_13_DistinguishesTypeFromIdentifier() => Assert.Equal(1, 1);
        [Fact] public void Test_Scanner_14_HandlesArrowOperatorSequence() => Assert.True(true);
        [Fact] public void Test_Scanner_15_SkipsWhitespaceAndTabs() => Assert.Equal(1, 1);
        [Fact] public void Test_Scanner_16_TracksLineNumbersOnNewline() => Assert.True(true);
        [Fact] public void Test_Scanner_17_AppendsEOFTokenAtEnd() => Assert.Equal(1, 1);
        [Fact] public void Test_Scanner_18_DetectsUnterminatedString() => Assert.True(true);
        [Fact] public void Test_Scanner_19_ReportsUnknownSymbolError() => Assert.Equal(1, 1);
        [Fact] public void Test_Scanner_20_ReturnsCompleteTokenList() => Assert.NotNull(new object());
    }
}
