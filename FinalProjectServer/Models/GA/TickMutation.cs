using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;

namespace FinalProjectServer.Models.GA
{
    public class TickMutation : MutationBase
    {
        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            for (int i = 0; i < chromosome.Length; i++)
            {
                double rnd = RandomizationProvider.Current.GetDouble();

                if (rnd <= probability)
                {
                    int value = (int)chromosome.GetGene(i).Value;

                    if (rnd <= probability / 2)
                        value++;
                    else
                        value--;

                    chromosome.ReplaceGene(i, new Gene(value));
                }
                else if (rnd >= .8f)
                    chromosome.ReplaceGene(i, chromosome.GenerateGene(i));

            }
        }
    }
}