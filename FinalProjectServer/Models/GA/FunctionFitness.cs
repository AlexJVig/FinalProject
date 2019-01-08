using System;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;

namespace FinalProjectServer.Models.GA
{
    public class FunctionFitness : IFitness
    {
        private readonly InputFunction[] data;

        public FunctionFitness(params InputFunction[] data)
        {
            this.data = data;
        }

        public double Evaluate(IChromosome chromosome)
        {
            FunctionChromosome ch = chromosome as FunctionChromosome;

            if (ch == null)
                throw new ArgumentException("Chromosome type is not FunctionChromosome");

            double fitness = 0;

            foreach (InputFunction func in data)
            {
                if (func.Length != chromosome.Length)
                    throw new Exception("Chromosome length is not equal to input function length");

                fitness += Math.Abs(ch.Calculate(func.Parameters) - func.Result);
            }

            return (fitness) * -1;
        }
    }
}