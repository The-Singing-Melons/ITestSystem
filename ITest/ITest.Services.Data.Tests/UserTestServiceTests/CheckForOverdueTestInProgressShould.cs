using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.Infrastructure.Providers;
using ITest.Infrastructure.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ITest.Services.Data.Tests.UserTestServiceTests
{
    [TestClass]
    public class CheckForOverdueTestInProgressShould
    {
        [TestMethod]
        public void Return_False_When_NoTestsInProggress_AreFound()
        {
            // Arrange
            string fakeUserId = "userId";

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, categoryRepoStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            // Act
            var result = userTestService.CheckForOverdueTestInProgress(fakeUserId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Throw_ArgumentNullException_WhenUserId_IsNullOrEmpty()
        {
            // Arrange
            string fakeUserId = "";

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, categoryRepoStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            Action executingAddUserToTestMethod = () => userTestService.CheckForOverdueTestInProgress(fakeUserId);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(executingAddUserToTestMethod);
        }
    }
}
