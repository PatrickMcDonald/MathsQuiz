using System.Security.Cryptography;
using MathsQuizApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MathsQuizApp.Controllers
{
    public class MultiplicationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new MultiplicationViewModel(NextRandom(), NextRandom());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(MultiplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Answer != model.Operand1 * model.Operand2)
            {
                ModelState.AddModelError(nameof(model.Answer), "Incorrect!");
            }

            return View(model);
        }

        private static int NextRandom() => RandomNumberGenerator.GetInt32(12) + 1;
    }
}
