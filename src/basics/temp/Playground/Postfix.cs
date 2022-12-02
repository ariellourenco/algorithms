using System.Text;

namespace Playground;

public static class Postfix
{
    private static readonly char[] Operators = "+-*/".ToCharArray();
    
    /// <summary>
    /// Exercise 1.3.10
    /// Converts an arithmetic expression from Infix to Postfix. It supports only 4 binary operators
    /// ‘+’, ‘*’, ‘-‘ and ‘/’. But can be extended for more operators by adding more switch cases.
    /// Time Complexity: O(N), where N is the size of the infix expression.
    /// </summary>
    /// <param name="expression">The infix expression to convert to.</param>
    public static string ToPostfix(string expression)
    {
        var builder = new StringBuilder();
        var stack = new Stack<char>();

        foreach (var element in expression)
        {
            if (char.IsLetterOrDigit(element)) builder.Append(element);
            else if (element == '(') stack.Push(element);
            else if (element == ')')
            {
                while (stack.Count > 0 && stack.Peek() != '(')
                {
                    builder.Append(stack.Pop());
                }

                stack.Pop();
            }
            else
            {
                // Ensure the character is a supported operator otherwise
                // continue scanning the expression.
                if (!Operators.Contains(element)) 
                    continue;

                while (stack.Count > 0 && Precedence(element) <= Precedence(stack.Peek()))
                    builder.Append(stack.Pop());

                stack.Push(element);
            }
        }

        while (stack.Count > 0)
        {
            builder.Append(stack.Pop());
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exercise 1.3.10
    /// Evaluates a postfix expressions.
    /// Time Complexity: O(N), where N is the size of the postfix expression.
    /// </summary>
    /// <param name="expression">The postfix expression to evaluate.</param>
    /// <returns>The expression result.</returns>
    public static int Evaluate(string expression)
    {
        var stack = new Stack<int>();

        foreach (var element in expression)
        {
            if (char.IsDigit(element))
            {
                stack.Push(int.Parse(element.ToString()));
            }
            else if (Operators.Contains(element))
            {
                // Pop, evaluate, and push result if token is ")".
                var value = stack.Pop();
                var result = element switch
                {
                    '+' => stack.Pop() + value,
                    '-' => stack.Pop() - value,
                    '*' => stack.Pop() * value,
                    '/' => stack.Pop() / value,
                    _ => throw new InvalidOperationException("Operation not supported.")
                };

                stack.Push(result);
            }
            else
            {
                if (element == ' ')
                    continue;

                throw new ArgumentException("Invalid expression");
            }
        }

        return stack.Pop();
    }

    private static int Precedence(char @operator) => @operator switch 
    {
        '+' => 1,
        '-' => 1,
        '*' => 2,
        '/' => 2,
        _ =>  -1
    };
}