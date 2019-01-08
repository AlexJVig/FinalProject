using GeneticSharp.Domain;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticConsole
{
    public class ZeroFitnessTermination : TerminationBase
    {
        protected override bool PerformHasReached(IGeneticAlgorithm geneticAlgorithm)
        {
            return geneticAlgorithm.BestChromosome.Fitness == 0;
        }
    }
}
