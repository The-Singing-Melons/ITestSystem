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
    public class UserHasCompletedTestShould
    {
        [TestMethod]
        public void Return_True_WhenAttendedTest_IsSubmitted()
        {
            // Arrange
            var fakeTestId = new Guid();
            string fakeUserId = "userId";

            var fakeUserTest = new UserTest()
            {
                UserId = fakeUserId,
                TestId = fakeTestId,
                IsSubmited = true
            };

            var fakeUserTests = new List<UserTest>() { fakeUserTest }.AsQueryable();

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            userTestRepoStub.SetupGet(utr => utr.All).Returns(fakeUserTests);

            // Act
            var result = userTestService.UserHasCompletedTest(fakeUserId, fakeTestId.ToString());

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Return_False_WhenAttendedTest_IsNotSubmitted()
        {
            // Arrange
            var fakeTestId = new Guid();
            string fakeUserId = "userId";

            var fakeUserTest = new UserTest()
            {
                UserId = fakeUserId,
                TestId = fakeTestId,
                IsSubmited = false
            };

            var fakeUserTests = new List<UserTest>() { fakeUserTest }.AsQueryable();

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            userTestRepoStub.SetupGet(utr => utr.All).Returns(fakeUserTests);

            // Act
            var result = userTestService.UserHasCompletedTest(fakeUserId, fakeTestId.ToString());

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Throw_ArgumentNullException_WhenUserId_IsNullOrEmpty()
        {
            // Arrange
            string fakeTestId = new Guid().ToString();
            string fakeUserId = "";

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            Action executingAddUserToTestMethod = () => userTestService.UserHasCompletedTest(fakeUserId, fakeTestId);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(executingAddUserToTestMethod);
        }

        [TestMethod]
        public void Throw_ArgumentNullException_WhenTestId_IsNullOrEmpty()
        {
            // Arrange
            string fakeTestId = "";
            string fakeUserId = "userId";

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            Action executingAddUserToTestMethod = () => userTestService.UserHasCompletedTest(fakeUserId, fakeTestId);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(executingAddUserToTestMethod);
        }
    }
}
