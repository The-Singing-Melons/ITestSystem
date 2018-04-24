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
        private readonly IMappingProvider mapper;

        public ManageController(ITestService testService, IUserService userService, IMappingProvider mapper)
        {
            this.testService = testService ?? throw new ArgumentNullException(nameof(testService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
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
            return View(new CreateTestViewModel() { Questions = new List<CreateQuestionViewModel>() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTest(CreateTestViewModel createTestViewModel)
        {
            if (createTestViewModel == null)
            {
                return View(createTestViewModel);
            }
            //Vallidations

            if (!this.ModelState.IsValid)
            {
                return View(createTestViewModel);
            }

            var logggedUserId = this.userService.GetLoggedUserId(this.User);

            var createTestDto = this.mapper.MapTo<CreateTestDto>(createTestViewModel);
            createTestDto.CreatedByUserId = logggedUserId;

            try
            {
                this.testService.CreateTest(createTestDto);
            }
            catch (Exception)
            {
                return View(createTestViewModel);
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