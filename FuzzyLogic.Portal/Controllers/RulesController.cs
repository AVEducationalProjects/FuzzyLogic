using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuzzyLogic.Portal.Data;
using FuzzyLogic.Portal.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace FuzzyLogic.Portal.Controllers
{
    public class RulesController : Controller
    {
        private readonly ILogger<RulesController> _logger;
        private readonly ILinguisticTypeRepositary _lingusticTypeRepositary;
        private readonly IRuleRepositary _ruleRepositary;

        public RulesController(ILogger<RulesController> logger,
            ILinguisticTypeRepositary lingusticTypeRepositary, IRuleRepositary ruleRepositary)
        {
            _logger = logger;
            _lingusticTypeRepositary = lingusticTypeRepositary;
            _ruleRepositary = ruleRepositary;
        }

        public async Task<IActionResult> Index()
        {
            var rules = await _ruleRepositary.List();
            return View(rules);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FuzzyRule rule)
        {
            rule.Id = ObjectId.GenerateNewId().ToString();
            await _ruleRepositary.Save(rule);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var rule = await _ruleRepositary.Get(ObjectId.Parse(id));
            return View(rule);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FuzzyRule rule)
        {
            await _ruleRepositary.Save(rule);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _ruleRepositary.Delete(ObjectId.Parse(id));
            return RedirectToAction("Index");
        }
    }
}