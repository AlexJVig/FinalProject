using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticConsole
{
    public class InputFunction
    {
        public int[] Parameters { get; set; }
        public int Result { get; set; }
        public int Length { get { return Parameters.Length; } }

        public InputFunction(int result, params int[] parameters)
        {
            Parameters = parameters;
            Result = result;
        }
    }
}
