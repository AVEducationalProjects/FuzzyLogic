using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzyLogic.Statements
{
    public class AtomicStatement : IStatement
    {
        public AtomicStatement(LinguisticVariable variable, ITerm term)
        {
            Variable = variable;
            Term = term;
        }

        public LinguisticVariable Variable { get; set; }

        public ITerm Term { get; private set; }

        public IList<LinguisticVariable> Variables => new List<LinguisticVariable> { Variable };

        public double GetTruthDegree()
        {
            if (Variable.Value == null)
                throw new ApplicationException($"Value of variable {Variable.Name} not set");
            return Variable.Value.GetDegree(Term);
        }

        public override string ToString()
        {
            return $"{Variable.Name} = {Term.Name}";
        }
    }
}
