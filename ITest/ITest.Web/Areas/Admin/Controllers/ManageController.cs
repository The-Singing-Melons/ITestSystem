using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Services.Data.Contracts;
using ITest.Web.Areas.Admin.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ITest.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Administrator")]
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

            model.CreatedTests.Add(new TestViewModel()
            {
                TestName = "testfake1",
                CategoryName = "testcategory2",
                IsPublished = true
            });

            model.CreatedTests.Add(new TestViewModel()
            {
                TestName = "testfake2",
                CategoryName = "testcategory2",
                IsPublished = true
            });

            model.CreatedTests.Add(new TestViewModel()
            {
                TestName = "testfake6",
                CategoryName = "testcategory1",
                IsPublished = false
            });

            model.CreatedTests.Add(new TestViewModel()
            {
                TestName = "testfake5",
                CategoryName = "testcategory1",
                IsPublished = true
            });

            model.CreatedTests.Add(new TestViewModel()
            {
                TestName = "testfake4",
                CategoryName = "testcategory3",
                IsPublished = false
            });

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateTest()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateTest(CreateTestViewModel createTestViewModel)
        {
            if (createTestViewModel == null)
            {
                throw new ArgumentNullException(nameof(createTestViewModel));
            }
            //Vallidations

            var logggedUserId = this.userService.GetLoggedUserId(this.User);

            var createTestDto = this.mapper.MapTo<CreateTestDto>(createTestViewModel);
            createTestDto.CreatedByUserId = logggedUserId;

            this.testService.CreateTest(createTestDto);

            return RedirectToRoute(new
            {
                area = "Admin",
                controller = "Manage",
                action = "Index"
            });
        }
    }
}