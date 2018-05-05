using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.Infrastructure.Providers;
using ITest.Infrastructure.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITest.Services.Data.Tests.UserTestServiceTests
{
    [TestClass]
    public class GetTimeRemainingForUserTestShould
    {
        [TestMethod]
        public void Return_CorrectTimeRemaining()
        {
            // Arrange
            var fakeTestId = new Guid();
            string fakeUserId = "userId";
            double fakeTestDuration = 30d;
            var fakeStartDateTime = new DateTime(2017, 5, 10, 12, 0, 0);
            var fakeDateTimeNow = new DateTime(2017, 5, 10, 12, 10, 0);

            var fakeUserTest = new UserTest()
            {
                UserId = fakeUserId,
                TestId = fakeTestId,
                StartedOn = fakeStartDateTime
            };
            var fakeUserTests = new List<UserTest>() { fakeUserTest }.AsQueryable();

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, categoryRepoStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            userTestRepoStub.SetupGet(utr => utr.All).Returns(fakeUserTests);
            timeProviderStub.SetupGet(tp => tp.Now).Returns(fakeDateTimeNow);

            var endTime = fakeUserTest.StartedOn.AddMinutes(fakeTestDuration);
            var expectedRemainingTime = Math.Round((endTime - fakeDateTimeNow).TotalSeconds);

            // Act
            var actualRemainingTime = userTestService.GetTimeRemainingForUserTest(fakeUserId, fakeTestId.ToString(), fakeTestDuration);

            // Assert
            Assert.AreEqual(expectedRemainingTime, actualRemainingTime);
        }

        [TestMethod]
        public void Throw_ArgumentNullException_When_UserTestIsNotFound()
        {
            // Arrange
            var fakeTestId = new Guid();
            string fakeUserId = "userId";
            double fakeTestDuration = 30d;

            var fakeUserTests = new List<UserTest>() { }.AsQueryable();

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, categoryRepoStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            userTestRepoStub.SetupGet(utr => utr.All).Returns(fakeUserTests);

            Action executingGetTimeRemainingForUserTest = () => userTestService.GetTimeRemainingForUserTest(fakeUserId, fakeTestId.ToString(), fakeTestDuration);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(executingGetTimeRemainingForUserTest);
        }
        
        [TestMethod]
        public void Throw_ArgumentNullException_WhenUserId_IsNullOrEmpty()
        {
            // Arrange
            var fakeTestId = new Guid();
            string fakeUserId = "";
            double fakeTestDuration = 30d;
            
            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, categoryRepoStub.Object, userTestRepoStub.Object, timeProviderStub.Object);
            
            Action executingGetTimeRemainingForUserTest = () => userTestService.GetTimeRemainingForUserTest(fakeUserId, fakeTestId.ToString(), fakeTestDuration);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(executingGetTimeRemainingForUserTest);
        }

        [TestMethod]
        public void Throw_ArgumentNullException_WhenTestId_IsNullOrEmpty()
        {
            // Arrange
            var fakeTestId = "";
            string fakeUserId = "userId";
            double fakeTestDuration = 30d;

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, categoryRepoStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            Action executingGetTimeRemainingForUserTest = () => userTestService.GetTimeRemainingForUserTest(fakeUserId, fakeTestId, fakeTestDuration);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(executingGetTimeRemainingForUserTest);
        }
    }
}
