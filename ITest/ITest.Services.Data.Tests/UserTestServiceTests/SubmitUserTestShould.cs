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
    public class SubmitUserTestShould
    {
        [TestMethod]
        public void Invoke_SaveChanges_DataSaver()
        {
            // Arrange
            var fakeTestId = new Guid();
            string fakeUserId = "userId";
            bool fakeIsPassed = true;

            var fakeUserTest = new UserTest()
            {
                UserId = fakeUserId,
                TestId = fakeTestId
            };
            var fakeUserTests = new List<UserTest>() { fakeUserTest }.AsQueryable();

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var mockDataSaver = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, mockDataSaver.Object, mappingProviderStub.Object, categoryRepoStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            userTestRepoStub.SetupGet(utr => utr.All).Returns(fakeUserTests);

            // Act
            userTestService.SubmitUserTest(fakeTestId.ToString(), fakeUserId, fakeIsPassed);

            // Assert
            mockDataSaver.Verify(ds => ds.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Assign_CorrectData_ToUserTest()
        {
            // Arrange
            var fakeTestId = new Guid();
            string fakeUserId = "userId";
            var fakeStartedDate = new DateTime(2017, 5, 10, 12, 0, 0);
            var fakeDateTimeNow = new DateTime(2017, 5, 10, 12, 10, 0);

            var executionTimeSpan = fakeStartedDate - fakeDateTimeNow;
            var expectedExecutionTime = Math.Abs(executionTimeSpan.TotalMinutes);

            bool expectedIsPassed = true;

            var fakeUserTest = new UserTest()
            {
                UserId = fakeUserId,
                TestId = fakeTestId,
                StartedOn = fakeStartedDate
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

            // Act
            userTestService.SubmitUserTest(fakeTestId.ToString(), fakeUserId, expectedIsPassed);

            // Assert
            var actualIsPassed = fakeUserTest.IsPassed;
            var actualIsSubmited = fakeUserTest.IsSubmited;
            var actualExecutionTime = fakeUserTest.ExecutionTime;

            Assert.AreEqual(expectedIsPassed, actualIsPassed);
            Assert.IsTrue(actualIsSubmited.Value);
            Assert.AreEqual(expectedExecutionTime, actualExecutionTime);
        }
    }
}
