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
using ITest.DTO.TakeTest;

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
            this.userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            this.testRepo = testRepo ?? throw new ArgumentNullException(nameof(testRepo));
            this.dataSaver = dataSaver ?? throw new ArgumentNullException(nameof(dataSaver));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
        }

        public IEnumerable<TestDto> GetUserTests(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

            IQueryable<Test> userTests = userRepo.All
                .Include(u => u.Tests)
                .Where(u => u.Id == id)
                .SelectMany(x => x.Tests);

            var userTestsDto = mapper.ProjectTo<TestDto>(userTests);

            return userTestsDto.ToList();
        }

        public TestDto GetTestById(string testId)
        {
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            var test = testRepo.All
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
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentNullException("Category name cannot be null!");
            }

            var random = new Random();
            var allTestsFromCategory = testRepo.All
                .Where(t => t.Category.Name == categoryName &&
                            t.IsPublished)
                .ToList();

            // To-do: Add a try-catch block if there is no test in the category
            int r = random.Next(allTestsFromCategory.Count);
            var randomTest = allTestsFromCategory[r];

            var randomTestDto = this.mapper.MapTo<TestDto>(randomTest);

            return randomTestDto;
        }

        public TestDto GetTestQuestionsWithAnswers(string testId)
        {
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            var testWithQuestionsAndAnswers = testRepo.All
                                        .Where(t => t.Id.ToString() == testId)
                                        .Include(t => t.Category)
                                        .Include(t => t.Questions)
                                        .ThenInclude(q => q.Answers)
                                        .SingleOrDefault();

            var testWithQuestionsAndAnswersDto = this.mapper.MapTo<TestDto>(testWithQuestionsAndAnswers);

            return testWithQuestionsAndAnswersDto;
        }

        public void CreateTest(ManageTestDto testDto)
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
            this.dataSaver.SaveChanges();
        }

        private bool IsTestPassed(int testQuestionsCount, int totalCorrectQuestions)
        {
            if (testQuestionsCount <= 0)
            {
                throw new ArgumentException("Test questions count cannot be less or equal to zero");
            }

            if (totalCorrectQuestions < 0)
            {
                throw new ArgumentException("Test questions count cannot be less then zero");
            }

            var isPassed = false;
            var resultPercentage = (totalCorrectQuestions / testQuestionsCount) * 100;
            if (resultPercentage >= 80D)
            {
                isPassed = true;
            }

            return isPassed;
        }


        public bool IsTestPassed(string testId, TestRequestViewModelDto submitedTest)
        {
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            if (submitedTest == null)
            {
                throw new ArgumentNullException(nameof(submitedTest));
            }

            var testWithQuestions = this.GetTestQuestionsWithAnswers(testId);
            var totalCorrectQuestions = 0;

            for (int i = 0; i < testWithQuestions.Questions.Count; i++)
            {
                var correctAnswer = testWithQuestions
                    .Questions[i].Answers
                    .Where(x => x.IsCorrect == true)
                    .FirstOrDefault();

                var selectedAnswer = submitedTest.Questions[i].Answers;

                if (selectedAnswer == null)
                {
                    continue;
                }

                if (correctAnswer.Id == selectedAnswer)
                {
                    totalCorrectQuestions++;
                }
            }

            return this.IsTestPassed(testWithQuestions.Questions.Count(), totalCorrectQuestions);
        }

        public IEnumerable<TestDashBoardDto> GetTestsDashboardInfo()
        {
            var tests = this.testRepo.All;

            return this.mapper.EnumerableProjectTo<Test, TestDashBoardDto>(tests);
        }

        public ManageTestDto GetTestByNameAndCategory(string name, string category)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name cannot be null!");
            }

            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException("Category cannot be null!");
            }

            var test = this.testRepo.All
                .Where(t => t.Name == name && t.Category.Name == category)
                .Include(t => t.Category)
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .SingleOrDefault();

            return this.mapper.MapTo<ManageTestDto>(test);
        }

        public void EditTest(ManageTestDto editedDto)
        {
            if (editedDto == null)
            {
                throw new ArgumentNullException(nameof(editedDto));
            }

            var testToEdit = this.testRepo.All.Where(t => t.Id.ToString() == editedDto.Id).SingleOrDefault();

            testToEdit = this.mapper.MapTo(editedDto, testToEdit);

            var category = this.categoryRepo.All.SingleOrDefault(c => c.Name == editedDto.CategoryName)
                ?? throw new ArgumentException($"Category {editedDto.CategoryName} does not exists!");

            testToEdit.Category = category;

            this.testRepo.Update(testToEdit);
            this.dataSaver.SaveChanges();
        }

        public void PublishTest(string name, string category)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name cannot be null!");
            }

            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException("Category  cannot be null!");
            }

            var test = this.testRepo.All
                .Include(t => t.Category)
                .Where(t => t.Name == name && t.Category.Name == category)
                .FirstOrDefault();

            if (!test.IsPublished)
            {
                test.IsPublished = true;

                this.testRepo.Update(test);
                this.dataSaver.SaveChanges();
            }
        }

        public void DeleteTest(string name, string category)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name cannot be null!");
            }

            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException("Category cannot be null!");
            }

            var test = this.testRepo.All
                .Include(t => t.Category)
                .Where(t => t.Name == name && t.Category.Name == category)
                .FirstOrDefault();

            if (!test.IsDeleted)
            {
                test.IsDeleted = true;

                this.testRepo.Delete(test);
                this.dataSaver.SaveChanges();
            }
        }

        public void DisableTest(string name, string category)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name cannot be null!");
            }

            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException("Category cannot be null!");
            }

            var test = this.testRepo.All
                .Include(t => t.Category)
                .Where(t => t.Name == name && t.Category.Name == category)
                .FirstOrDefault();

            if (test.IsPublished)
            {
                test.IsPublished = false;

                this.testRepo.Update(test);
                this.dataSaver.SaveChanges();
            }
        }
    }
}
