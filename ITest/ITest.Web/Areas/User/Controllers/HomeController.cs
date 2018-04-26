using System;
using System.Collections.Generic;
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
        private readonly IQuestionService questionService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICategoryService categoryService;
        private readonly IUserTestService userTestService;


        public HomeController(IMappingProvider mapper, ITestService testService,
            UserManager<ApplicationUser> userManager, IQuestionService questionService,
            ICategoryService categoryService, IUserTestService userTestService)
        {
            this.mapper = mapper;
            this.testService = testService;
            this.userManager = userManager;
            this.questionService = questionService;
            this.categoryService = categoryService;
            this.userTestService = userTestService;
        }

        public IActionResult Index()
        {
            var allCategories = this.categoryService.GetAllCategories();
            var categoriesViewModel = this.mapper.EnumerableProjectTo
                                        <CategoryDto, CategoryViewModel>(allCategories).ToList();

            var userId = this.userManager.GetUserId(this.HttpContext.User);

            for (int i = 0; i < allCategories.Count; i++)
            {
                var result = this.userTestService
                    .CheckForCompletedUserTestInCategory(userId, allCategories[i].Name);

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
                var submitedTest = this.mapper.MapTo<TestRequestViewModelDto>
                    (takeTestRequestViewModel);

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
