using System;
using System.Collections.Generic;
using System.Linq;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
namespace ITest.Services.Data.Tests.TestServiceTests
{
    [TestClass]
    public class GetTestByIdShould
    {
        private Mock<IDataRepository<ApplicationUser>> userRepoMock;
        private Mock<IDataRepository<Test>> testRepoMock;
        private Mock<IDataRepository<Question>> questionRepoMock;
        private Mock<IDataRepository<Answer>> answerRepoMock;
        private Mock<IDataRepository<Category>> categoryRepoMock;
        private Mock<IDataSaver> dataSaverMock;
        private Mock<IMappingProvider> mapperMock;
        private Mock<IRandomProvider> randomMock;
        private Mock<IShuffleProvider> shufflerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            this.userRepoMock = new Mock<IDataRepository<ApplicationUser>>();
            this.testRepoMock = new Mock<IDataRepository<Test>>();
            this.questionRepoMock = new Mock<IDataRepository<Question>>();
            this.answerRepoMock = new Mock<IDataRepository<Answer>>();
            this.categoryRepoMock = new Mock<IDataRepository<Category>>();
            this.dataSaverMock = new Mock<IDataSaver>();
            this.mapperMock = new Mock<IMappingProvider>();
            this.randomMock = new Mock<IRandomProvider>();
            this.shufflerMock = new Mock<IShuffleProvider>();
        }

        [TestMethod]
        public void ReturnCorrectData_WhenInvokedWithValidParameters()
        {
            // Arrange
            var testId = new Guid();
            var testName = "Random test";
            var testsDomain = new List<Test>()
            {
                new Test()
                {
                    Id = testId,
                    Name = testName
                }
            };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testsDomain.AsQueryable());

            var testDtoToReturn = new TestDto() { Id = testId.ToString(), Name = testName };

            this.mapperMock.Setup(x => x.MapTo<TestDto>(It.IsAny<Test>()))
                .Returns(testDtoToReturn);

            // Act
            var sut = new TestService(userRepoMock.Object, testRepoMock.Object, questionRepoMock.Object,
                answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
                randomMock.Object, shufflerMock.Object);

            var actualResult = sut.GetTestById(testId.ToString());

            // Assert
            Assert.AreEqual(actualResult.Id, testDtoToReturn.Id);
        }
    }
}
