using System;
using System.Collections.Generic;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;

namespace FinalProjectServer.Models.GA
{
    public class GaService
    {
        public static string StartGa(IoData data)
        {
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

            var inputsList = new List<InputFunction>();

            foreach (var entry in data.Data)
            {
                inputsList.Add(new InputFunction(entry));
            }

            int funcLength = data.Data[0].Input.Count;


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

        IChromosome chromosome = new FunctionChromosome(-10, 10, funcLength);
            IPopulation population = new Population(10000, 20000, chromosome)
            {
                GenerationStrategy = new PerformanceGenerationStrategy()
            };
            IFitness fitness = new FunctionFitness(inputsList.ToArray());
        ISelection selection = new EliteSelection();
        ICrossover crossover = new OnePointCrossover();
        IMutation mutation = new UniformMutation(true);
            //IMutation mutation = new TickMutation();

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
            {
                Termination = new FitnessThresholdTermination(0),
                MutationProbability = .5f
            };

            double latestFitness = double.MinValue;
            FunctionChromosome bestChromosome = null;

        ga.GenerationRan += (sender, e) =>
            {
                //Console.Title = ga.TimeEvolving.ToString();
                //Console.Title = ga.Population.CurrentGeneration.Chromosomes.Count.ToString();

                bestChromosome = ga.BestChromosome as FunctionChromosome;
        double bestFitness = bestChromosome.Fitness.Value;
                if (bestFitness != latestFitness)
                {
                    latestFitness = bestFitness;

                    //Console.WriteLine("\n--------\n");
                    //Console.WriteLine("Generation: {0}", ga.Population.GenerationsNumber);
                    //Console.WriteLine("Time: {0}", ga.TimeEvolving);
                    //Console.WriteLine("Fitness: {0}", bestFitness);
                    //Console.WriteLine("Best: {0}", bestChromosome.BuildFunction());
                }
};

ga.Start();

            return GenerateCSharpFunction(bestChromosome.BuildFunction());
        }

        private static string GenerateCSharpFunction(string mathRepresentation)
        {
            var functions = mathRepresentation.Split(' ');

            Array.Resize(ref functions, functions.Length - 1); // Remove last element.

            return "";
        }
    }
}
