using System;
using System.Runtime.Serialization;

namespace FuzzyLogic.Portal.Model
{
    [Serializable]
    internal class RuleParseException : Exception
    {
        public RuleParseException()
        {
        }

        public RuleParseException(string message) : base(message)
        {
        }

        public RuleParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RuleParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}