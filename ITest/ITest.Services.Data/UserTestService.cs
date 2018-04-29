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
            this.userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            this.testRepo = testRepo ?? throw new ArgumentNullException(nameof(testRepo));
            this.userTestRepo = userTestRepo ?? throw new ArgumentNullException(nameof(userTestRepo));
            this.dataSaver = dataSaver ?? throw new ArgumentNullException(nameof(dataSaver));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void AddUserToTest(string testId, string userId)
        {
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

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
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

            var currentTest = this.userTestRepo.All
                .Where(x => x.UserId == userId && x.TestId.ToString() == testId)
                .FirstOrDefault();

            return currentTest.StartedOn;
        }

        public bool UserStartedTest(string testId, string userId)
        {
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

            var userStartedTest = false;

            userStartedTest = this.userTestRepo.All
                .Any(ut => ut.UserId == userId & ut.TestId.ToString() == testId);

            return userStartedTest;
        }

        public IEnumerable<UserTest> GetAllTestsDoneByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

            // must project to dto..
            var testsTakenByUser = this.userTestRepo.All
                        .Where(x => x.UserId.ToString() == userId)
                        .Include(t => t.Test)
                        .ThenInclude(t => t.Category);

            var userTestDtos = this.mapper.ProjectTo<UserTestDto>(testsTakenByUser);

            return testsTakenByUser;
        }

        public bool CheckForCompletedUserTestInCategory(string userId, string categoryName, IEnumerable<UserTest> testsTakenByUser)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentNullException("CategoryName cannot be null!");
            }

            var isTestTaken = testsTakenByUser
                .Any(x => x.Test.Category.Name == categoryName
                                                    && x.IsSubmited == true);

            return isTestTaken;
        }

        public void SubmitUserTest(string testId, string userId, bool isPassed)
        {
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }


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
            // if you throw exception that is not handled would not this cause
            // the whole program to crash? Handle with Try/Catch block?
            // redirect to 302?

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User not found!");
            }

            var result = this.userTestRepo.All
                .Where(ut => ut.UserId == userId && ut.IsSubmited == null)
                .Include(ut => ut.Test)
                .FirstOrDefault();

            var userTestDto = this.mapper.MapTo<UserTestDto>(result);
            return userTestDto;

        }
    }
}
