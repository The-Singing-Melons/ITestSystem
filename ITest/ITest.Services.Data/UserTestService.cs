﻿using System;
using System.Linq;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Models;
using ITest.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;

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

        public void AddUserToTest(string testId, string userId, bool isPassed)
        {
            var userToTestObject = new UserTest
            {
                TestId = Guid.Parse(testId),
                UserId = userId,
                IsPassed = isPassed,
                IsSubmited = true,
                IsDeleted = false,
            };
            this.userTestRepo.Add(userToTestObject);
            this.dataSaver.SaveChanges();
        }


        public bool CheckFoCompletedUserTestInCategory(string userId, string categoryName)
        {
            // TO-DO : Do this fat query only once 
            var testsTakenByUser = this.userTestRepo.All
                        .Where(x => x.UserId.ToString() == userId)
                        .Include(t => t.Test)
                        .ThenInclude(t => t.Category)
                        .ToList();

            var isTestTaken = testsTakenByUser
                .Any(x => x.Test.Category.Name == categoryName);

            return isTestTaken;
        }
    }
}
