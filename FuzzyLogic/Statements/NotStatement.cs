using System.Collections.Generic;

namespace FuzzyLogic.Statements
{
    public class NotStatement : IStatement
    {
        public NotStatement(IStatement statement)
        {
            Statement = statement;
        }

        public IStatement Statement { get; private set; }

        public IList<LinguisticVariable> Variables => Statement.Variables;

        public double GetTruthDegree() => 1 - Statement.GetTruthDegree();

        public override string ToString()
        {
            return $"NOT ({Statement.ToString()})";
        }
    }
}
