namespace Algorithms.Basics.Stack;

public static class Parentheses
{
    /// <summary>
    /// Exercise 1.3.4
    /// Evaluates a fully parenthesized expressions to ensure that its parentheses are properly balanced.
    /// Where {, (, and [ are opening brackets and }, ), and ] are closing brackets.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    public static bool IsBalanced(string expression)
    {
        var stack = new Stack<char>();

        if ((expression[0] == ')') || 
            (expression[0] == ']') || 
            (expression[0] == '}'))
            return false;

        foreach (var element in expression)
        {
            if ((element == '(') || (element == '[') || (element == '{'))
            {
                stack.Push(element);
            }
            else
            {
                if (!stack.TryPop(out var left))
                    return false;

                if ((left == '(' && element != ')') || 
                    (left == '[' && element != ']') || 
                    (left == '{' && element != '}')) 
                    return false;
            }
        }
        
        return stack.Count == 0;
    }
}