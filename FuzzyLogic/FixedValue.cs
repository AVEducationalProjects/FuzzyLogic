using System;
using System.Linq;

namespace FuzzyLogic
{
    class FixedValue : ILingusticVariableValue
    {
        public FixedValue(LinguisticType type, double value)
        {
            Type = type;
            Value = value;
        }

        public LinguisticType Type { get; private set; }
        public double Value { get; private set; }

        public double Defuzzify() => Value;

        public double GetDegree(ITerm term) => term.CalcTruthDegree(Value);

        public override string ToString()
        {
            return $"Value: {Value} (" +
                string.Concat(Type.Terms.Select(x => $"{x.Name}-{Math.Round(x.CalcTruthDegree(Value)*100)}% ")) 
                + ")";
        }

    }
}