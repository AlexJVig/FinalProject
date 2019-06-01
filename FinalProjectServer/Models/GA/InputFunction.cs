using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectServer.Models.GA
{
    public class InputFunction : GeneticConsole.InputFunction
    {
        public InputFunction(int result, params int[] parameters) : base(result, parameters)
        {
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
