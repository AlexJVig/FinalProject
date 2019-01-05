using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticConsole
{
    public class FunctionChromosome : ChromosomeBase
    {
        private int minValue;
        private int maxValue;

        public FunctionChromosome(int minValue, int maxValue, int length): base(length)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            CreateGenes();
        }

        public override IChromosome CreateNew()
        {
            return new FunctionChromosome(minValue, maxValue, Length);
        }

        public override Gene GenerateGene(int geneIndex)
        {
            int value = RandomizationProvider.Current.GetInt(minValue, maxValue);
            return new Gene(value);
        }

        public int Calculate(params int[] parameters)
        {
            if (parameters.Length != Length)
                throw new ArgumentException("Invalid parameters count for function");

            int result = 0;

            for (int i = 0; i < Length; i++)
                result += (int)GetGene(i).Value * parameters[i];

            return result;
        }
        public string BuildFunction()
        {
            StringBuilder builder = new StringBuilder();
            char letter = 'A';

            for (int i = 0; i < Length; i++)
            {
                int coefficient = (int)GetGene(i).Value;

                if (coefficient >= 0 && i > 0)
                    builder.Append("+");

                builder.AppendFormat("{0}{1} ", coefficient, letter);

                letter++;
            }

            return builder.ToString();
        }
    }
}
