using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionLib
{
    public interface IContext<T>
    {
        // The number of arguments the operator operates on
        int NumParams(string oprtr);

        // Should return the product of the values, processed by the operator
        T EvalOperator(string oprtr, List<T> values);

        // 
        bool IsValue(string c);
        bool IsValue(string c, out T r);

        //
        bool IsOperator(string c);

        //
        int PriorityOf(string c);

        //
        Associativity AssociativityOf(string c);
    }

    public enum Associativity
    {
        Left,
        Right
    }
}