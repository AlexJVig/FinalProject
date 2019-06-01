using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticConsole
{
    public class InputFunction
    {
        public float[] Parameters { get; set; }
        public float Result { get; set; }
        public int Length { get { return Parameters.Length; } }

        protected InputFunction()
        {
        }

        public InputFunction(float result, params float[] parameters)
        {
            Parameters = parameters;
            Result = result;
        }
    }
}