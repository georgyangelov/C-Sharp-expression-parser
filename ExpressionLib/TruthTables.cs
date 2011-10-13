using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressionLib.Contexts;

namespace ExpressionLib
{
    public class TruthTables
    {
        public static void PrintTable(string expression)
        {
            Evaluator<bool> eval = new Evaluator<bool>(expression, new ExpressionLib.Contexts.SimpleLogic());

            foreach (string var in eval.Variables)
                Console.Write("| " + var + " ");

            Console.WriteLine("| " + expression);

            Console.WriteLine();
            Permutations(eval.Variables, (vals) => {
                foreach ( bool v in vals.Values )
                    Console.Write("| " + SimpleLogic.ToString(v) + " ");

                Console.WriteLine("| " + SimpleLogic.ToString(eval.Eval(vals)));
            });

            Console.WriteLine();
        }

        delegate void processorFunc(Dictionary<string, bool> vals);
        private static void Permutations(List<string> vars, processorFunc func)
        {
            var dict = new Dictionary<string, bool>();
            foreach (string name in vars)
                dict.Add(name, false);

            Permutations(0, vars, dict, func);
        }

        private static void Permutations(int index, List<string> vars, Dictionary<string, bool> vals, processorFunc func)
        {
            for ( int b = 0; b <= 1; b++ )
            {
                vals[vars[index]] = ( b == 1 );

                if (index < vars.Count - 1)
                    Permutations(index + 1, vars, vals, func);
                else
                    func(vals);
            }
        }
    }
}