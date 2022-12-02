namespace Playground.Tests.Stack;

public sealed class PostfixTests
{
    [Theory]
    [InlineData("1 + 2", "12+")]
    [InlineData("1 + 2 - 3", "12+3-")]
    [InlineData("1 + 2 - 3 + 5", "12+3-5+")]
    [InlineData("A + B * C + D", "ABC*+D+")]
    [InlineData("(A + B) * C - D", "AB+C*D-")]
    [InlineData("((A + B) - C * (D / E)) + F", "AB+CDE/*-F+")]
    public void CanCovertFromInfix(string infix, string postfix) => 
        Assert.Equal(postfix, Postfix.ToPostfix(infix));

    [Theory]
    [InlineData("12+", 3)]
    [InlineData("12+3-", 0)]
    [InlineData("12+3-5+", 5)]
    public void EvaluatesReturnExpectedResults(string expression, int result) =>
        Assert.Equal(result, Postfix.Evaluate(expression));
}