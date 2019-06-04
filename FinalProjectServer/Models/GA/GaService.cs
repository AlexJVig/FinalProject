using System;
using System.Collections.Generic;
using System.Text;
using GeneticConsole;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using System.Linq;
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

            //        IChromosome chromosome = new FunctionChromosome(-10, 10, funcLength);
            //            IPopulation population = new Population(10000, 20000, chromosome)
            //            {
            //                GenerationStrategy = new PerformanceGenerationStrategy()
            //            };
            //            IFitness fitness = new FunctionFitness(inputsList.ToArray());
            //        ISelection selection = new EliteSelection();
            //        ICrossover crossover = new OnePointCrossover();
            //        IMutation mutation = new UniformMutation(true);
            //            //IMutation mutation = new TickMutation();

            //            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
            //            {
            //                Termination = new FitnessThresholdTermination(0),
            //                MutationProbability = .5f
            //            };

            //            double latestFitness = double.MinValue;
            //            FunctionChromosome bestChromosome = null;

            //        ga.GenerationRan += (sender, e) =>
            //            {
            //                //Console.Title = ga.TimeEvolving.ToString();
            //                //Console.Title = ga.Population.CurrentGeneration.Chromosomes.Count.ToString();

            //                bestChromosome = ga.BestChromosome as FunctionChromosome;
            //        double bestFitness = bestChromosome.Fitness.Value;
            //                if (bestFitness != latestFitness)
            //                {
            //                    latestFitness = bestFitness;

            //                    //Console.WriteLine("\n--------\n");
            //                    //Console.WriteLine("Generation: {0}", ga.Population.GenerationsNumber);
            //                    //Console.WriteLine("Time: {0}", ga.TimeEvolving);
            //                    //Console.WriteLine("Fitness: {0}", bestFitness);
            //                    //Console.WriteLine("Best: {0}", bestChromosome.BuildFunction());
            //                }
            //};

            int maxLength = 5 + 3 * funcLength;

            IChromosome chromosome = new ExpressionChromosome(funcLength, maxLength);
            IPopulation population = new Population(10000, 20000, chromosome);
            population.GenerationStrategy = new PerformanceGenerationStrategy();
            IFitness fitness = new ExpressionFitness(inputsList.ToArray());
            ISelection selection = new EliteSelection();
            ICrossover crossover = new ExpressionCrossover(maxLength);
            IMutation mutation = new ExpressionMutation(funcLength);
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
                    Console.WriteLine("Fitness: {0}", bestFitness);
                    Console.WriteLine("Best: {0}", bestChromosome.ToString());
                }
            };

            ga.Start();

            ExpressionChromosome bestChromosome1 = ga.BestChromosome as ExpressionChromosome;
            ExpressionGene[] arrChromosome = bestChromosome1.GetExpressionGenes();
            // Gene[] arrChromosome = ga.BestChromosome.GetGenes();

            // GeneType.Number.GetType() == arrChromosome[0].GetType()

            //return GenerateCSharpFunction(bestChromosome.BuildFunction());

            var variables = arrChromosome.Where(x => x.Type == GeneType.Variable).Distinct().ToList();
            var resultingFunction = GaService.PrefixToInfix(arrChromosome);

            return GaService.GenerateCSharpFunction(variables, resultingFunction);
        }

        private static string GenerateCSharpFunction(List<ExpressionGene> variables, string mathRepresentation)
        {
            return $@"private double resultFunction({string.Join(", ", variables.Select(x => $"double {x}").ToList())})
            {{
                return {mathRepresentation};
            }}";
        }

        private static string GetFunc(string mathRepresentation)
        {
            //string prefix = "";
            //string str = "Public String Func(){@return " + PrefixToInfix(prefix) + ";@}";
            
            //str = str.Replace("@", "@" + System.Environment.NewLine);
            return "";
        }

        private static string PrefixToInfix(ExpressionGene[] pre_exp)
        {
            Stack<string> s = new Stack<string>();

            // length of expression 
            int length = pre_exp.Length;

            // reading from right to left 
            for (int i = length - 1; i >= 0; i--)
            {
                // check if symbol is operator 
                if (pre_exp[i].Type == GeneType.Operator)
                {
                    // pop two operands from stack 
                    string op1 = s.Peek(); s.Pop();
                    string op2 = s.Peek(); s.Pop();

                    // concat the operands and operator 
                    string temp = "(" + op1 + pre_exp[i] + op2 + ")";

                    // Push string temp back to stack 
                    s.Push(temp);
                }

                // if symbol is an operand 
                else
                {
                    // push the operand to the stack 
                    s.Push(pre_exp[i] + "");
                }
            }

            // Stack now contains the Infix expression 
            return s.Peek();
        }

        //private static string PrefixToInfix(string pre_exp)
        //{
        //    Stack<string> s = new Stack<string>();

        //    // length of expression 
        //    int length = pre_exp.Length;

        //    // reading from right to left 
        //    for (int i = length - 1; i >= 0; i--)
        //    {
        //        // check if symbol is operator 
        //        if (IsOperator(pre_exp[i]))
        //        {
        //            // pop two operands from stack 
        //            string op1 = s.Peek(); s.Pop();
        //            string op2 = s.Peek(); s.Pop();

        //            // concat the operands and operator 
        //            string temp = "(" + op1 + pre_exp[i] + op2 + ")";

        //            // Push string temp back to stack 
        //            s.Push(temp);
        //        }

        //        // if symbol is an operand 
        //        else
        //        {
        //            // push the operand to the stack 
        //            s.Push(pre_exp[i]+"");
        //        }
        //    }

        //    // Stack now contains the Infix expression 
        //    return s.Peek();
        //}

     
//        private static string GenerateCSharpFunction(string mathRepresentation)
//        {
//            if (mathRepresentation.Contains(' '))
//            {
//                var functions = mathRepresentation.Split(' ');
//                StringBuilder resultingFunction = new StringBuilder();
//                StringBuilder signature = new StringBuilder();

//                Array.Resize(ref functions, functions.Length - 1); // Remove last element.

//                foreach (var function in functions)
//                {
//                    signature.Append($"int {function.Substring(function.Length - 1)}, ");
//                    resultingFunction.Append(GenerateCSharpFunction(function));
//                }

//                resultingFunction = resultingFunction[0] == '+' ? resultingFunction.Remove(0, 2) : resultingFunction;

//                return $@"private int resultFunction({signature.Remove(signature.Length - 2, 2).ToString()})
//                          {{
//return {resultingFunction.ToString()};
        //                  }}";
        //    }

        //    StringBuilder remainingString = new StringBuilder(mathRepresentation);
        //    StringBuilder cSharpFunction = new StringBuilder();

        //    // Check if contains negative coefficient.
        //    if (mathRepresentation.Contains('-'))
        //    {
        //        cSharpFunction.Append("- ");
        //        remainingString.Remove(0, 1);
        //    }
        //    else if (mathRepresentation.Contains('+'))
        //    {
        //        cSharpFunction.Append("+ ");
        //        remainingString.Remove(0, 1);
        //    }
        //    else
        //    {
        //        cSharpFunction.Append("+ ");
        //    }

        //    // Check if polynomial.
        //    if (mathRepresentation.Contains('^'))
        //    {
        //        var polynomial = remainingString.ToString().Split('^');
        //    }

        //    // Get coefficient number.
        //    var variable = remainingString.ToString().Substring(remainingString.Length - 1, 1);
        //    var coefficient = remainingString.Remove(remainingString.Length - 1, 1).ToString();

        //    cSharpFunction.Append(coefficient);
        //    cSharpFunction.Append($" * {variable}");

        //    return cSharpFunction.ToString();
        //}
        

            private static int GenerateLinear(string mathRepresentation)
        {
            return - foo(mathRepresentation);
        }

        private static int foo(string mathRepresentation)
        {
            return 1;
        }
    }
}
