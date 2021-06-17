using System;
using System.Collections.Generic;

namespace Algorithms.Basics
{
    public static class Expression
    {
        /// <summary>
        /// Exercise 1.3.10 - Takes an infix expression and prints the equivalent postfix expression.
        /// https://runestone.academy/runestone/books/published/pythonds/BasicDS/InfixPrefixandPostfixExpressions.html
        /// </summary>
        public static string Postfix(string expression)
        {
            return string.Empty;
        }
    }

    public static class Program
    {
        public static void Main()
        {
            string[] given = new string[] { "1+2)*3-4)*5-6)))", "1+2)*3-4)*(5-6)))" };

            foreach (var expression in given)
                Console.WriteLine(Expression.Postfix(expression));
        }
    }
}
