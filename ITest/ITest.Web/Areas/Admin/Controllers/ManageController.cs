using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
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
        private readonly IUserService userService;
        private readonly ICategoryService categoryService;
        private readonly IMappingProvider mapper;

        public ManageController(ITestService testService, IUserService userService, ICategoryService categoryService, IMappingProvider mapper)
        {
            this.testService = testService ?? throw new ArgumentNullException(nameof(testService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IActionResult Index()
        {
            var model = new CreatedTestsViewModel();

            var testDtos = this.testService.GetTestsDashboardInfo();
            var createdTestsViewModels = new List<TestViewModel>();

            testDtos
                .ToList()
                .ForEach(tDto =>
            {
                var createdTestsViewModel = this.mapper.MapTo<TestViewModel>(tDto);
                createdTestsViewModels.Add(createdTestsViewModel);
            });

            model.CreatedTests = createdTestsViewModels;

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
                return this.View(createTestViewModel);
            }

            var logggedUserId = this.userService.GetLoggedUserId(this.User);

            var createTestDto = this.mapper.MapTo<ManageTestDto>(createTestViewModel);
            createTestDto.CreatedByUserId = logggedUserId;

            try
            {
                this.testService.CreateTest(createTestDto);
            }
            catch (Exception)
            {
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
                return this.View(manageTestViewModel);
            }

            var logggedUserId = this.userService.GetLoggedUserId(this.User);

            var createTestDto = this.mapper.MapTo<ManageTestDto>(manageTestViewModel);
            createTestDto.CreatedByUserId = logggedUserId;

            try
            {
                this.testService.EditTest(createTestDto);
            }
            catch (Exception)
            {
                return this.View(manageTestViewModel);
            }

            return RedirectToRoute(new
            {
                area = "Admin",
                controller = "Manage",
                action = "Index"
            });
        }
    }
}