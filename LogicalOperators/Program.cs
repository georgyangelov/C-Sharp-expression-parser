using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressionLib;

namespace LogicalOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            Evaluator<double> eval = new Evaluator<double>(new ExpressionLib.Contexts.SimpleMath());

            string expr = "-1/2+(-3*2)^2";

            try
            {
                Console.WriteLine(expr);
                Console.WriteLine(eval.ToPostfix(expr));
                Console.WriteLine(eval.EvalInfix(expr));
            }
            catch (ParsingException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
