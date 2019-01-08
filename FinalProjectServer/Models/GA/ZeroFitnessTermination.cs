using GeneticSharp.Domain;
using GeneticSharp.Domain.Terminations;

namespace FinalProjectServer.Models.GA
{
    public class ZeroFitnessTermination : TerminationBase
    {
        protected override bool PerformHasReached(IGeneticAlgorithm geneticAlgorithm)
        {
            return geneticAlgorithm.BestChromosome.Fitness == 0;
        }
    }
}