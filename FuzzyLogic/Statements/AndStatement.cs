using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzyLogic.Statements
{
    public class AndStatement : IStatement
    {

        public AndStatement(IStatement left, IStatement right)
        {
            Left = left;
            Right = right;
        }

        public IStatement Left { get; private set; }
        public IStatement Right { get; private set; }

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

        public double GetTruthDegree() => Math.Min(Left.GetTruthDegree(), Right.GetTruthDegree());

        public override string ToString()
        {
            return $"({Left.ToString()}) AND ({Right.ToString()})";
        }
    }
}
