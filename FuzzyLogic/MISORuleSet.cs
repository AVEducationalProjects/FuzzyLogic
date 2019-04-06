using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzyLogic
{
    public class MISORuleSet 
    {
        public LinguisticType ConclusionType { get; private set; }

        private readonly IDictionary<ITerm, IList<IStatement>> _rules = new Dictionary<ITerm, IList<IStatement>>();

        public MISORuleSet(LinguisticType conclusionType)
        {
            ConclusionType = conclusionType;
        }

        public void AddRule(IStatement statement, ITerm term)
        {
            if (!_rules.ContainsKey(term))
            {
                _rules.Add(term, new List<IStatement> { statement });
                return;
            }
            _rules[term].Add(statement);
        }

        public ILingusticVariableValue GetResult()
        {
            var result = new CombinedValue(ConclusionType);

            foreach (var term in ConclusionType.Terms)
                result.Set(term, _rules.ContainsKey(term) ? _rules[term].Max(s => s.GetTruthDegree()) : 0);

            return result;
        }

        public IList<MISORule> Rules
        {
            get
            {
                var result = new List<MISORule>();

                _rules.Keys.ToList().ForEach(t =>
                    result.AddRange(_rules[t].Select(s => new MISORule(s, t))));

                return result;
            }
        }

        public override string ToString()
        {
            return string.Concat(Rules.Select(x => $"{x} \n"));
        }
    }
}
