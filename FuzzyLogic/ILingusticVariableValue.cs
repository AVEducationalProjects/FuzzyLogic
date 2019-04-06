namespace FuzzyLogic
{
    public interface ILingusticVariableValue
    {
        LinguisticType Type { get; }

        double GetDegree(ITerm term);

        double Defuzzify();
    }
}