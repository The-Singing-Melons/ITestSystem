using System;
using System.Collections.Generic;
using System.Linq;
using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Models;
using ITest.Services.Data.Contracts;
using ITest.Web.Areas.User.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITest.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMappingProvider mapper;
        private readonly ITestService testService;
        private readonly IQuestionService questionService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICategoryService categoryService;

        public HomeController(IMappingProvider mapper, ITestService testService,
            UserManager<ApplicationUser> userManager, IQuestionService questionService, ICategoryService categoryService)
        {
            this.mapper = mapper;
            this.testService = testService;
            this.userManager = userManager;
            this.questionService = questionService;
            this.categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var allCategories = this.categoryService.GetAllCategories();
            var categoriesViewModel = this.mapper.EnumerableProjectTo
                <CategoryDto, CategoryViewModel>(allCategories);

            return View(categoriesViewModel);
        }

        public IActionResult GetRandomTest(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }

            var randomTest = this.testService.GetRandomTest(id);
            var randomTestViewModel = this.mapper.MapTo<TestViewModel>(randomTest);

            return Json(Url.Action("TakeTest/" + randomTestViewModel.Id));
        }

        public IActionResult TakeTest(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Id cannot be null or empty");
            }

            // DTO
            var testWithQuestions = this.testService.GetTestQuestionsWithAnswers(id);

            // VM
            var testWithQuestionsViewModel = this.mapper.MapTo<TestViewModel>(testWithQuestions);

            return View(testWithQuestionsViewModel);
        }

        [HttpPost]
        public IActionResult TakeTest(TestViewModel takeTestViewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(takeTestViewModel);
        }

        [HttpGet]
        public TestViewModel GetRandomTests()
        {
            throw new NotImplementedException();
        }
    }
}
