using System;
using System.Collections.Generic;
using System.Text;
using ITest.Data.Repository;
using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Models;
using ITest.Services.Data.Contracts;

namespace ITest.Services.Data
{
    public class UserService : IUserService
    {
        private readonly IMappingProvider mapper;
        private readonly IDataRepository<ApplicationUser> userRepo;

        public IEnumerable<TestDto> GetUserTest()
        {
            throw new NotImplementedException();
        }
    }
}
