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

            string expr = "-1/2+(-3*2)^2 * sqrt(2+2)";

            try
            {
                Console.WriteLine("Sample math expression: " + expr);
                Console.WriteLine("In postfix format: " + eval.ToPostfix(expr));
                Console.WriteLine("Result: " + eval.EvalInfix(expr));
            }
            catch (ParsingException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine("----------------------------------------");

            Evaluator<bool> evalLogic = new Evaluator<bool>(new ExpressionLib.Contexts.SimpleLogic());

            string exprLogic = "(F | (!T > T)) > ((F & T) > (F > T))";

            try
            {
                Console.WriteLine("Sample logical expression: " + exprLogic);
                Console.WriteLine("In postfix format: " + evalLogic.ToPostfix(exprLogic));
                Console.WriteLine("Result: " + evalLogic.EvalInfix(exprLogic));
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
