using System;
using System.Linq;
using ITest.DTO;
using ITest.DTO.TakeTest;
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICategoryService categoryService;
        private readonly IUserTestService userTestService;


        public HomeController(IMappingProvider mapper, ITestService testService,
            UserManager<ApplicationUser> userManager, IQuestionService questionService,
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

            var testInProgress = this.userTestService
                .CheckForTestInProgress(userId);

            if (testInProgress != null)
            {
                var endTime = testInProgress.StartedOn
                    .AddMinutes(testInProgress.Test.Duration);

                var timeRemaining = Math.Round((endTime - DateTime.Now).TotalSeconds);

                if (timeRemaining == 0)
                {
                    this.userTestService
                        .SubmitUserTest(testInProgress.Test.Id, userId, false);
                    return RedirectToAction("Index");
                }
            }

            // add cache here
            var allCategories = this.categoryService.GetAllCategories();
            var categoriesViewModel = this.mapper
                .EnumerableProjectTo<CategoryDto, CategoryViewModel>(allCategories)
                .ToList();

            var allTestsDoneByUser = this.userTestService
                                                .GetAllTestsDoneByUser(userId);

            for (int i = 0; i < allCategories.Count; i++)
            {
                var result = this.userTestService
                    .CheckForCompletedUserTestInCategory(userId, allCategories[i].Name, allTestsDoneByUser);

                categoriesViewModel[i].HasUserTakenTestForThisCategory = result;
            }

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

            // DTO
            var testWithQuestions = this.testService.GetTestQuestionsWithAnswers(id);

            // VM
            var testWithQuestionsViewModel = this.mapper.MapTo<TestViewModel>(testWithQuestions);

            var userId = this.userManager.GetUserId(this.HttpContext.User);
            var testId = testWithQuestions.Id;

            if (this.userTestService.UserStartedTest(testId, userId))
            {
                var endTime = this.userTestService.GetStartingTimeForUserTest(userId, testId).AddMinutes(testWithQuestions.Duration);

                var timeRemaining = Math.Round((endTime - DateTime.Now).TotalSeconds);

                testWithQuestionsViewModel.TimeRemaining = timeRemaining;

                if (timeRemaining == 0)
                {
                    this.userTestService.SubmitUserTest(testId, userId, false);
                    return RedirectToAction("Index");
                }

                return View(testWithQuestionsViewModel);
            }
            else
            {
                this.userTestService.AddUserToTest(testWithQuestionsViewModel.Id, userId);

                testWithQuestionsViewModel.TimeRemaining =
                    Convert.ToInt32(((DateTime.Now.AddMinutes
                    (testWithQuestionsViewModel.Duration) - DateTime.Now).TotalSeconds));
                return View(testWithQuestionsViewModel);
            }
        }

        [HttpPost]
        public IActionResult TakeTest(TestRequestViewModel takeTestRequestViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = this.userManager.GetUserId(this.HttpContext.User);
                var testId = takeTestRequestViewModel.Id;
                var submitedTest = this.mapper.MapTo<TestRequestViewModelDto>(takeTestRequestViewModel);


                this.answerService.AddAnswersToUser(userId, submitedTest.Questions);

                var isPassed = this.testService
                    .IsTestPassed(testId, submitedTest);

                this.userTestService.SubmitUserTest(testId,
                    userId, isPassed);

                return RedirectToAction("Index");
            }

            return View(takeTestRequestViewModel);
        }
    }
}
