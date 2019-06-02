using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticConsole
{
    public class PrefixHelper
    {
        public static double Compute(double a, double b, Operator? op)
        {
            switch (op)
            {
                case Operator.Add:
                    return a + b;

                case Operator.Substract:
                    return a - b;

                case Operator.Multiply:
                    return a * b;

                case Operator.Divide:
                    return a / b;

                case Operator.Pow:
                    return Math.Pow(a, b);

                default:
                    throw new ArgumentException("invalid operator");
            }
        }

        public static double Compute(double a, double b, char ch)
        {
            Operator op;

            switch (ch)
            {
                case '+':
                    op = Operator.Add;
                    break;

                case '-':
                    op = Operator.Substract;
                    break;

                case '*':
                    op = Operator.Multiply;
                    break;

                case '/':
                    op = Operator.Divide;
                    break;

                case '^':
                    op = Operator.Pow;
                    break;

                default:
                    throw new ArgumentException("invalid operator");
            }

            return Compute(a, b, op);
        }

        public static string InfixToPrefix(string infix)
        {
            // reverse infix
            string reverse = infix.Reverse();

            StringBuilder builder = new StringBuilder(reverse);

            // replace ( and )
            for (int i = 0; i < builder.Length; i++)
            {
                if (builder[i] == '(')
                {
                    builder[i] = ')';
                    i++;
                }
                else if (builder[i] == ')')
                {
                    builder[i] = '(';
                    i++;
                }
            }

            string prefix = InfixToPostfix(builder.ToString());

            // reverse postfix
            return prefix.Reverse();
        }

        private static int GetPriority(char op)
        {
            if (op == '-' || op == '+')
                return 1;

            if (op == '*' || op == '/')
                return 2;

            if (op == '^')
                return 3;

            return 0;
        }

        public static string InfixToPostfix(string infix)
        {
            string expression = '(' + infix + ')';
            Stack<char> stack = new Stack<char>();
            string output = "";

            for (int i = 0; i < expression.Length; i++)
            {
                // If the scanned character is an operand, add it to output
                if (char.IsLetterOrDigit(expression[i]))
                    output += expression[i];
                // If the scanned character is an ‘(‘, push it to the stack. 
                else if (expression[i] == '(')
                    stack.Push('(');

                // If the scanned character is an ‘)’, pop and output from the stack until an ‘(‘ is encountered. 
                else if (expression[i] == ')')
                {
                    while (stack.Peek() != '(')
                        output += stack.Pop();

                    // Remove '(' from the stack
                    stack.Pop();
                }
                // Operator found 
                else
                {
                    // Operator on top of the stack
                    if (!char.IsLetterOrDigit(stack.Peek()))
                    {
                        while (GetPriority(expression[i]) <= GetPriority(stack.Peek()))
                            output += stack.Pop();

                        // Push current Operator on stack 
                        stack.Push(expression[i]);
                    }
                }
            }

            return output;
        }

        public static double EvaluatePrefix(string prefix, params double[] variables)
        {
            Stack<double> stack = new Stack<double>();
            for (int i = prefix.Length - 1; i >= 0; i--)
            {
                char ch = prefix[i];

                if (ch == 'z')
                    stack.Push(-4);
                else if (char.IsDigit(ch))
                    stack.Push(double.Parse(ch.ToString()));
                else if (char.IsLetter(ch))
                {
                    double value = variables[ch - 'a'];
                    stack.Push(value);
                }
                else
                {
                    double a = stack.Pop();
                    double b = stack.Pop();
                    stack.Push(Compute(a, b, ch));
                }
            }

            return stack.Pop();
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Get the array slice between the two indexes.
        /// ... Inclusive for start index, exclusive for end index.
        /// </summary>
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }

        public static string Reverse(this string source)
        {
            var ary = source.ToCharArray();
            Array.Reverse(ary);
            return new string(ary);
        }
    }
}
