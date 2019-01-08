using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;

namespace FinalProjectServer.Models.GA
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