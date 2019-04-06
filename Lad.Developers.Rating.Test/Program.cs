using FuzzyLogic;
using FuzzyLogic.Statements;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lad.Developers.Rating.Test
{
    class Program
    {
        static readonly Random random = new Random();

        static LinguisticType LegalStateType, FinanceStateType, TransparencyType, HonestyType, PlanType, RatingType;

        static Dictionary<string, LinguisticVariable> Variables = new Dictionary<string, LinguisticVariable>{
            {"Правовое состояние", null },
            {"Финансовое состояние", null },
            {"Прозрачность", null },
            {"Честность", null },
            {"Соблюдение планов", null },
        };

        static MISORuleSet Rules;

        static void Main(string[] args)
        {
            // load types
            LegalStateType = LoadLinguisticType("data/types/legalstate.json");
            SaveLinguisticTypeLineCharDataSet(LegalStateType, "data/types/legalstate.graph.json");

            FinanceStateType = LoadLinguisticType("data/types/financestate.json");
            SaveLinguisticTypeLineCharDataSet(FinanceStateType, "data/types/financestate.graph.json");

            TransparencyType = LoadLinguisticType("data/types/transparency.json");
            SaveLinguisticTypeLineCharDataSet(TransparencyType, "data/types/transparency.graph.json");

            HonestyType = LoadLinguisticType("data/types/honesty.json");
            SaveLinguisticTypeLineCharDataSet(HonestyType, "data/types/honesty.graph.json");

            PlanType = LoadLinguisticType("data/types/plan.json");
            SaveLinguisticTypeLineCharDataSet(PlanType, "data/types/plan.graph.json");

            RatingType = LoadLinguisticType("data/types/rating.json");
            SaveLinguisticTypeLineCharDataSet(RatingType, "data/types/rating.graph.json");

            // load data
            LoadAndFuzzifyData("data/developers/developer.json");
            SaveFuzzifiedData("data/developers/developer.data");

            // load rules
            LoadRules("data/rules/ruleset.json");
            SaveRulesData("data/rules/ruleset.data");

            // get results
            SaveResultDataSet("data/results/result.graph.json");
            SaveResultValues("data/results/result.data");
        }

        private static void SaveResultValues(string fileName)
        {
            File.WriteAllText(fileName, RatingType.GetValue(Rules.GetResult().Defuzzify()).ToString());
        }

        private static void SaveResultDataSet(string fileName)
        {
            var results = Rules.GetResult();

            const int PointCount = 15;

            var labels = new List<int>();
            var termDatasets = Rules.ConclusionType.Terms.ToDictionary(x => x.Name, x => new List<double>());
            var resultDatasets = Rules.ConclusionType.Terms.ToDictionary(x => x.Name, x => new List<double>());

            double step = (Rules.ConclusionType.MaxValue - Rules.ConclusionType.MinValue) / PointCount;
            for (double label = Rules.ConclusionType.MinValue; label <= Rules.ConclusionType.MaxValue; label += step)
            {
                labels.Add((int)Math.Round(label));

                foreach (var term in Rules.ConclusionType.Terms)
                {
                    termDatasets[term.Name].Add(term.CalcTruthDegree(label));
                    resultDatasets[term.Name].Add(term.CalcTruthDegree(label) * results.GetDegree(term));
                }

            }

            JObject result = new JObject();
            result["labels"] = new JArray(labels.ToArray());
            result["datasets"] = new JArray(termDatasets.Select(x =>
            {
                var set = new JObject();
                var color = GenerateRandomColor();
                set["label"] = x.Key;
                set["data"] = new JArray(x.Value.ToArray());
                set["borderColor"] = color;
                set["fill"] = false;
                return set;
            }).Union(resultDatasets.Select(x =>
            {
                var set = new JObject();
                set["data"] = new JArray(x.Value.ToArray());
                return set;
            })).ToArray());

            File.WriteAllText(fileName, result.ToString());
        }

        private static void SaveRulesData(string fileName)
        {
            File.WriteAllText(fileName, Rules.ToString());
        }

        private static void LoadRules(string fileName)
        {
            Rules = new MISORuleSet(RatingType);

            var ruleset = JArray.Parse(File.ReadAllText(fileName));
            foreach (var rule in ruleset)
            {
                var premise = rule["premise"];
                var conclusion = (string)rule["conclusion"];

                Rules.AddRule(BuildStatement(premise), RatingType[conclusion]);
            }
        }

        private static IStatement BuildStatement(JToken statement)
        {
            var op = (string)statement["op"];
            switch (op)
            {
                case "=":
                    var variable = (string)statement["var"];
                    var value = (string)statement["val"];
                    return new AtomicStatement(Variables[variable], Variables[variable].Type[value]);
                case "and":
                    var st1 = BuildStatement(statement["left"]);
                    var st2 = BuildStatement(statement["right"]);
                    return new AndStatement(st1, st2);
                case "or":
                    st1 = BuildStatement(statement["left"]);
                    st2 = BuildStatement(statement["right"]);
                    return new OrStatement(st1, st2);
                case "not":
                    var st = BuildStatement(statement["statement"]);
                    return new NotStatement(st);
                default:
                    throw new NotImplementedException();
            }
        }

        private static void SaveFuzzifiedData(string fileName)
        {
            var sb = new StringBuilder();
            foreach (var variable in Variables.Values)
            {
                sb.AppendLine(variable?.ToString());
            }
            File.WriteAllText(fileName, sb.ToString());
        }

        private static void LoadAndFuzzifyData(string fileName)
        {
            var developerParams = JObject.Parse(File.ReadAllText(fileName));

            var legal = (double)developerParams["legal"];
            Variables["Правовое состояние"] = LegalStateType.CreateVariable("Правовое состояние");
            Variables["Правовое состояние"].Value = LegalStateType.GetValue(legal);

            var finance = (double)developerParams["finance"];
            Variables["Финансовое состояние"] = FinanceStateType.CreateVariable("Финансовое состояние");
            Variables["Финансовое состояние"].Value = FinanceStateType.GetValue(finance);

            var transparent = (double)developerParams["transparent"];
            Variables["Прозрачность"] = TransparencyType.CreateVariable("Прозрачность");
            Variables["Прозрачность"].Value = TransparencyType.GetValue(transparent);

            var honest = (double)developerParams["honest"];
            Variables["Честность"] = HonestyType.CreateVariable("Честность");
            Variables["Честность"].Value = HonestyType.GetValue(honest);

            var plan = (double)developerParams["plan"];
            Variables["Соблюдение планов"] = PlanType.CreateVariable("Соблюдение планов");
            Variables["Соблюдение планов"].Value = PlanType.GetValue(plan);
        }

        private static void SaveLinguisticTypeLineCharDataSet(LinguisticType type, string fileName)
        {
            const int PointCount = 15;

            var labels = new List<int>();
            var datasets = type.Terms.ToDictionary(x=>x.Name,x=>new List<double>());

            double step = (type.MaxValue - type.MinValue) / PointCount;
            for (double label = type.MinValue; label <= type.MaxValue; label+=step)
            {
                labels.Add((int)Math.Round(label));

                foreach (var term in type.Terms)
                {
                    datasets[term.Name].Add(term.CalcTruthDegree(label));
                }
                
            }

            JObject result = new JObject();
            result["labels"] = new JArray(labels.ToArray());
            result["datasets"] = new JArray(datasets.Select(x =>
            {
                var set = new JObject();
                var color = GenerateRandomColor();
                set["label"] = x.Key;
                set["data"] = new JArray(x.Value.ToArray());
                set["borderColor"] = color;
                set["fill"] = false;
                return set;
            }).ToArray());

            File.WriteAllText(fileName, result.ToString());
        }

        private static LinguisticType LoadLinguisticType(string fileName)
        {
            var typeParameters = JObject.Parse(File.ReadAllText(fileName));

            var name = (string)typeParameters["name"];
            var minValue = typeParameters.ContainsKey("min") ?
                (double)typeParameters["min"] : 0;
            var maxValue = typeParameters.ContainsKey("max") ?
                (double)typeParameters["max"] : int.MaxValue;

            var terms = (JArray)typeParameters["terms"];

            var type = new LinguisticType(name, minValue, maxValue);
            for (int i = 0; i < terms.Count; i++)
            {
                var term = terms[i];
                var termName = (string)term["name"];
                var termCenter = (double)term["center"];
                var termWidth = (double)term["width"];

                if (i == 0)
                    type.CreateTerm(termName, termCenter, termWidth, TermType.Left);
                else if (i==terms.Count-1)
                    type.CreateTerm(termName, termCenter, termWidth, TermType.Right);
                else
                    type.CreateTerm(termName, termCenter, termWidth);
            }

            return type;
        }

        private static string GenerateRandomColor() => string.Format("#{0:X6}", random.Next(0x1000000));

    }
}
