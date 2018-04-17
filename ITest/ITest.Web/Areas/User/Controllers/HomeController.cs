using System;
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
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(IMappingProvider mapper, ITestService testService,
            UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this.testService = testService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            // possibly async method?
            var userId = this.userManager.GetUserId(this.HttpContext.User);
            var userTests = this.testService.GetUserTests(userId);

            var userTestsViewModel = new DashboardTestViewModel();
            var userTestsVM = this.mapper.EnumerableProjectTo<TestDto, DashboardTestViewModel>(userTests);

            return View(userTestsVM);
        }

        public IActionResult TakeTest(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Id cannot be null or empty");
            }
            var testDto = this.testService.GetTestById(id);

            var takeTestViewModel = new TakeTestViewModel();

            return View();
        }
    }
}
