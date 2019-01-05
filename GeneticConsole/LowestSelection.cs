using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticConsole
{
    public class LowestSelection : SelectionBase
    {
        public LowestSelection() : base(2)
        {

        }

        protected override IList<IChromosome> PerformSelectChromosomes(int number, Generation generation)
        {
            var ordered = generation.Chromosomes.OrderBy(c => c.Fitness);
            return ordered.Take(number).ToList();
        }
    }
}
