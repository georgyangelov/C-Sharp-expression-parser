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
            if (oprtr == "!" || oprtr == "not")
                return 1;
            else
                return 2;
        }

        public bool EvalOperator(string c, List<bool> values)
        {
            if (c == "!" || c == "not")
                return !values[0];
            else if (c == "&" || c == "&&")
                return (values[0] && values[1]);
            else if (c == "|" || c == "||")
                return (values[0] || values[1]);
            else if (c == "^" || c == "xor")
                return ( (values[0] && !values[1]) || (values[1] && !values[0]));
            else if (c == ">" || c == "->" || c == "=>" || c == "implies")
                return (!values[0] || values[1]);
            else if (c == "<->" || c == "<=>" || c == "=")
                return (values[0] == values[1]);
            else
                throw new ParsingException("Unknown operator '" + c + "' detected!");
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

        public bool IsOperator(string c)
        {
            return
            (
                c == "!" || c == "not" || c == "&" || c == "&&" || c == "|" || c == "||" || c == ">" || c == "<->" || c == "<=>" || c == "=" || c == "xor" || c == "^" || c == "->" || c == "=>" || c == "implies"
            );
        }

        public int PriorityOf(string c)
        {
            if (c == "!" || c == "not")
                return 5;
            else if (c == "&" || c == "&&")
                return 4;
            else if (c == "|" || c == "||")
                return 3;
            else if (c == "^" || c == "xor")
                return 3;
            else if (c == ">" || c == "->" || c == "=>" || c == "implies")
                return 2;
            else if (c == "<->" || c == "<=>" || c == "=")
                return 1;
            else return 0;
        }

        public Associativity AssociativityOf(string c)
        {
            if (c == "!" || c == "not")
                return Associativity.Right;
            else
                return Associativity.Left;
        }

        public static string ToString(bool value)
        {
            return (value ? "T" : "F");
        }
    }
}
