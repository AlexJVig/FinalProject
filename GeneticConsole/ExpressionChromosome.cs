using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Infrastructure.Framework.Commons;
using GeneticSharp.Infrastructure.Framework.Texts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticConsole
{
    public class ExpressionChromosome : IChromosome
    {
        private Gene[] m_genes;
        private int m_variableCount;

        public double? Fitness { get; set; }

        public int Length
        {
            get
            {
                if (m_genes != null)
                    return m_genes.Length;

                return 0;
            }
        }


        public ExpressionChromosome(int variableCount)
        {
            m_variableCount = variableCount;
            m_genes = CreateGenes();
        }

        private Gene[] CreateGenes()
        {
            List<Gene> genes = new List<Gene>();
            Gene gene = CreateGene();
            genes.Add(gene);

            if (IsGeneOperator(gene))
            {
                Gene[] left = CreateGenes();
                Gene[] right = CreateGenes();

                genes.AddRange(left);
                genes.AddRange(right);
            }

            return genes.ToArray();
        }

        private bool IsGeneOperator(Gene gene)
        {
            return (gene.Value as ExpressionGene).Type == GeneType.Operator;
        }

        private Gene CreateGene()
        {

            int type = RandomizationProvider.Current.GetInt(0, 3);
            GeneType geneType = (GeneType)type;
            Gene gene = new Gene(new ExpressionGene(geneType));

            switch (geneType)
            {
                case GeneType.Operator:
                    int op = RandomizationProvider.Current.GetInt(0, Enum.GetNames(typeof (Operator)).Length);
                    (gene.Value as ExpressionGene).Operator = (Operator)op;
                    break;

                case GeneType.Number:
                    float number = RandomizationProvider.Current.GetFloat(-10, 10);
                    (gene.Value as ExpressionGene).Number = number;

                    break;

                case GeneType.Variable:
                    int variable = RandomizationProvider.Current.GetInt(0, m_variableCount);
                    (gene.Value as ExpressionGene).Variable = variable;

                    break;

                default:
                    break;
            }

            return gene;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Length; i++)
            {
                builder.Append(m_genes[i].Value.ToString());
            }
            return builder.ToString();
        }

        public double Evaluate(params double[] variables)
        {
            Stack<double> stack = new Stack<double>();
            for (int i = Length - 1; i >= 0; i--)
            {
                ExpressionGene gene = m_genes[i].Value as ExpressionGene;

                switch (gene.Type)
                {
                    case GeneType.Number:
                        stack.Push(gene.Number);
                        break;

                    case GeneType.Variable:
                        stack.Push(variables[gene.Variable]);
                        break;

                    case GeneType.Operator:
                        double a = stack.Pop();
                        double b = stack.Pop();
                        stack.Push(Compute(a, b, gene.Operator));
                        break;

                    default:
                        break;
                }
            }

            return stack.Pop();
        }

        private double Compute(double a, double b, Operator? op)
        {
            switch (op)
            {
                case Operator.Add:
                    return a + b;

                case Operator.Substract:
                    return a - b;

                case Operator.Multiply:
                    return a * b;

                case Operator.Divide:
                    return a / b;

                case Operator.Pow:
                    return Math.Pow(a, b);

                default:
                    throw new ArgumentException("invalid operator");
            }
        }

        /// interface members

        /// <summary>
        /// Generates the gene for the specified index.
        /// </summary>
        /// <param name="geneIndex">Gene index.</param>
        /// <returns>The gene generated at the specified index.</returns>
        public Gene GenerateGene(int geneIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new chromosome using the same structure of this.
        /// </summary>
        /// <returns>The new chromosome.</returns>
        public IChromosome CreateNew()
        {
            return new ExpressionChromosome(m_variableCount);
        }

        /// <summary>
        /// Creates a clone.
        /// </summary>
        /// <returns>The chromosome clone.</returns>
        public virtual IChromosome Clone()
        {
            var clone = CreateNew();
            clone.ReplaceGenes(0, GetGenes());
            clone.Fitness = Fitness;

            return clone;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="other">The other chromosome.</param>
        public int CompareTo(IChromosome other)
        {
            if (other == null)
            {
                return -1;
            }

            var otherFitness = other.Fitness;

            if (Fitness == otherFitness)
            {
                return 0;
            }

            return Fitness > otherFitness ? 1 : -1;
        }

        /// <summary>
        /// Gets the gene in the specified index.
        /// </summary>
        /// <param name="index">The gene index.</param>
        /// <returns>
        /// The gene.
        /// </returns>
        public Gene GetGene(int index)
        {
            return m_genes[index];
        }

        /// <summary>
        /// Gets the genes.
        /// </summary>
        /// <returns>The genes.</returns>
        public Gene[] GetGenes()
        {
            return m_genes;
        }

        /// <summary>
        /// Replaces the gene in the specified index.
        /// </summary>
        /// <param name="index">The gene index to replace.</param>
        /// <param name="gene">The new gene.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index;There is no Gene on index {0} to be replaced..With(index)</exception>
        public void ReplaceGene(int index, Gene gene)
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "There is no Gene on index {0} to be replaced.".With(index));
            }

            m_genes[index] = gene;
            Fitness = null;
        }

        /// <summary>
        /// Replaces the genes starting in the specified index.
        /// </summary>
        /// <param name="startIndex">Start index.</param>
        /// <param name="genes">The genes.</param>
        /// <remarks>
        /// The genes to be replaced can't be greater than the available space between the start index and the end of the chromosome.
        /// </remarks>
        public void ReplaceGenes(int startIndex, Gene[] genes)
        {
            ExceptionHelper.ThrowIfNull("genes", genes);

            if (genes.Length > 0)
            {
                if (startIndex < 0 || startIndex >= Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex), "There is no Gene on index {0} to be replaced.".With(startIndex));
                }

                var genesToBeReplacedLength = genes.Length;

                var availableSpaceLength = Length - startIndex;

                if (genesToBeReplacedLength > availableSpaceLength)
                {
                    throw new ArgumentException(
                        nameof(Gene),
                        "The number of genes to be replaced is greater than available space, there is {0} genes between the index {1} and the end of chromosome, but there is {2} genes to be replaced."
                        .With(availableSpaceLength, startIndex, genesToBeReplacedLength));
                }

                Array.Copy(genes, 0, m_genes, startIndex, genes.Length);

                Fitness = null;
            }
        }

        /// <summary>
        /// Resizes the chromosome to the new length.
        /// </summary>
        /// <param name="newLength">The new length.</param>
        public void Resize(int newLength)
        {
            Array.Resize(ref m_genes, newLength);
        }
    }
}
