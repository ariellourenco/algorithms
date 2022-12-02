namespace Playground;

public static class Parentheses
{
    /// <summary>
    /// Parentheses expression evaluation.
    /// Evaluates a fully parenthesized expressions to ensure that its parentheses are properly balanced.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    public static bool IsBalanced(string expression)
    {
        var stack = new Stack<char>();
        var supportedElements = ")]}".ToCharArray();

        foreach (var element in expression)
        {
            if (element == '{') stack.Push(element);
            else if (element == '[') stack.Push(element);
            else if (element == '(') stack.Push(element);
            else
            {
                if (!supportedElements.Contains(element)) 
                    continue;
                
                var left = stack.Pop();

                if ((left == '(' && element != ')') ||
                    (left == '[' && element != ']') ||
                    (left == '{' && element != '}'))
                    return false;
            }
        }
        
        return stack.Count == 0;
    }
}