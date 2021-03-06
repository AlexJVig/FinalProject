﻿using System;
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
using System.Threading.Tasks;
using System.Diagnostics;
using GeneticSharp.Domain.Randomizations;
using Newtonsoft.Json;

namespace FinalProjectServer.Models.GA
{
    public class GaService
    {
        public static string StartGa(FreeTextData data)
        {
            var parsedObject = new IoData(data.Points);
            var inputsList = new List<InputFunction>[parsedObject.Data[0].Output.Count];

            for (int i = 0; i < inputsList.Length; i++)
            {
                inputsList[i] = new List<InputFunction>();
            }

            foreach (var entry in parsedObject.Data)
            {
                for (int i = 0; i < inputsList.Length; i++)
                {
                    inputsList[i].Add(new InputFunction(entry, i));
                }
            }

            Task<ExpressionChromosome>[] tasks = new Task<ExpressionChromosome>[inputsList.Length];

            for (int i = 0; i < inputsList.Length; i++)
            {
                var inputs = inputsList[i].ToArray();
                tasks[i] = Task.Run(() => RunGA(inputs));
            }

            try
            {
                Task.WaitAll(tasks);
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }

            string result = GenerateCSharpFunction(tasks.Select(x=>x.Result).ToArray(), parsedObject.Data[0].Input.Count, data.FunctionName, data.VariableNames);
            return result;
        }

        private static ExpressionChromosome RunGA(InputFunction[] inputs)
        {
            Debug.WriteLine(string.Join(',', inputs.Select(x=>x.Result)));
            int funcLength = inputs[0].Parameters.Length;
            int maxLength = 5 + 3 * funcLength;

            IChromosome chromosome = new ExpressionChromosome(funcLength, maxLength);
            IPopulation population = new Population(8500, 10000, chromosome);
            population.GenerationStrategy = new PerformanceGenerationStrategy();
            IFitness fitness = new ExpressionFitness(inputs.ToArray());
            ISelection selection = new EliteSelection();
            ICrossover crossover = new ExpressionCrossover(maxLength);
            IMutation mutation = new ExpressionMutation(funcLength);

            GeneticAlgorithm ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.Termination = new ExpressionTermination(200);
            ga.MutationProbability = .5f;

            ga.Start();

           return ga.BestChromosome as ExpressionChromosome;
        }

        private static string GenerateCSharpFunction(ExpressionChromosome[] results, int variableCount, string functionName, string variables)
        {
            string[] mathReps = results.Select(x => PrefixToInfix(x.GetExpressionGenes())).ToArray();
            string[] variablesArray = Enumerable.Range(0, variableCount).Select(i => $"double X{i}").ToArray();
            //var variableEnumerable = variables.Split(',').ToList().Select(s => $"double {s}");
            string variableString = string.Join(", ", variablesArray);
            string[] variableArray = variables.Split(',');

            if (results.Length == 1)
            {
                StringBuilder result = new StringBuilder($@"{'\n'}
private dobule {functionName}({variableString})
{{
    return {mathReps[0]};
}}");

                for (int i = 0; i < variableArray.Length; i++)
                    result.Replace($"X{i}", variableArray[i]);

                return result.ToString();
            }
            else
            {
                string[] tempVars = mathReps.Select((x, i) => $"double result{i} = {x};").ToArray();
                string[] varNames = Enumerable.Range(0, results.Length).Select(x => $"result{x}").ToArray();
                StringBuilder result = new StringBuilder($@"{'\n'}
private dobule[] {functionName}({variableString})
{{
    {string.Join("\n    ", tempVars)}
    double[] result = new double[] {{ {string.Join(", ", varNames)} }};
    return result;
}}");

                for (int i = 0; i < variableArray.Length; i++)
                    result.Replace($"X{i}", variableArray[i]);

                return result.ToString();
            }
        }

        private static string GetFunc(string mathRepresentation)
        {
            //string prefix = "";
            //string str = "Public String Func(){@return " + PrefixToInfix(prefix) + ";@}";
            
            //str = str.Replace("@", "@" + System.Environment.NewLine);
            return "";
        }

        public static string GenerateSampleData(string equation)
        {
            string prefix = PrefixHelper.InfixToPrefix(equation);

            List<Models.GA.InputFunction> data = new List<Models.GA.InputFunction>();

            int sampleDataSize = 20;
            int variableNumber = equation.Where(x => char.IsLetter(x)).Distinct().Count();

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < sampleDataSize; i++)
            {
                result.Append("(");
                double[] values = new double[variableNumber];
                for (int j = 0; j < variableNumber; j++)
                {
                    values[j] = RandomizationProvider.Current.GetDouble(-10, 10);
                    result.Append(values[j]);
                    if (j < variableNumber - 1)
                        result.Append(',');
                }
                result.Append(")(");

                double result1 = PrefixHelper.EvaluatePrefix(prefix, values);

                result.AppendLine($"{result1})");

                InputFunction input = new InputFunction(result1, values);

                data.Add(input);
            }

            return result.ToString();
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
            string result = s.Peek();

            result = result.Replace("+-", "-");
            result = result.Replace("--", "+");
            return result;
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
