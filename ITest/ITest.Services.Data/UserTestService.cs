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
using ITest.Infrastructure.Providers;

namespace ITest.Services.Data
{
    public class UserTestService : IUserTestService
    {
        private readonly IDataRepository<Test> testRepo;
        private readonly IDataRepository<UserTest> userTestRepo;
        private readonly TimeProvider time;
        private readonly IDataSaver dataSaver;
        private readonly IMappingProvider mapper;

        public UserTestService(
            IDataRepository<Test> testRepo, IDataSaver dataSaver,
            IMappingProvider mapper, IDataRepository<UserTest> userTestRepo, TimeProvider time)
        {
            this.testRepo = testRepo ?? throw new ArgumentNullException(nameof(testRepo));
            this.userTestRepo = userTestRepo ?? throw new ArgumentNullException(nameof(userTestRepo));
            this.time = time ?? throw new ArgumentNullException(nameof(time));
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
                StartedOn = this.time.Now
            };

            this.userTestRepo.Add(userToTestObject);
            this.dataSaver.SaveChanges();
        }

        public double GetTimeRemainingForUserTest(string userId, string testId, double testDuration)
        {
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

            var currentUserTest = this.userTestRepo.All
                .Where(x => x.UserId == userId && x.TestId.ToString() == testId)
                .FirstOrDefault();

            if (currentUserTest == null)
            {
                throw new ArgumentNullException("currentUserTest not found!");
            }

            var endTime = currentUserTest.StartedOn.AddMinutes(testDuration);
            var timeRemaining = Math.Round((endTime - this.time.Now).TotalSeconds);

            return timeRemaining;

        }

        public bool UserHasCompletedTest(string userId, string testId)
        {
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

            var isTestTaken = this.userTestRepo.All
                .Any(x => x.UserId == userId &&
                          x.TestId.ToString() == testId &&
                          x.IsSubmited == true);

            return isTestTaken;
        }

        public void SubmitUserTest(string testId, string userId, bool isPassed)
        {
            // add check if the user has manipulated the javascript clock time and submited late
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
            var testExecutionTime = currentTest.StartedOn - this.time.Now;
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

        public bool CheckForOverdueTestInProgress(string userId)
        {
            var isOverdue = false;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User not found!");
            }

            var result = this.userTestRepo.All
                .Where(ut => ut.UserId == userId && ut.IsSubmited == null)
                .Include(ut => ut.Test)
                .FirstOrDefault();

            if (result != null)
            {
                isOverdue = this.CheckForOverdueTestInProgress(result, isOverdue);
            }

            return isOverdue;
        }

        private bool CheckForOverdueTestInProgress(UserTest result, bool isOverdue)
        {
            var endTime = result.StartedOn
                  .AddMinutes(result.Test.Duration);

            var timeRemaining = Math.Round((endTime - this.time.Now).TotalSeconds);

            if (timeRemaining == 0)
            {
                this.SubmitUserTest(result.Test.Id.ToString(), result.UserId, false);
                return true;
            }

            return false;
        }
    }
}
