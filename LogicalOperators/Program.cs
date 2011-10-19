using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressionLib;
using ExpressionLib.Contexts;

namespace LogicalOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            //string expr = "pr/2+(-3*2)^2 * sqrt(2+2) * max(sqrt(2), sqrt(4))";
            string expr = "(1 + 2.5) * 3";

            Dictionary<string, double> vars = new Dictionary<string, double>();
            vars.Add("pr", -1);
            Evaluator<double> eval = new Evaluator<double>(expr, new SimpleMath());

            try
            {
                Console.WriteLine("Sample math expression: " + expr);
                Console.WriteLine("In postfix format: " + eval.Expression);
                Console.WriteLine("Result: " + eval.Eval(vars));
            }
            catch (ParsingException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine("----------------------------------------");

            Evaluator<bool> evalLogic = new Evaluator<bool>(new SimpleLogic());

            string exprLogic = "(F | (!T > T)) = ((F & T) > (F > T))";
            try
            {
                Console.WriteLine("Sample logical expression: " + exprLogic);
                Console.WriteLine("Result: " + evalLogic.EvalInfix(exprLogic));
            }
            catch (ParsingException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine("----------------------------------------");

            //string exprLogic2 = "(p > (q > r)) > ((s > t) > (u > v))";
            //string exprLogic2 = "(p -> (not q -> r)) <-> ((r -> !q) -> (p -> r))";
            string exprLogic2 = "(((a|b)>(c=d))&(f^g))=h";

            try
            {
                Console.WriteLine("Truth table of: " + exprLogic2);

                Console.WriteLine();

                TruthTables.PrintTable(exprLogic2);
            }
            catch (ParsingException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
