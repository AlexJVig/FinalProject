using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticConsole
{
    class NoMutation : MutationBase
    {
        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
           
        }
    }
}
