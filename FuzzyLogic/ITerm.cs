namespace FuzzyLogic
{
    public interface ITerm
    {
        LinguisticType Type { get; }

        double WeightCenter { get; }

        string Name { get; }

        double Center {get;}

        double Width {get;}

        double CalcTruthDegree(double x);
    }
}