using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.Infrastructure.Providers;
using ITest.Infrastructure.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace ITest.Services.Data.Tests.UserTestServiceTests
{
    [TestClass]
    public class GetUserTestResultsShould
    {
        [TestMethod]
        public void Invoke_MappingProvider_ProjectTo_With_ResultsFrom_UserTestRepo()
        {
            // Arrange
            var fakeResults = new List<UserTest>() { }.AsQueryable();

            var testRepoStub = new Mock<IDataRepository<Test>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mockMappingProvider = new Mock<IMappingProvider>();
            var categoryRepoStub = new Mock<IDataRepository<Category>>();
            var userTestRepoStub = new Mock<IDataRepository<UserTest>>();
            var timeProviderStub = new Mock<TimeProvider>();

            var userTestService = new UserTestService(testRepoStub.Object, dataSaverStub.Object, mockMappingProvider.Object, categoryRepoStub.Object, userTestRepoStub.Object, timeProviderStub.Object);

            userTestRepoStub.Setup(utr => utr.All).Returns(fakeResults);

            // Act
            userTestService.GetUserTestResults();

            // Assert
            mockMappingProvider.Verify(mp => mp.ProjectTo<UserTestResultDto>(fakeResults), Times.Once);
        }
    }
}
