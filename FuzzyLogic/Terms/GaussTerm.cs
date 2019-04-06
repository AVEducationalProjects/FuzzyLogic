using System;

namespace FuzzyLogic.Terms
{
    class GaussTerm : ITerm
    {
        public GaussTerm(LinguisticType type, string name, double center, double width)
        {
            Type = type;
            Name = name;
            Center = center;
            Width = width;
        }

        public LinguisticType Type { get; private set; }

        public string Name { get; private set; }

        public double Center { get; private set; }

        public double Width { get; private set; }

        public double WeightCenter => Center;

        public double CalcTruthDegree(double x) => Math.Exp(-0.5 * Math.Pow((x - Center) / Width, 2));
    }
}

// exp(-0.5*((x-center)/width)^2)
