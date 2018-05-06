using System;
using System.Linq;
using ITest.DTO.TakeTest;
using ITest.DTO.UserHome.Index;
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
        private readonly IUserAnswerService answerService;
        private readonly IQuestionService questionService;
        private readonly IUserManagerProvider userManager;
        private readonly ICategoryService categoryService;
        private readonly IUserTestService userTestService;


        public HomeController(IMappingProvider mapper, ITestService testService,
            IUserManagerProvider userManager, IQuestionService questionService,
            ICategoryService categoryService, IUserTestService userTestService,
            IUserAnswerService answerService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.testService = testService ?? throw new ArgumentNullException(nameof(testService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            this.userTestService = userTestService ?? throw new ArgumentNullException(nameof(userTestService));
            this.answerService = answerService ?? throw new ArgumentNullException(nameof(answerService));
        }


        public IActionResult Index()
        {
            var userId = this.userManager.GetUserId(this.HttpContext.User);

            var x = this.HttpContext.User;

            var overdueTestInProgress = this.userTestService
                .CheckForOverdueTestInProgress(userId);

            if (overdueTestInProgress)
            {
                return RedirectToAction("Index");
            }

            var allCategories = this.categoryService.GetAllCategories(userId);

            var categoriesViewModel = this.mapper
                .EnumerableProjectTo<CategoryIndexDto, CategoryViewModel>(allCategories)
                .ToList();

            return View(categoriesViewModel);
        }

        //[HttpGet("[action]/{categoryName}")]
        public IActionResult GetRandomTest(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                var randomTest = this.testService.GetRandomTest(id);
                var randomTestViewModel = this.mapper.MapTo<TestViewModel>(randomTest);
                this.userTestService.AddUserToTest(randomTest.Id, this.userManager.GetUserId(User));
                return Json(new { IsSuccessful = true, url = Url.Action("TakeTest/" + randomTestViewModel.Id) });
            }
            catch (ArgumentNullException)
            {
                TempData["Error"] = "No such category name!";
                return Json(new { IsSuccessful = false });
            }
            catch (ArgumentException)
            {
                TempData["NoTestInCategory"] = "No tests in this category!";
                return Json(new { IsSuccessful = false });
            }
        }

        public IActionResult TakeTest(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Id cannot be null or empty");
            }

            var testWithQuestions = this.testService.GetTestQuestionsWithAnswers(id);

            var testId = testWithQuestions.Id;
            var userId = this.userManager.GetUserId(this.HttpContext.User);

            if (this.userTestService.UserHasCompletedTest(userId, testId))
            {
                return RedirectToAction("Index");
            }

            this.testService.ShuffleTest(testWithQuestions);

            // VM
            var testWithQuestionsViewModel = this.mapper.MapTo<TestViewModel>(testWithQuestions);

            var timeRemaining = this.userTestService
                    .GetTimeRemainingForUserTest(userId, testId, testWithQuestions.Duration);
            testWithQuestionsViewModel.TimeRemaining = timeRemaining;

            return View(testWithQuestionsViewModel);

        }

        [HttpPost]
        public IActionResult TakeTest(TestRequestViewModel takeTestRequestViewModel)
        {
            // refactor TO-DO : Check for null answer in the requestVM and pass valid data to DB
            var userId = this.userManager.GetUserId(this.HttpContext.User);
            var isOverdue = this.userTestService.CheckForOverdueTestInProgress(userId);

            if (isOverdue)
            {
                RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var testId = takeTestRequestViewModel.Id;
                var submitedTest = this.mapper.MapTo<TestRequestDto>(takeTestRequestViewModel);


                this.answerService.AddAnswersToUser(userId, submitedTest.Questions);

                var isPassed = this.testService
                    .IsTestPassed(testId, submitedTest,
                                  this.testService.GetTestQuestionsWithAnswers(testId));

                this.userTestService.SubmitUserTest(testId,
                    userId, isPassed);

                return RedirectToAction("Index");
            }

            return View(takeTestRequestViewModel);
        }
    }
}
