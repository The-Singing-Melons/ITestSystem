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
            IQueryable<Test> userTests = userRepo.All
                .Include(u => u.Tests)
                .Where(u => u.Id == id)
                .SelectMany(x => x.Tests);

            var userTestsDto = mapper.ProjectTo<TestDto>(userTests);

            return userTestsDto.ToList();
        }

        public void AddTestsToUser(ApplicationUser user)
        {
            var javaTest = GetRandomJavaTest();
            var javascriptTest = GetRandomJavaScriptTest();
            var dotNetTest = GetRandomDotNetTest();
            var sqlTest = GetRandomSqlTest();
            user.Tests.Add(javaTest);
            user.Tests.Add(javascriptTest);
            user.Tests.Add(dotNetTest);
            user.Tests.Add(sqlTest);
            this.dataSaver.SaveChanges();
        }

        public TestDto GetTestById(string id)
        {
            var test = testRepo.All.Where(t => t.Id.ToString() == id)
                .FirstOrDefault();

            if (test == null)
            {
                throw new ArgumentNullException("Test not found!");
            }

            var testDto = this.mapper.MapTo<TestDto>(test);

            return testDto;
        }

        // need to refactor var random = new Random() 
        // code duplication & dependency
        private Test GetRandomSqlTest()
        {
            var random = new Random();
            var sqlTests = testRepo.All
                .Where(t => t.Category.Name == "SQL" && t.IsPublished)
                .ToList();
            int r = random.Next(sqlTests.Count);
            var sqlTest = sqlTests[r];

            return sqlTest;
        }

        private Test GetRandomDotNetTest()
        {
            var random = new Random();
            var dotNetTests = testRepo.All
                .Where(t => t.Category.Name == "DotNet" && t.IsPublished)
                .ToList();
            int r = random.Next(dotNetTests.Count);
            var dotNetTest = dotNetTests[r];

            return dotNetTest;
        }

        private Test GetRandomJavaScriptTest()
        {
            var random = new Random();
            var JavaScriptTests = testRepo.All
                .Where(t => t.Category.Name == "JavaScript" && t.IsPublished)
                .ToList();
            int r = random.Next(JavaScriptTests.Count);
            var javaScriptTest = JavaScriptTests[r];

            return javaScriptTest;
        }

        private Test GetRandomJavaTest()
        {
            var random = new Random();
            var JavaTests = testRepo.All
                .Where(t => t.Category.Name == "Java" && t.IsPublished)
                .ToList();
            int r = random.Next(JavaTests.Count);
            var javaTest = JavaTests[r];

            return javaTest;
        }

    }
}
