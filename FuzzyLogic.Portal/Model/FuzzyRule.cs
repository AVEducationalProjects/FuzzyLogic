using FuzzyLogic.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuzzyLogic.Portal.Model
{
    public class FuzzyRule
    {
        public string Id { get; set; }

        public string RuleDefinition { get; set; }

        static IDictionary<string, LinguisticVariable> _variables = new Dictionary<string, LinguisticVariable>();

        public (LinguisticType conclusionType, ITerm conclusionTerm, IStatement proposal) GetRuleParams(IList<LinguisticType> linguisticTypes)
        {
            if (!RuleDefinition.Contains("->"))
                throw new RuleParseException();

            var proposalDefinition = RuleDefinition.Substring(0, RuleDefinition.IndexOf("->")).Trim();
            var conclusionDefinition = RuleDefinition.Substring(RuleDefinition.IndexOf("->") + 2).Trim();

            var proposal = ParseProposal(proposalDefinition, linguisticTypes);

            if (!conclusionDefinition.Contains("="))
                throw new RuleParseException();

            var conclusionTypeDefinition = conclusionDefinition.Substring(0, conclusionDefinition.IndexOf("=")).Trim();
            var conclusionTermDefinition = conclusionDefinition.Substring(conclusionDefinition.IndexOf("=") + 1).Trim();
            var conclusionType = linguisticTypes.SingleOrDefault(x => x.Name == conclusionTypeDefinition);
            if (conclusionType == null)
                throw new RuleParseException();

            var conclusionTerm = conclusionType.Terms.SingleOrDefault(x => x.Name == conclusionTermDefinition);
            if (conclusionTerm == null)
                throw new RuleParseException();

            return (conclusionType, conclusionTerm, proposal);
        }

        private IStatement ParseProposal(string proposalDefinition, IList<LinguisticType> linguisticTypes)
        {
            if (proposalDefinition.Contains("("))
            {
                (var left, var right, var op) = GetBinaryOpTokens(proposalDefinition);
                switch (op)
                {
                    case "И":
                        return new AndStatement(ParseProposal(left, linguisticTypes), ParseProposal(right, linguisticTypes));
                    case "ИЛИ":
                        return new OrStatement(ParseProposal(left, linguisticTypes), ParseProposal(right, linguisticTypes));
                    case "НЕ":
                        return new NotStatement(ParseProposal(right, linguisticTypes));
                    default:
                        throw new RuleParseException();
                }
            }

            if (!proposalDefinition.Contains("="))
                throw new RuleParseException();

            var typeDefinition = proposalDefinition.Substring(0, proposalDefinition.IndexOf("=")).Trim();
            var termDefinition = proposalDefinition.Substring(proposalDefinition.IndexOf("=") + 1).Trim();
            var type = linguisticTypes.SingleOrDefault(x => x.Name == typeDefinition);
            if (type == null)
                throw new RuleParseException();

            var term = type.Terms.SingleOrDefault(x => x.Name == termDefinition);
            if (term == null)
                throw new RuleParseException();

            if (!_variables.ContainsKey(type.Name))
                _variables[type.Name] = new LinguisticVariable(type.Name, type);

            return new AtomicStatement(_variables[type.Name], term);
        }

        private (string left, string right, string op) GetBinaryOpTokens(string proposalDefinition)
        {
            if (proposalDefinition.First() != '(')
            {
                if (!proposalDefinition.StartsWith("НЕ"))
                    throw new RuleParseException();

                var parameter = proposalDefinition.Substring(2).Trim();
                parameter = parameter.Substring(1, parameter.Length - 2);

                return (null, parameter, "НЕ");
            }

            int counter = 1;
            int position = 1;
            while (position < proposalDefinition.Length && counter > 0)
            {
                if (proposalDefinition[position] == '(')
                    counter++;
                if (proposalDefinition[position] == ')')
                    counter--;

                position++;
            }

            var left = proposalDefinition.Substring(1, position - 2);
            proposalDefinition = proposalDefinition.Substring(position).Trim();


            if (!proposalDefinition.StartsWith("И") && !proposalDefinition.StartsWith("ИЛИ"))
                throw new RuleParseException();

            var op = proposalDefinition.Substring(0, proposalDefinition.IndexOf("(") - 1).Trim();
            var right = proposalDefinition.Substring(proposalDefinition.IndexOf("(")).Trim();
            right = right.Substring(1, right.Length - 2);

            return (left, right, op);
        }
    }
}
