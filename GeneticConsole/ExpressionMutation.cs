using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticConsole
{
    class ExpressionMutation : MutationBase
    {
        int variableCount;

        public ExpressionMutation(int variableCount)
        {
            this.variableCount = variableCount;
        }

        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            if (RandomizationProvider.Current.GetDouble() <= probability)
            {
                int index = RandomizationProvider.Current.GetInt(0, chromosome.Length);
                ExpressionGene gene = chromosome.GetGene(index).Value as ExpressionGene;

                if (gene.Type == GeneType.Operator)
                {
                    Operator op = (Operator)RandomizationProvider.Current.GetInt(0, Enum.GetNames(typeof(Operator)).Length);
                    chromosome.ReplaceGene(index, new Gene(new ExpressionGene(GeneType.Operator, op)));
                }
                else
                {
                    // Number or Variable
                    GeneType type = (GeneType)RandomizationProvider.Current.GetInt(1, 3);
                    if (type == GeneType.Number)
                    {
                        double num = RandomizationProvider.Current.GetDouble(-10, 10); // Get from outside
                        chromosome.ReplaceGene(index, new Gene(new ExpressionGene(type, null, num)));
                    }
                    else if (type == GeneType.Variable)
                    {
                        int variable = RandomizationProvider.Current.GetInt(0, variableCount);
                        chromosome.ReplaceGene(index, new Gene(new ExpressionGene(type, null, 0, variable)));
                    }
                }
            }
        }
    }
}
