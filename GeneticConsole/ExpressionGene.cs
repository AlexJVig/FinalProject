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
                    return GetOperatorSymbol().ToString();

                case GeneType.Number:
                    return Number.ToString();

                case GeneType.Variable:
                    return $"X{Variable}";

                default:
                    return "Invalid Type";
            }
        }

        public int CompareTo(ExpressionGene other)
        {
            if (other == null)
                return -1;

            if (other.Type != Type)
                return -1;

            switch (Type)
            {
                case GeneType.Operator:
                    if (other.Operator != Operator) return -1;
                    break;

                case GeneType.Number:
                    if (other.Number != Number) return -1;
                    break;

                case GeneType.Variable:
                    if (other.Variable != Variable) return -1;
                    break;
            }

            return 0;
        }

        public static bool operator ==(ExpressionGene first, ExpressionGene second)
        {
            if (Object.ReferenceEquals(first, second))
            {
                return true;
            }

            if (((object)first == null) || ((object)second == null))
            {
                return false;
            }

            return first.CompareTo(second) == 0;
        }

        public static bool operator !=(ExpressionGene first, ExpressionGene second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            var other = obj as ExpressionGene;

            if (other == null)
            {
                return false;
            }

            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Operator.GetHashCode() ^ Number.GetHashCode() ^ Variable;
        }

        private char GetOperatorSymbol()
        {
            switch (Operator)
            {
                case GeneticConsole.Operator.Add:
                    return '+';

                case GeneticConsole.Operator.Substract:
                    return '-';

                case GeneticConsole.Operator.Multiply:
                    return '*';

                case GeneticConsole.Operator.Divide:
                    return '/';

                case GeneticConsole.Operator.Pow:
                    return '^';

                default:
                    return '?';
            }
        }
    }
}
