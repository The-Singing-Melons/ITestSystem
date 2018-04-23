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
using Microsoft.AspNetCore.Identity;

namespace ITest.Services.Data
{
    public class TestService : ITestService
    {
        private readonly IDataRepository<ApplicationUser> userRepo;
        private readonly IDataRepository<Test> testRepo;
        private readonly IDataRepository<Category> categoryRepo;
        private readonly IDataSaver dataSaver;
        private readonly IMappingProvider mapper;

        public TestService(IDataRepository<ApplicationUser> userRepo,
            IDataRepository<Test> testRepo, IDataSaver dataSaver,
            IMappingProvider mapper, IDataRepository<Category> categoryRepo)
        {
            this.userRepo = userRepo;
            this.testRepo = testRepo;
            this.dataSaver = dataSaver;
            this.mapper = mapper;
            this.categoryRepo = categoryRepo;
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

        public TestDto GetTestById(string testId)
        {
            var test = testRepo.All
                .Include(t => t.Category)
                .Where(t => t.Id.ToString() == testId)
                .FirstOrDefault();

            if (test == null)
            {
                throw new ArgumentNullException("Test not found!");
            }

            var testDto = this.mapper.MapTo<TestDto>(test);

            return testDto;
        }

        public TestDto GetRandomTest(string categoryName)
        {
            // get test with and category and test in a single query
            // how?
            var random = new Random();
            var allTestsFromCategory = testRepo.All
                .Where(t => t.Category.Name == categoryName &&
                            t.IsPublished)
                .ToList();

            int r = random.Next(allTestsFromCategory.Count);
            var randomTest = allTestsFromCategory[r];

            var randomTestDto = this.mapper.MapTo<TestDto>(randomTest);

            return randomTestDto;
        }

        public IEnumerable<QuestionDto> GetTestQuestions(string testId)
        {

            var testQuestions = testRepo.All
                                    .Include(t => t.Questions)
                                    .Where(t => t.Id.ToString() == testId)
                                    .SelectMany(t => t.Questions);

            var testQuestionDto = mapper.ProjectTo<QuestionDto>(testQuestions);

            return testQuestionDto.ToList();
        }

        public IEnumerable<TestDto> GetRandomTestForEachCategory()
        {
            var testsForEachCategory = new List<TestDto>();
            var allCategories = this.categoryRepo.All;

            foreach (var c in allCategories)
            {
                //var testForCategory = this.GetRandomTest();
            }

            return testsForEachCategory;
        }

        public void CreateTest(CreateTestDto testDto)
        {
            if (testDto == null)
            {
                throw new ArgumentNullException(nameof(testDto));
            }

            var testToAdd = this.mapper.MapTo<Test>(testDto);

            var category = this.categoryRepo.All.SingleOrDefault(c => c.Name == testDto.CategoryName)
                ?? throw new ArgumentException($"Category {testDto.CategoryName} does not exists!");
            testToAdd.Category = category;

            this.testRepo.Add(testToAdd);
            this.dataSaver.SaveChangesAsync();
        }
    }
}
