using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticConsole
{
    public class InputFunction
    {
        public double[] Parameters { get; set; }
        public double Result { get; set; }
        public int Length { get { return Parameters.Length; } }

        protected InputFunction()
        {
        }

        public InputFunction(double result, params double[] parameters)
        {
            Parameters = parameters;
            Result = result;
        }
    }
}