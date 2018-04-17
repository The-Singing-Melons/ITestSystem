using ITest.Web.Areas.Admin.Models.ManageViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ITest.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Administrator")]
    [Area("Admin")]
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult CreateTest()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateTest(CreateTestViewModel model)
        {
            //if (validationsFail)
            //{
            //  return View(model);
            //}

            //When all fine

            return RedirectToAction(nameof(this.Index));
        }

        public IActionResult LoadQuestion()
        {
            return PartialView("_LoadQuestionPartial");
        }

        public IActionResult LoadAnswer()
        {
            return PartialView("_LoadAnswerPartial");
        }
    }
}