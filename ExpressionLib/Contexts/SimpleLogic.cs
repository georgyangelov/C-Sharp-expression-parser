using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpressionLib.Contexts
{
    public class SimpleLogic : IContext<bool>
    {
        public int NumParams(string oprtr)
        {
            if (oprtr == "!")
                return 1;
            else
                return 2;
        }

        public bool EvalOperator(string oprtr, List<bool> values)
        {
            if (oprtr == "!")
                return !values[0];
            else if (oprtr == "&")
                return (values[0] && values[1]);
            else if (oprtr == "|")
                return (values[0] || values[1]);
            else if (oprtr == ">")
                return (!values[0] || values[1]);
            else if (oprtr == "=")
                return (values[0] == values[1]);
            else
                throw new ParsingException("Unknown operator '" + oprtr + "' detected!");
        }

        public bool IsValue(string c)
        {
            bool r;
            return IsValue(c, out r);
        }

        public bool IsValue(string c, out bool r)
        {
            if (c == "T" || c == "1")
            {
                r = true;

                return true;
            }
            else if (c == "F" || c == "0")
            {
                r = false;

                return true;
            }
            else
            {
                r = false;
                return false;
            }
        }

        public bool IsValidIdentificator(string c)
        {
            return Regex.IsMatch(c, @"^[a-zA-Z_]([a-zA-Z0-9_]+)?$");
        }

        public bool IsOperator(string c)
        {
            return
            (
                c == "!" || c == "&" || c == "|" || c == ">" || c == "="
            );
        }

        public int PriorityOf(string c)
        {
            if (c == "!")
                return 5;
            else if (c == "&")
                return 4;
            else if (c == "|")
                return 3;
            else if (c == ">")
                return 2;
            else if (c == "=")
                return 1;
            else return 0;
        }

        public Associativity AssociativityOf(string c)
        {
            return Associativity.Left;
        }
    }
}
