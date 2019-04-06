using FuzzyLogic;
using FuzzyLogic.Statements;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = new LinguisticType("Рейтинговые баллы");
            points.CreateTerm("Мало", 20, 5, TermType.Left);
            points.CreateTerm("Средне", 40, 20);
            points.CreateTerm("Много", 70, 10, TermType.Right);

            var skips = new LinguisticType("Пропуски");
            skips.CreateTerm("Мало", 0, 10, TermType.Left);
            skips.CreateTerm("Средне", 5, 5);
            skips.CreateTerm("Много", 10, 10, TermType.Right);

            var studentPoints = points.CreateVariable("Баллы студента");
            studentPoints.Value = points.GetValue(10.0);
            var studentSkips = skips.CreateVariable("Пропуски студента");
            studentSkips.Value = skips.GetValue(20.0);

            var statement1 = new OrStatement(
                new AtomicStatement(studentPoints, points["Мало"]),
                new AtomicStatement(studentSkips, skips["Много"]));


            var call = new LinguisticType("Необходимость звонка", 0, 10);
            call.CreateTerm("Срочно звонить", 0, 4, TermType.Left);
            call.CreateTerm("Поговорить на собрании", 5, 4);
            call.CreateTerm("Не звонить", 10, 4, TermType.Right);

            var statement2 = new AndStatement(
                new AtomicStatement(studentPoints, points["Много"]),
                new AtomicStatement(studentSkips, skips["Мало"]));

            var ruleset = new MISORuleSet(call);
            ruleset.AddRule(statement1, call["Срочно звонить"]);
            ruleset.AddRule(statement2, call["Не звонить"]);

            Console.WriteLine(studentPoints);
            Console.WriteLine(studentSkips);

            Console.WriteLine(ruleset);
            Console.WriteLine(call.GetValue(ruleset.GetResult().Defuzzify()));

        }
    }
}
