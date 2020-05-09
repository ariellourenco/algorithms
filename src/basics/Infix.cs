using System;
using System.Collections.Generic;

namespace Algorithms.Basics
{
    public static class Expression
    {
        /// <summary>
        /// Exercise 1.3.9 - Takes an expression without left parentheses and prints the equivalent infix
        /// expression with the parentheses inserted.
        /// </summary>
        public static string Infix(string expression)
        {
            var expressions = new Stack<string>();
            var operands = new Stack<char>();
            var operators = new Stack<char>();

            for(var i = 0; i < expression.Length; i++)
            {
                if      (expression[i].Equals('+')) operators.Push(expression[i]);
                else if (expression[i].Equals('-')) operators.Push(expression[i]);
                else if (expression[i].Equals('*')) operators.Push(expression[i]);
                else if (expression[i].Equals('/')) operators.Push(expression[i]);
                else if (char.IsDigit(expression[i])) operands.Push(expression[i]);
                else
                {
                    if (expression[i].Equals(')'))
                    {
                        if(operands.Count >= 2)
                        {
                            var right = operands.Pop();
                            var left = operands.Pop();
                            var token = operators.Pop();

                            expressions.Push($"({left}{token}{right})");
                        }
                        else if (expressions.Count >= 2)
                        {
                            var right = expressions.Pop();
                            var left = expressions.Pop();
                            var token = operators.Pop();

                            expressions.Push($"({left}{token}{right})");
                        }
                        else
                        {
                            throw new FormatException();
                        }
                    }
                }
            }

            return expressions.Pop();
        }
    }

    public static class Program
    {
        public static void Main()
        {
            string[] given = new string[] { "1+2)*3-4)*5-6)))", "1+2)*3-4)*(5-6)))" };

            foreach (var expression in given)
                Console.WriteLine(Expression.Infix(expression));
        }
    }
}
