using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzyLogic
{
    public interface IStatement
    {
        IList<LinguisticVariable> Variables { get; }
        double GetTruthDegree();
    }
}
