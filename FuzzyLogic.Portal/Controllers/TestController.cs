using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuzzyLogic.Portal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FuzzyLogic.Portal.Controllers
{
    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;
        private readonly ILinguisticTypeRepositary _lingusticTypeRepositary;
        private readonly IRuleRepositary _ruleRepositary;
        
        public TestController(ILogger<TestController> logger,
            ILinguisticTypeRepositary lingusticTypeRepositary, IRuleRepositary ruleRepositary)
        {
            _logger = logger;
            _lingusticTypeRepositary = lingusticTypeRepositary;
            _ruleRepositary = ruleRepositary;
        }

        public async Task<IActionResult> Index()
        {
            var types = await _lingusticTypeRepositary.List();
            var ruleDefinitions = await _ruleRepositary.List();

            var parsed = ruleDefinitions.Select(x => x.GetRuleParams(types)).ToList();
            var inputVars = parsed.SelectMany(x => x.proposal.Variables).Distinct().OrderBy(x=>x.Name).ToList();

            return View(inputVars);
        }

        public async Task<IActionResult> Calculate()
        {
            var types = await _lingusticTypeRepositary.List();
            var ruleDefinitions = await _ruleRepositary.List();

            var parsed = ruleDefinitions.Select(x => x.GetRuleParams(types)).ToList();
            
            var inputVars = parsed.SelectMany(x => x.proposal.Variables).Distinct().OrderBy(x => x.Name).ToList();
            for (int i = 0; i < inputVars.Count; i++)
            {
                inputVars[i].Value = inputVars[i].Type.GetValue(double.Parse(HttpContext.Request.Query[$"var_{i}"]));
            }

            var ruleSets = new Dictionary<string, MISORuleSet>();
            foreach (var item in parsed)
            {
                if (!ruleSets.ContainsKey(item.conclusionType.Name))
                    ruleSets[item.conclusionType.Name] = new MISORuleSet(item.conclusionType);

                ruleSets[item.conclusionType.Name].AddRule(item.proposal, item.conclusionTerm);
            }


            return PartialView(ruleSets.Values.OrderBy(x=>x.ConclusionType.Name));
        }
    }
}

