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
    public class AddUserToTestShould
    {
        [TestMethod]
        public void Invoke_AddMethod_UserTestRepo()
        {
            // Arrange
            string fakeTestId = new Guid().ToString();
            string fakeUserId = "userId";

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var mockUserTestRepo = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, categoryRepoStub.Object, mockUserTestRepo.Object, timeProviderStub.Object);

            // Act
            userTestService.AddUserToTest(fakeTestId, fakeUserId);

            // Assert
            mockUserTestRepo.Verify(utr => utr.Add(It.IsAny<UserTest>()), Times.Once);
        }

        [TestMethod]
        public void Invoke_SaveChangesMethod_DataSaver()
        {
            // Arrange
            string fakeTestId = new Guid().ToString();
            string fakeUserId = "userId";

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var mockDataSaver = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, mockDataSaver.Object, mappingProviderStub.Object, categoryRepoStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            // Act
            userTestService.AddUserToTest(fakeTestId, fakeUserId);

            // Assert
            mockDataSaver.Verify(ds => ds.SaveChanges(), Times.Once);
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

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, categoryRepoStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            Action executingAddUserToTestMethod = () => userTestService.AddUserToTest(fakeTestId, fakeUserId);

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

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, categoryRepoStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            Action executingAddUserToTestMethod = () => userTestService.AddUserToTest(fakeTestId, fakeUserId);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(executingAddUserToTestMethod);
        }
    }
}
