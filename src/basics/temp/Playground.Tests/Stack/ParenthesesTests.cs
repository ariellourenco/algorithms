namespace Playground.Tests.Stack;

public sealed class ParenthesesTests
{
    [Theory]
    [InlineData("[()]{}")]
    [InlineData("{[()()]()}")]
    [InlineData("[()]{}{[]}()")]
    [InlineData("[()]{}{[()()]()}")]
    public void Parentheses_ReturnsTrue_ForBalancedExpression(string expression) =>
        Assert.True(Parentheses.IsBalanced(expression));
    
    [Theory]
    [InlineData("[(]{}")]
    [InlineData("{[()]()")]
    [InlineData("[()]{}{]}()")]
    [InlineData("[(){}{[())]()}")]
    public void Parentheses_ReturnsFalse_ForUnbalancedExpression(string expression) =>
        Assert.False(Parentheses.IsBalanced(expression));
}