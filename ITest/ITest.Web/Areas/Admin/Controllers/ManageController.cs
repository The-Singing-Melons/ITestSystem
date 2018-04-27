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
        private readonly IMappingProvider mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ManageController(ITestService testService, ICategoryService categoryService, IUserTestService userTestService, UserManager<ApplicationUser> userManager, IMappingProvider mapper)
        {
            this.testService = testService ?? throw new ArgumentNullException(nameof(testService));
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            this.userTestService = userTestService ?? throw new ArgumentNullException(nameof(userTestService));
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
            if (createTestViewModel == null || !this.ModelState.IsValid)
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

            return this.View(testViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTest(ManageTestViewModel manageTestViewModel)
        {
            if (manageTestViewModel == null || !this.ModelState.IsValid)
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

        [HttpGet]
        public IActionResult PublishTest(string testName, string categoryName)
        {
            if (string.IsNullOrEmpty(testName))
            {
                return this.View();
            }

            if (string.IsNullOrEmpty(categoryName))
            {
                return this.View();
            }

            var isPublished = this.testService.PublishTest(testName, categoryName);

            return this.Json(isPublished);
        }

        [HttpGet]
        public IActionResult DeleteTest(string testName, string categoryName)
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

            return this.View(testViewModel);
        }
    }
}