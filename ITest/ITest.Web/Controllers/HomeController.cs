using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ITest.Web.Models;
using Microsoft.AspNetCore.Identity;
using ITest.Models;

namespace ITest.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (this.User != null &&
               this.User.Identity != null &&
               this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole("Administrator"))
                {
                    return this.RedirectToAction("Index", "Manage", new { area = "Admin" });
                }

                return this.RedirectToAction("Index", "Home", new { area = "User" });
            }

            return this.RedirectToAction(nameof(AccountController.Authorize), "Account");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
