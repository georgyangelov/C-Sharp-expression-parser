using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpressionLib
{
    public class Evaluator<T>
    {
        private IContext<T> context;
        public  IContext<T> Context
        {
            get { return context; }
        }

        private string expression;
        public string Expression
        {
            get { return expression; }
            set { expression = ToPostfix(value, variables); }
        }

        private List<string> variables = new List<string>();
        public List<string> Variables
        {
            get { return variables; }
        }


        public Evaluator(IContext<T> context)
        {
            this.context = context;
        }
        public Evaluator(string expression, IContext<T> context)
        {
            this.context = context;
            Expression = expression;
        }

        public T Eval(Dictionary<string, T> variables = null)
        {
            if (Expression.Length == 0)
                throw new Exception("No expression set!");

            return EvalPostfix(Expression, variables);
        }

        public T EvalInfix(string infix, Dictionary<string, T> variables = null)
        {
            return EvalPostfix(ToPostfix(infix), variables);
        }

        public T EvalPostfix(string postfix, Dictionary<string, T> variables = null)
        {
            bool useVariables = (variables != null);
            Stack<T> nums = new Stack<T>();
            string[] expr = postfix.Split(' ');

            T val;
            List<T> tempVals = new List<T>();
            foreach (string o in expr)
            {
                if (o.Length == 0) continue;

                if (useVariables && IsValidIdentificator(o) && variables.ContainsKey(o))
                    nums.Push(variables[o]);
                else if (Context.IsValue(o, out val))
                    nums.Push(val);
                else
                {
                    tempVals.Clear();

                    // Operator
                    for (int i = 1; i <= Context.NumParams(o); i++)
                        tempVals.Add(nums.Pop());
                    // Fix the position of the elements
                    tempVals.Reverse();

                    nums.Push(Context.EvalOperator(o, tempVals));
                }
            }

            if ( nums.Count != 1 )
                throw new ParsingException("Something is wrong! We should've had one element left in the stack...");

            return nums.Pop();
        }

        public string ToPostfix(string infix, List<string> variables = null)
        {
            string[] expr = splitExpression(infix);
            string output = "";
            Stack<string> oprtr = new Stack<string>();
            int openedBrackets = 0;

            foreach (string o in expr)
            {
                if (Context.IsValue(o))
                    output += o + " ";
                else if (!Context.IsOperator(o) && IsValidIdentificator(o))
                {
                    if ( variables != null && !variables.Contains(o) ) 
                        variables.Add(o);
                    
                    output += o + " ";
                }
                else if (o == ",")
                {
                    // Pop stack to output until we find the opening bracket
                    while (oprtr.Count > 0 && (!oprtr.Peek().Contains('(')))
                        output += oprtr.Pop() + " ";

                    if (oprtr.Count == 0)
                        throw new ParsingException("Too many closing brackets!");
                }
                else if (o == ")")
                {
                    // Pop stack to output until we find the opening bracket
                    while (oprtr.Count > 0 && (!oprtr.Peek().Contains('(')))
                        output += oprtr.Pop() + " ";

                    // Discard the bracket
                    if (oprtr.Count == 0)
                        throw new ParsingException("Too many closing brackets!");

                    openedBrackets--;

                    if (oprtr.Peek() != "(" && oprtr.Peek().Contains('('))
                    {
                        output += "(" + oprtr.Pop().Replace('(', ')') + " ";
                    }
                    else
                    {
                        oprtr.Pop();
                    }
                }
                else
                {
                    if (!o.Contains('(') && oprtr.Count > 0 && (Context.PriorityOf(o) < Context.PriorityOf(oprtr.Peek()) || (Context.AssociativityOf(o) == Associativity.Left && Context.PriorityOf(o) == Context.PriorityOf(oprtr.Peek()))))
                        output += oprtr.Pop() + " ";

                    if (o.Contains('('))
                        openedBrackets++;

                    oprtr.Push(o);
                }
            }

            if (openedBrackets != 0)
                throw new ParsingException("Too many opening brackets!");

            // Pop entire stack to output
            while (oprtr.Count > 0)
                output += oprtr.Pop() + " ";

            return output;
        }

        private string[] splitExpression(string expr)
        {
            char[] input = expr.ToCharArray();
            List<string> output = new List<string>();

            string last = "";
            foreach (char c in input)
            {
                if (c == ' ')
                {
                    last = "space";
                    continue;
                }

                if (Context.IsValue(c.ToString()) || (last == "value" && c == '.') || ((last == "(" || last == ")" || last == "") && c == '-'))
                {
                    if (last != "value")
                        output.Add(c.ToString());
                    else
                        output[output.Count - 1] += c.ToString();

                    last = "value";
                }
                else if (Context.IsOperator(c.ToString()))
                {
                    output.Add(c.ToString());
                    last = "operator";
                }
                else if (IsValidIdentificator(c.ToString()))
                {
                    if (last != "identificator")
                        output.Add(c.ToString());
                    else
                        output[output.Count - 1] += c.ToString();

                    last = "identificator";
                }
                else if (c == '(')
                {
                    if (last == "identificator")
                        output[output.Count - 1] += c.ToString();
                    else
                        output.Add(c.ToString());

                    last = "(";
                }
                else if (c == ')')
                {
                    output.Add(c.ToString());

                    last = ")";
                }
                else if (c == ',')
                    last = ",";
            }

            return output.ToArray();
        }

        public bool IsValidIdentificator(string c)
        {
            return Regex.IsMatch(c, @"^[a-zA-Z_]([a-zA-Z0-9_]+)?$");
        }
    }

    public class ParsingException : Exception
    {
        public ParsingException(string msg) : base(msg)
        {
        }
    }
}
