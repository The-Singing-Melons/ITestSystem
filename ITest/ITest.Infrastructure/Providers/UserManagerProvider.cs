using System.Linq;
using System.Security.Claims;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Models;
using Microsoft.AspNetCore.Identity;

namespace ITest.Infrastructure.Providers
{
    public class UserManagerProvider : IUserManagerProvider
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserManagerProvider(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public string GetUserId(ClaimsPrincipal principal)
        {
            return this.userManager.GetUserId(principal);
        }

        public virtual IQueryable<ApplicationUser> Users
        {
            get
            {
                return this.userManager.Users;
            }
        }
    }
}
