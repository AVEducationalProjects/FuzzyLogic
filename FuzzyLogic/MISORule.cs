namespace FuzzyLogic
{
    public class MISORule
    {
        public IStatement Premise { get; private set; }
        public ITerm Conclusion { get; private set; }

        public MISORule(IStatement premise, ITerm conclusion)
        {
            Premise = premise;
            Conclusion = conclusion;
        }

        public override string ToString()
        {
            return $"{Premise} -> {Conclusion.Name}";
        }
    }
}