using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticConsole
{
    public class ExpressionFitness : IFitness
    {
        private readonly InputFunction[] data;

        public ExpressionFitness(params InputFunction[] data)
        {
            this.data = data;
        }

        public double Evaluate(IChromosome chromosome)
        {
            ExpressionChromosome ch = chromosome as ExpressionChromosome;

            if (ch == null)
                throw new ArgumentException("Chromosome type is not ExpressionChromosome");

            double fitness = 0;

            foreach (InputFunction func in data)
            {
                double result = ch.Evaluate(func.Parameters);
                fitness += Math.Abs(result - func.Result);
            }

            return fitness * -1;
        }
    }
}
