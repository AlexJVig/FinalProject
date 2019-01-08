using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalProjectServer.Models.GA
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

        public InputFunction(IoPair pair)
        {
            Parameters = GetInputArray(pair);
            Result = (int)pair.Output.FirstOrDefault();
        }

        private int[] GetInputArray(IoPair pair)
        {
            var resultList = new List<int>();

            foreach (var number in pair.Input)
                resultList.Add((int)number);

            return resultList.ToArray();
        }
    }
}