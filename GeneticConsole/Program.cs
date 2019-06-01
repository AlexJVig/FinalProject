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
using System.Threading;

namespace GeneticConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Sample data for 5A-7B

            List<InputFunction> data = new List<InputFunction>()
            {
                new InputFunction(-15, 4,5),
                new InputFunction(-2,1,1),
                new InputFunction(8,-4,-4),
                new InputFunction(-31,-2,3)
            };

            int funcLength = 2;


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

            IChromosome chromosome = new ExpressionChromosome(funcLength);
            IPopulation population = new Population(10000, 20000, chromosome);
            population.GenerationStrategy = new PerformanceGenerationStrategy();
            IFitness fitness = new ExpressionFitness(data.ToArray());
            ISelection selection = new EliteSelection();
            ICrossover crossover = new ExpressionCrossover();
            IMutation mutation = new ExpressionMutation(funcLength);
            //IMutation mutation = new TickMutation();

            GeneticAlgorithm ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.Termination = new FitnessThresholdTermination(0);
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
                    Console.WriteLine("Fitness: {0}", bestFitness);
                    Console.WriteLine("Best: {0}", bestChromosome.ToString());
                }
            };

            ga.Start();

            Console.WriteLine("\nTime: {0}", ga.TimeEvolving);

            Console.ReadKey();
        }
    }
}
