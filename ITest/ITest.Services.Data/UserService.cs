using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using ITest.Data.Repository;
using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Models;
using ITest.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;

namespace ITest.Services.Data
{
    public class UserService : IUserService
    {
        private readonly IMappingProvider mapper;
        private readonly IDataRepository<ApplicationUser> userRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public IEnumerable<TestDto> GetUserTest()
        {
            throw new NotImplementedException();
        }

        public string GetLoggedUserId(ClaimsPrincipal user)
        {
            return this.userManager.GetUserId(user);
        }
    }
}
