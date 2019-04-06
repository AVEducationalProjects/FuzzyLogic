using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzyLogic
{
    public class LinguisticVariable
    {
        public LinguisticVariable(string name, LinguisticType type)
        {
            Name = name;
            Type = type;
            Value = null;
        }

        public string Name { get; private set; }

        public LinguisticType Type { get; private set; }

        public ILingusticVariableValue Value { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }
}
