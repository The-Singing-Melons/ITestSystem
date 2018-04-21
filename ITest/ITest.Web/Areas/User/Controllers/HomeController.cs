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
            var test = this.testService.GetTestById(id);
            var testQuestions = this.testService.GetTestQuestions(id);

            // VM
            var questionWithAnswersVM = new Dictionary<QuestionViewModel, IEnumerable<TakeTestAnswerViewModel>>();
            var testViewModel = this.mapper.MapTo<TestViewModel>(test);

            foreach (var question in testQuestions)
            {
                var answersForQuestion = this.questionService
                                        .GetAnswersForQuestion(question.Id);

                var questionVM = this.mapper.MapTo<QuestionViewModel>(question);
                var answersVM = this.mapper.EnumerableProjectTo<AnswerDto, TakeTestAnswerViewModel>(answersForQuestion);

                questionWithAnswersVM.Add(questionVM, answersVM);
            }

            var takeTestViewModel = new TakeTestViewModel
            {
                QuestionWithAnswersVM = questionWithAnswersVM,
                TestViewModel = testViewModel
            };

            return View(takeTestViewModel);
        }

        [HttpPost]
        public IActionResult SubmitTest(TakeTestViewModel takeTestViewModel)
        {
            return View();
        }

        [HttpGet]
        public TestViewModel GetRandomTests()
        {
            throw new NotImplementedException();
        }
    }
}
