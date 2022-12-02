namespace Playground;

/// <summary>
/// Arithmetic expression evaluation.
/// Evaluates a fully parenthesized arithmetic expressions, which specify precisely which
/// operators apply to which operands, and produce the number represented by the expression as output.
/// See https://faculty.cs.niu.edu/~hutchins/csci241/eval.htm for more information
/// </summary>
public static class Calculator
{ 
    public static double Calculate(string expression)
    {
        var operands = new Stack<double>();
        var operators = new Stack<char>();

        foreach (var item in expression)
        {
            if (char.IsDigit(item)) operands.Push(double.Parse(item.ToString()));
            else if (item.Equals('+')) operators.Push(item);
            else if (item.Equals('-')) operators.Push(item);
            else if (item.Equals('*')) operators.Push(item);
            else if (item.Equals('/')) operators.Push(item);
            else if (item.Equals(')'))
            {
                // Pop, evaluate, and push result if token is ")".
                var value = operands.Pop();
                var result = operators.Pop() switch
                {
                    '+' => operands.Pop() + value,
                    '-' => operands.Pop() - value,
                    '*' => operands.Pop() * value,
                    '/' => operands.Pop() / value,
                    _ => throw new InvalidOperationException("Operation not supported.")
                };
            
                operands.Push(result);
            }
        }

        return operands.Pop();
    }
}