using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Models;
using ITest.Services.Data.Contracts;
using ITest.Web.Areas.Admin.Models.ManageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITest.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Admin")]
    public class ManageController : Controller
    {
        private readonly ITestService testService;
        private readonly ICategoryService categoryService;
        private readonly IUserTestService userTestService;
        private readonly IUserAnswerService userAnswerService;
        private readonly IMappingProvider mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ManageController(
            ITestService testService,
            ICategoryService categoryService,
            IUserTestService userTestService,
            IUserAnswerService userAnswerService,
            UserManager<ApplicationUser> userManager,
            IMappingProvider mapper)
        {
            this.testService = testService ?? throw new ArgumentNullException(nameof(testService));
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            this.userTestService = userTestService ?? throw new ArgumentNullException(nameof(userTestService));
            this.userAnswerService = userAnswerService ?? throw new ArgumentNullException(nameof(userTestService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel();

            var testDtos = this.testService.GetTestsDashboardInfo();
            var createdTestsViewModels = this.mapper.EnumerableProjectTo<TestDashBoardDto, CreatedTestViewModel>(testDtos);
            model.CreatedTests = createdTestsViewModels.ToList();

            var resultDtos = this.userTestService.GetUserTestResults();
            var resultTestViewModels = this.mapper.EnumerableProjectTo<UserTestResultDto, TestResultViewModel>(resultDtos);
            model.TestResults = resultTestViewModels.ToList();

            return View(model);
        }

        public IActionResult TestScore(string userId, string testId)
        {
            try
            {
                var answersForTestDto = this.userAnswerService
                            .GetAnswersForTestDoneByUser(userId, testId);

                var answersForTestViewModel = this.mapper
                    .EnumerableProjectTo<UserAnswerDto, TestScoreAnswerForTestViewModel>
                    (answersForTestDto);

                var userDetailsViewModel = this.mapper
                    .MapTo<TestScoreUserDetailsViewModel>
                    (answersForTestDto.First());

                var testScoreViewModel = new TestScoreUserAnswerViewModel()
                {
                    AnswerForTestViewModels = answersForTestViewModel,
                    UserDetailsViewModel = userDetailsViewModel
                };

                return View(testScoreViewModel);
            }
            catch (ArgumentException)
            {
                var user = this.userManager.Users
                    .Where(x => x.Id == userId).FirstOrDefault();
                var test = this.testService.GetTestById(testId);

                var testScoreNoAnswers = new TestScoreUserAnswerViewModel();
                testScoreNoAnswers.UserDetailsViewModel.UserName = user.UserName;
                testScoreNoAnswers.UserDetailsViewModel.TestName = test.Name;
                testScoreNoAnswers.UserDetailsViewModel.TestCategory = test.Category.Name;

                return View(testScoreNoAnswers);
            }


        }

        [HttpGet]
        public IActionResult CreateTest()
        {
            var model = new ManageTestViewModel()
            {
                CategoryNames = this.categoryService.GetAllCategoriesNames().ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTest(ManageTestViewModel createTestViewModel)
        {
            if (createTestViewModel == null || !this.ModelState.IsValid
                || createTestViewModel.Questions.Any(q => q.Answers.All(a => !a.IsCorrect)))
            {
                createTestViewModel.CategoryNames = this.categoryService.GetAllCategoriesNames().ToList();
                return this.View(createTestViewModel);
            }

            var logggedUserId = this.userManager.GetUserId(this.HttpContext.User);

            var createTestDto = this.mapper.MapTo<ManageTestDto>(createTestViewModel);
            createTestDto.CreatedByUserId = logggedUserId;

            try
            {
                this.testService.CreateTest(createTestDto);
            }
            catch (Exception)
            {
                createTestViewModel.CategoryNames = this.categoryService.GetAllCategoriesNames().ToList();
                return this.View(createTestViewModel);
            }
            return RedirectToRoute(new
            {
                area = "Admin",
                controller = "Manage",
                action = "Index"
            });
        }

        [HttpGet]
        public IActionResult EditTest(string testName, string categoryName)
        {
            if (string.IsNullOrEmpty(testName))
            {
                return this.View();
            }

            if (string.IsNullOrEmpty(categoryName))
            {
                return this.View();
            }

            var testDto = this.testService.GetTestByNameAndCategory(testName, categoryName);

            var testViewModel = this.mapper.MapTo<ManageTestViewModel>(testDto);
            testViewModel.CategoryNames = this.categoryService.GetAllCategoriesNames().ToList();

            if (testDto.IsDisabled)
            {
                return this.View("EditPublishedTest", testViewModel);
            }

            return this.View(testViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTest(ManageTestViewModel manageTestViewModel)
        {
            if (manageTestViewModel == null || !this.ModelState.IsValid
                || manageTestViewModel.Questions.Any(q => q.Answers.All(a => !a.IsCorrect)))
            {
                manageTestViewModel.CategoryNames = this.categoryService.GetAllCategoriesNames().ToList();
                return this.View(manageTestViewModel);
            }

            var logggedUserId = this.userManager.GetUserId(this.HttpContext.User);

            var editTestDto = this.mapper.MapTo<ManageTestDto>(manageTestViewModel);
            editTestDto.CreatedByUserId = logggedUserId;

            try
            {
                this.testService.EditTest(editTestDto);
            }
            catch (Exception)
            {
                manageTestViewModel.CategoryNames = this.categoryService.GetAllCategoriesNames().ToList();
                return this.View(manageTestViewModel);
            }

            return RedirectToRoute(new
            {
                area = "Admin",
                controller = "Manage",
                action = "Index"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPublishedTest(ManageTestViewModel manageTestViewModel)
        {
            if (manageTestViewModel == null || !this.ModelState.IsValid
                || manageTestViewModel.Questions.Any(q => q.Answers.All(a => !a.IsCorrect)))
            {
                manageTestViewModel.CategoryNames = this.categoryService.GetAllCategoriesNames().ToList();
                return this.View(manageTestViewModel);
            }

            var logggedUserId = this.userManager.GetUserId(this.HttpContext.User);

            var editTestDto = this.mapper.MapTo<ManageTestDto>(manageTestViewModel);
            editTestDto.CreatedByUserId = logggedUserId;

            try
            {
                this.testService.EditTest(editTestDto);
            }
            catch (Exception)
            {
                manageTestViewModel.CategoryNames = this.categoryService.GetAllCategoriesNames().ToList();
                return this.View(manageTestViewModel);
            }

            return RedirectToRoute(new
            {
                area = "Admin",
                controller = "Manage",
                action = "Index"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PublishTest(PublishTestViewModel model)
        {
            if (model == null)
            {
                return this.Json(new JsonResult(new { isPublished = false }));
            }

            //validate rights

            try
            {
                this.testService.PublishTest(model.TestName, model.CategoryName);
            }
            catch (Exception)
            {
                return this.Json(new JsonResult(new { isPublished = false }));
            }

            return this.Json(new JsonResult(new { isPublished = true }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTest(string testName, string categoryName)
        {
            if (string.IsNullOrEmpty(testName))
            {
                return this.Json(false);
            }

            if (string.IsNullOrEmpty(categoryName))
            {
                return this.Json(false);
            }

            //validate rights

            try
            {
                this.testService.DeleteTest(testName, categoryName);
            }
            catch (Exception)
            {
                return this.Json(false);
            }


            return this.Json(true);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DisableTest(DisableTestViewModel model)
        {
            if (model == null)
            {
                return this.Json(new JsonResult(new { isDisabled = false }));
            }

            try
            {
                this.testService.DisableTest(model.TestName, model.CategoryName);
            }
            catch (Exception)
            {
                return this.Json(new JsonResult(new { isDisabled = false }));
            }

            return this.Json(new JsonResult(new { isDisabled = true }));
        }

        [HttpGet]
        public IActionResult GetPublishedTestPartial(string testName, string categoryName)
        {
            return this.PartialView("_PublishTestPartial", new PublishTestViewModel()
            {
                TestName = testName,
                CategoryName = categoryName
            });
        }

        [HttpGet]
        public IActionResult GetDisabledTestPartial(string testName, string categoryName)
        {
            return this.PartialView("_DisableTestPartial", new DisableTestViewModel()
            {
                TestName = testName,
                CategoryName = categoryName
            });
        }
    }
}