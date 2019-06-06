using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectServer.Models.GA
{
    public class InputFunction : GeneticConsole.InputFunction
    {
        public InputFunction(double result, params double[] parameters) : base(result, parameters)
        {
        }

        public InputFunction(IoPair pair, int outputIndex = 0)
        {
            Parameters = GetInputArray(pair);
            Result = pair.Output[outputIndex];
        }

        private double[] GetInputArray(IoPair pair)
        {
            var resultList = new List<double>();

            foreach (var number in pair.Input)
                resultList.Add(number);

            return resultList.ToArray();
        }
    }
}
