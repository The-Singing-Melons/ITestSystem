using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Models;
using ITest.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ITest.Services.Data
{
    public class TestService : ITestService
    {
        private readonly IDataRepository<ApplicationUser> userRepo;
        private readonly IDataRepository<Test> testRepo;
        private readonly IDataSaver dataSaver;
        private readonly IMappingProvider mapper;

        public TestService(IDataRepository<ApplicationUser> userRepo,
            IDataRepository<Test> testRepo, IDataSaver dataSaver,
            IMappingProvider mapper)
        {
            this.userRepo = userRepo;
            this.testRepo = testRepo;
            this.dataSaver = dataSaver;
            this.mapper = mapper;
        }

        public IEnumerable<TestDto> GetAllTests(string userId)
        {
            var user = this.userRepo.All
                .Include(u => u.UserTests)
                    .ThenInclude(ut => ut.Test)
                        .ThenInclude(t => t.Category)
                .FirstOrDefault(u => u.Id == userId);

            var testsToReturn = user.UserTests.Select(ut =>
            {
                return new
                {
                    IsPassed = ut.IsPassed,
                    IsSubmitted = ut.IsSubmited,
                    TestType = ut.Test.Category.Name
                };
            });

            throw new NotImplementedException();
        }

        public IEnumerable<TestDto> GetUserTests(string id)
        {
            var userTests = userRepo.All.Where(u => u.Id == id)
                .FirstOrDefault().Tests;

            var userTestsDto = mapper.ProjectTo<TestDto>(userTests);

            return userTestsDto;
        }

        public void AddTestsToUser(ApplicationUser user)
        {
            var random = new Random();
            var JavaTests = testRepo.All
                .Where(t => t.Category.Name == "Java" && t.IsPublished)
                .ToList();
            int r = random.Next(JavaTests.Count);
            var javaTest = JavaTests[r];
            user.Tests.Add(javaTest);
            this.dataSaver.SaveChanges();
        }

        //private T GetRandomEntity<T>(IDataRepository<T> repo) where T : Test<Guid>
        //{
        //    var skip = (int)(rand.NextDouble() * repo.Items.Count());
        //    return repo.Items.OrderBy(o => o.ID).Skip(skip).Take(1).First();
        //}
    }
}
