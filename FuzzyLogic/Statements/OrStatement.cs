using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzyLogic.Statements
{
    public class OrStatement :IStatement
    {
        public OrStatement(IStatement left, IStatement right)
        {
            Left = left;
            Right = right;
        }

        public IList<LinguisticVariable> Variables
        {
            get
            {
                return
                    Left.Variables.ToHashSet()
                    .Union(Right.Variables.ToHashSet())
                    .ToList();
            }
        }

        public IStatement Left { get; private set; }

        public IStatement Right { get; private set; }

        public double GetTruthDegree() => Math.Max(Left.GetTruthDegree(), Right.GetTruthDegree());

        public override string ToString()
        {
            return $"({Left.ToString()}) OR ({Right.ToString()})";
        }
    }
}
