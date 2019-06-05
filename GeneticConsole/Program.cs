using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Extensions.Mathematic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace GeneticConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "c-5*(a*(b+5)-1)/c+(a*b/c)";
            int variableNumber = 3;
            string prefix = PrefixHelper.InfixToPrefix(s);

            List<InputFunction> data = new List<InputFunction>();
            int sampleDataSize = 20;

            using (StreamWriter sw = new StreamWriter("InputData.txt"))
            {
                for (int i = 0; i < sampleDataSize; i++)
                {
                    sw.Write("(");
                    double[] values = new double[variableNumber];
                    for (int j = 0; j < variableNumber; j++)
                    {
                        values[j] = RandomizationProvider.Current.GetDouble(-10, 10);
                        sw.Write(values[j]);
                        if (j < variableNumber - 1)
                            sw.Write(',');
                    }
                    sw.Write(")(");

                    double result = PrefixHelper.EvaluatePrefix(prefix, values);
                    sw.WriteLine(result + ")");

                    InputFunction input = new InputFunction(result, values);

                    data.Add(input);
                }
            }

            #region old sample data

            // Sample data for 5A-7B

            //List<InputFunction> data = new List<InputFunction>()
            //{
            //    new InputFunction(-15, 4,5),
            //    new InputFunction(-2,1,1),
            //    new InputFunction(8,-4,-4),
            //    new InputFunction(-31,-2,3)
            //};

            //int funcLength = 2;


            // Sample data for 5A-7B-4C+2D

            //List<InputFunction> data = new List<InputFunction>()
            //{
            //    new InputFunction(11, 4,5,-5,3),
            //    new InputFunction(12,1,1,0,7),
            //    new InputFunction(0,-4,-4,3,2),
            //    new InputFunction(-9,-2,3,-7,-3)
            //};

            //int funcLength = 4;


            // Sample data for 5A-7B-4C+2D+0E+1F+9G

            //List<InputFunction> data = new List<InputFunction>()
            //{
            //    new InputFunction(152,4,5,-5,3,8,6,15),
            //    new InputFunction(221,1,1,0,7,-25,-7,24),
            //    new InputFunction(-269,-4,-4,3,2,-1,55,-36),
            //    new InputFunction(384,-2,3,-7,-3,0,-12,45)
            //};

            //int funcLength = 7;


            // Custom data input

            //Console.WriteLine("Enter function length:");
            //int funcLength = int.Parse(Console.ReadLine());

            //int[] parameters = new int[funcLength];

            //while (true) 
            //{
            //    for (int i = 0; i < funcLength; i++)
            //    {
            //        Console.WriteLine("Enter #{0} param:", i);
            //        parameters[i] = int.Parse(Console.ReadLine());
            //    }

            //    Console.WriteLine("Enter result:");
            //    int result = int.Parse(Console.ReadLine());

            //    data.Add(new InputFunction(result, parameters));

            //    Console.WriteLine("press enter to continue, or write 'done' to start the ga");
            //    string input = Console.ReadLine();
            //    if (input == "done")
            //        break;
            //}

            #endregion

            int maxLength = 6 + 5 * variableNumber;

            IChromosome chromosome = new ExpressionChromosome(variableNumber,  maxLength);
            IPopulation population = new Population(8500, 10000, chromosome);
            population.GenerationStrategy = new PerformanceGenerationStrategy();
            IFitness fitness = new ExpressionFitness(data.ToArray());
            ISelection selection = new EliteSelection();
            ICrossover crossover = new ExpressionCrossover(maxLength);
            IMutation mutation = new ExpressionMutation(variableNumber);
            //IMutation mutation = new TickMutation();

            GeneticAlgorithm ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.Termination = new ExpressionTermination(200);
            ga.MutationProbability = .5f;

            double latestFitness = double.MinValue;

            ga.GenerationRan += (sender, e) =>
            {
                Console.Title = ga.TimeEvolving.ToString();
                //Console.Title = ga.Population.CurrentGeneration.Chromosomes.Count.ToString();

                ExpressionChromosome bestChromosome = ga.BestChromosome as ExpressionChromosome;
                double bestFitness = bestChromosome.Fitness.Value;
                if (bestFitness != latestFitness)
                {
                    latestFitness = bestFitness;

                    Console.WriteLine("\n--------\n");
                    Console.WriteLine("Generation: {0}", ga.Population.GenerationsNumber);
                    //Console.WriteLine("Time: {0}", ga.TimeEvolving);
                    Console.WriteLine("Fitness: {0}", bestFitness.ToString("0.00"));
                    Console.WriteLine("Best: {0}", bestChromosome.ToString());
                }
            };

            ga.Start();

            Console.WriteLine("\nTime: {0}", ga.TimeEvolving);

            double sum = 0;
            double[] diffs = new double[sampleDataSize];

            for (int i = 0; i < sampleDataSize; i++)
            {
                double resultActual = PrefixHelper.EvaluatePrefix(prefix, data[i].Parameters);
                double resultOur = PrefixHelper.EvaluatePrefix(ga.BestChromosome.GetGenes(), data[i].Parameters);
                double diff = Math.Abs(resultActual - resultOur);
                Console.WriteLine($"Actual: {resultActual.ToString("0.00")} | Our: {resultOur.ToString("0.00")} | Diff: {diff.ToString("0.00")}");
                sum += diff;
                diffs[i] = diff;
            }

            double avgDiff = sum / sampleDataSize;
            Console.WriteLine($"Average diff: {avgDiff.ToString("0.00")} | Median: {GetMedian(diffs).ToString("0.00")}");

            Console.WriteLine("\n ---- New Samples ---- \n");

            for (int i = 0; i < sampleDataSize; i++)
            {
                double[] values = new double[variableNumber];
                for (int j = 0; j < variableNumber; j++)
                {
                    values[j] = RandomizationProvider.Current.GetDouble(-10, 10);
                }

                double result = PrefixHelper.EvaluatePrefix(prefix, values);
                double our = PrefixHelper.EvaluatePrefix(ga.BestChromosome.GetGenes(), values);
                double diff = Math.Abs(result - our);
                Console.WriteLine($"Actual: {result.ToString("0.00")} | Our: {our.ToString("0.00")} | Diff: {diff.ToString("0.00")}");
                sum += diff;
                diffs[i] = diff;
            }

            avgDiff = sum / sampleDataSize;
            Console.WriteLine($"Average diff: {avgDiff.ToString("0.00")} | Median: {GetMedian(diffs).ToString("0.00")}");

            Console.ReadKey();
        }

        public static double GetMedian(double[] array)
        {
            double[] tempArray = array;
            int count = tempArray.Length;

            Array.Sort(tempArray);

            double medianValue = 0;

            if (count % 2 == 0)
            {
                // count is even, need to get the middle two elements, add them together, then divide by 2
                double middleElement1 = tempArray[(count / 2) - 1];
                double middleElement2 = tempArray[(count / 2)];
                medianValue = (middleElement1 + middleElement2) / 2;
            }
            else
            {
                // count is odd, simply get the middle element.
                medianValue = tempArray[(count / 2)];
            }

            return medianValue;
        }
    }
}
