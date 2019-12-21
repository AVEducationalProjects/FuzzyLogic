using System.Threading.Tasks;
using FuzzyLogic.Portal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace FuzzyLogic.Portal.Controllers
{
    public class LingusticTypeController : Controller
    {
        private readonly ILogger<LingusticTypeController> _logger;
        private readonly ILinguisticTypeRepositary _lingusticTypeRepositary;

        public LingusticTypeController(ILogger<LingusticTypeController> logger,
            ILinguisticTypeRepositary lingusticTypeRepositary)
        {
            _logger = logger;
            _lingusticTypeRepositary = lingusticTypeRepositary;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _lingusticTypeRepositary.List();
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LinguisticType linguisticType)
        {
            linguisticType.Id = ObjectId.GenerateNewId().ToString();
            await _lingusticTypeRepositary.Save(linguisticType);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var linguisticType = await _lingusticTypeRepositary.Get(new ObjectId(id));
            return View(linguisticType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LinguisticType linguisticType)
        {
            await _lingusticTypeRepositary.Save(linguisticType);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _lingusticTypeRepositary.Delete(new ObjectId(id));
            return RedirectToAction("Index");
        }
    }
}
