using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticConsole
{
    public enum GeneType { Operator, Number, Variable };
    public enum Operator { Add, Substract, Multiply, Divide, Pow };

    public class ExpressionGene
    {
        public GeneType Type { get; set; }
        public Operator? Operator { get; set; }
        public double Number { get; set; }
        public int Variable { get; set; }

        public ExpressionGene()
        {

        }

        public ExpressionGene(GeneType type, Operator? op = null, double number = 0, int variable = 0)
        {
            Type = type;
            Operator = op;
            Number = number;
            Variable = variable;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case GeneType.Operator:
                    return $"[Operator: {Enum.GetName(typeof(Operator), Operator)}]";

                case GeneType.Number:
                    return $"[Number: {Number}]";

                case GeneType.Variable:
                    return $"[Variable: {Variable}]";

                default:
                    return "Invalid Type";
            }
        }
    }
}
