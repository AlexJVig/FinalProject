using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GeneticConsole
{
    public class ExpressionCrossover : CrossoverBase
    {
        public ExpressionCrossover() : base(2,2,1)
        {
        }

        protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
        {
            ExpressionChromosome parent1 = parents[0] as ExpressionChromosome;
            ExpressionChromosome parent2 = parents[1] as ExpressionChromosome;

            int index1 = (int)Math.Floor(RandomizationProvider.Current.GetDouble() * parent1.Length);
            int index2 = (int)Math.Floor(RandomizationProvider.Current.GetDouble() * parent2.Length);

            Gene[] subtree1 = GetSubtree(parent1.GetGenes(), index1);
            Gene[] subtree2 = GetSubtree(parent2.GetGenes(), index2);

            // Copy subtree2 to parent1 at index1.
            ExpressionChromosome child1 = parent1.CreateNew() as ExpressionChromosome;
            Gene[] result = GetNewGenes(parent1.GetGenes(), index1, subtree1, subtree2);
            child1.SetGenes(result);

            // Copy subtree1 to parent2 at index2.
            ExpressionChromosome child2 = parent2.CreateNew() as ExpressionChromosome;
            result = GetNewGenes(parent2.GetGenes(), index2, subtree2, subtree1);
            child2.SetGenes(result);

            return new List<IChromosome>() { child1, child2 };
        }

        private Gene[] GetNewGenes(Gene[] source, int indexToSlice, Gene[] search, Gene[] replace)
        {
            Gene[] a = source.Slice(0, indexToSlice); // first part
            Gene[] b = source.Slice(indexToSlice, source.Length);
            int index = FindSequence(b, search);
            List<Gene> list = b.ToList();
            list.RemoveRange(index, search.Length);
            list.InsertRange(index, replace);
            Gene[] result = new Gene[a.Length + list.Count];
            a.CopyTo(result, 0);
            list.CopyTo(result, a.Length);
            return result;
        }

        //private Gene[] GetSubtree(Gene[] genes, int index)
        //{
        //    Gene gene = genes[index];

        //    Stack<Gene> operators = new Stack<Gene>();
        //    Stack<Gene> values = new Stack<Gene>();

        //    int valCount = 0;
        //    int i = index + 1;

        //    if ((gene.Value as ExpressionGene).Type == GeneType.Operator)
        //        operators.Push(gene);
        //    else
        //        values.Push(gene);

        //    while (operators.Count > 0 && i < genes.Length)
        //    {
        //        gene = genes[i];

        //        if ((gene.Value as ExpressionGene).Type != GeneType.Operator && valCount > 0)
        //        {
        //            double value = PrefixHelper.Compute(GetGeneValue(values.Pop()), GetGeneValue(gene), (operators.Pop().Value as ExpressionGene).Operator);
        //            values.Push(new Gene(new ExpressionGene(GeneType.Number,null ,value)));

        //            while (values.Count > 2 && operators.Count > 0)
        //            {
        //                value = PrefixHelper.Compute(GetGeneValue(values.Pop()), GetGeneValue(values.Pop()), (operators.Pop().Value as ExpressionGene).Operator);
        //                values.Push(new Gene(new ExpressionGene(GeneType.Number, null, value)));
        //            }

        //            if (operators.Count == 0 && values.Count == 1)
        //            {
        //                i++;
        //                break;
        //            }

        //            if (values.Count == 0)
        //                valCount = 0;
        //        }
        //        else if ((gene.Value as ExpressionGene).Type == GeneType.Operator)
        //        {
        //            operators.Push(gene);
        //            valCount = 0;
        //        }
        //        else
        //        {
        //            double value = GetGeneValue(gene);
        //            values.Push(new Gene(new ExpressionGene(GeneType.Number, null, value)));
        //            valCount++;
        //        }

        //        i++;
        //    }

        //    if (Math.Abs(index - i) % 2 == 0)
        //    {
        //        i--;
        //    }

        //    return genes.Slice(index, i);
        //}

        private Gene[] GetSubtree(Gene[] genes, int index)
        {
            int ops = 0;
            int nums = 0;
            int i = index;

            for (; i < genes.Length; i++)
            {
                ExpressionGene eg = genes[i].Value as ExpressionGene;
                if (eg.Type == GeneType.Operator)
                    ops++;
                else
                    nums++;

                if (nums == ops + 1)
                    break;
            }

            return genes.Slice(index, i+1);
        }

        private double GetGeneValue(Gene gene)
        {
            ExpressionGene eg = gene.Value as ExpressionGene;

            switch (eg.Type)
            {
                case GeneType.Number:
                    return eg.Number;

                case GeneType.Variable:
                    return 1;

                case GeneType.Operator:
                default:
                    throw new ArgumentException();
            }
        }

        private int FindSequence(Gene[] source, Gene[] search)
        {
            for (int i = 0; i <= source.Length - search.Length; i++)
            {
                bool error = false;

                for (int j = 0; j < search.Length; j++)
                {
                    if (source[j].Value != search[j].Value)
                    {
                        error = true;
                        break;
                    }
                }

                if (!error)
                {
                    return i;
                }

            }

            throw new Exception("Not Found");
        }

        private Gene[] ReplaceAt(Gene[] source, int index, Gene[] replace)
        {
            Gene[] result = new Gene[Math.Max(source.Length, index + replace.Length)];
            for (int i = 0; i < source.Length; i++)
            {
                result[i] = source[i];
            }

            for (int i = 0; i < replace.Length; i++)
            {
                result[index + i] = replace[i];
            }

            return result;
        }
    }
}
