using System.Collections.Generic;
using System.Linq;

namespace FuzzyLogic
{
    class CombinedValue : ILingusticVariableValue
    {
        public LinguisticType Type { get; private set; }

        private readonly IDictionary<ITerm, double> _values = new Dictionary<ITerm, double>();

        public CombinedValue(LinguisticType type)
        {
            Type = type;
        }

        public double GetDegree(ITerm term)
        {
            return _values.ContainsKey(term) ? _values[term] : 0;
        }

        public void Set(ITerm term, double degree)
        {
            _values[term] = degree;
        }

        public double Defuzzify()
        {
            return _values.Select(x => x.Key.WeightCenter * x.Value).Sum() / _values.Values.Sum();
        }
    }
}
