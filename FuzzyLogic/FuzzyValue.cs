namespace FuzzyLogic
{
    class FuzzyValue : ILingusticVariableValue
    {
        public ITerm Term { get; private set; }

        public double Degree { get; private set; }

        public LinguisticType Type { get; set; }

        public FuzzyValue(LinguisticType type, ITerm term, double degree)
        {
            Type = type;
            Term = term;
            Degree = degree;
        }

        public double GetDegree(ITerm term) => (term == Term) ? Degree : 0;

        public double Defuzzify() => Term.WeightCenter;
    }
}