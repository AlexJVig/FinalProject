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
    }
}
