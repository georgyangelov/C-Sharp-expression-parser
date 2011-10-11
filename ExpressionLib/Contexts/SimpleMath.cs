using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpressionLib.Contexts
{
    public class SimpleMath : IContext<double>
    {
        public int NumParams(string oprtr)
        {
            if (oprtr == "+" || oprtr == "-" || oprtr == "*" || oprtr == "/" || oprtr == "^")
                return 2;
            else if (oprtr == "(sqrt)")
                return 1;
            else
                return 0;
        }

        public double EvalOperator(string oprtr, List<double> values)
        {
            if (oprtr == "+")
                return values[0] + values[1];
            else if (oprtr == "-")
                return values[0] - values[1];
            else if (oprtr == "*")
                return values[0] * values[1];
            else if (oprtr == "/")
                return values[0] / values[1];
            else if (oprtr == "^")
                return Math.Pow(values[0], values[1]);
            else if (oprtr == "(sqrt)")
                return Math.Sqrt(values[0]);
            else
                throw new ParsingException("Unknown operator '" + oprtr + "' detected!");
        }

        public bool IsValue(string c)
        {
            double r;
            return IsValue(c, out r);
        }

        public bool IsValue(string c, out double r)
        {
            return double.TryParse(c.Replace('.', ','), out r);
        }

        public bool IsValidIdentificator(string c)
        {
            return Regex.IsMatch(c, @"^[a-zA-Z_]([a-zA-Z0-9_]+)?$");
        }

        public bool IsOperator(string c)
        {
            return
            (
                c == "^" || c == "*" || c == "/" || c == "+" || c == "-"
            );
        }

        public int PriorityOf(string c)
        {
            if (c == "(sqrt)")
                return 4;
            else if (c == "^")
                return 3;
            else if (c == "*" || c == "/")
                return 2;
            else if (c == "+" || c == "-")
                return 1;
            else return 0;
        }

        public Associativity AssociativityOf(string c)
        {
            if (c == "^")
                return Associativity.Right;
            else
                return Associativity.Left;
        }
    }
}
