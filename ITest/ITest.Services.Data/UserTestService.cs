using System;
using System.Collections.Generic;
using System.Linq;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Models;
using ITest.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using ITest.DTO;

namespace ITest.Services.Data
{
    public class UserTestService : IUserTestService
    {
        private readonly IDataRepository<ApplicationUser> userRepo;
        private readonly IDataRepository<Test> testRepo;
        private readonly IDataRepository<UserTest> userTestRepo;
        private readonly IDataSaver dataSaver;
        private readonly IMappingProvider mapper;

        public UserTestService(IDataRepository<ApplicationUser> userRepo,
            IDataRepository<Test> testRepo, IDataSaver dataSaver,
            IMappingProvider mapper, IDataRepository<Category> categoryRepo,
            IDataRepository<UserTest> userTestRepo)
        {
            this.userRepo = userRepo;
            this.testRepo = testRepo;
            this.userTestRepo = userTestRepo;
            this.dataSaver = dataSaver;
            this.mapper = mapper;
        }

        public void AddUserToTest(string testId, string userId)
        {
            var userToTestObject = new UserTest
            {
                TestId = Guid.Parse(testId),
                UserId = userId,
                IsDeleted = false,
                StartedOn = DateTime.Now
            };

            this.userTestRepo.Add(userToTestObject);
            this.dataSaver.SaveChanges();
        }

        public DateTime GetStartingTimeForUserTest(string userId, string testId)
        {
            var currentTest = this.userTestRepo.All
                .Where(x => x.UserId == userId && x.TestId.ToString() == testId)
                .FirstOrDefault();

            return currentTest.StartedOn;
        }

        public bool UserStartedTest(string testId, string userId)
        {
            var userStartedTest = false;

            userStartedTest = this.userTestRepo.All
                .Any(ut => ut.UserId == userId & ut.TestId.ToString() == testId);

            return userStartedTest;
        }

        public IEnumerable<UserTest> GetAllTestsDoneByUser(string userId)
        {
            var testsTakenByUser = this.userTestRepo.All
                        .Where(x => x.UserId.ToString() == userId)
                        .Include(t => t.Test)
                        .ThenInclude(t => t.Category)
                        .ToList();

            return testsTakenByUser;
        }

        public bool CheckForCompletedUserTestInCategory(string userId, string categoryName, IEnumerable<UserTest> testsTakenByUser)
        {
            var isTestTaken = testsTakenByUser
                .Any(x => x.Test.Category.Name == categoryName
                                                    && x.IsSubmited == true);

            return isTestTaken;
        }

        public void SubmitUserTest(string testId, string userId, bool isPassed)
        {
            var currentTest = this.userTestRepo.All
              .Where(x => x.UserId == userId && x.TestId.ToString() == testId)
              .FirstOrDefault();

            currentTest.IsPassed = isPassed;
            currentTest.IsSubmited = true;
            var testExecutionTime = currentTest.StartedOn - DateTime.Now;
            currentTest.ExecutionTime = Math.Abs(testExecutionTime.TotalMinutes);

            this.dataSaver.SaveChanges();
        }

        public IEnumerable<UserTestResultDto> GetUserTestResults()
        {
            var results = this.userTestRepo.All
                .Include(ut => ut.User)
                .Include(ut => ut.Test)
                .ThenInclude(t => t.Category);

            return this.mapper.ProjectTo<UserTestResultDto>(results).ToList();
        }

        public UserTestDto CheckForTestInProgress(string userId)
        {
            var result = this.userTestRepo.All
                .Where(ut => ut.UserId == userId && ut.IsSubmited == null)
                .Include(ut => ut.Test)
                .FirstOrDefault();

            var userTestDto = this.mapper.MapTo<UserTestDto>(result);
            return userTestDto;

        }
    }
}
