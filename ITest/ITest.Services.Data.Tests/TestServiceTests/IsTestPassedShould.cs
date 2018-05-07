using System;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.DTO.TakeTest;
using ITest.Infrastructure.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Caching.Memory;

namespace ITest.Services.Data.Tests.TestServiceTests
{
    [TestClass]
    public class IsTestPassedShould
    {
        private Mock<IDataRepository<Test>> testRepoMock;
        private Mock<IDataRepository<Question>> questionRepoMock;
        private Mock<IDataRepository<Answer>> answerRepoMock;
        private Mock<IDataRepository<Category>> categoryRepoMock;
        private Mock<IDataSaver> dataSaverMock;
        private Mock<IMappingProvider> mapperMock;
        private Mock<IRandomProvider> randomMock;
        private Mock<IShuffleProvider> shufflerMock;
        private Mock<IMemoryCache> memoryCacheMock;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testRepoMock = new Mock<IDataRepository<Test>>();
            this.questionRepoMock = new Mock<IDataRepository<Question>>();
            this.answerRepoMock = new Mock<IDataRepository<Answer>>();
            this.categoryRepoMock = new Mock<IDataRepository<Category>>();
            this.dataSaverMock = new Mock<IDataSaver>();
            this.mapperMock = new Mock<IMappingProvider>();
            this.randomMock = new Mock<IRandomProvider>();
            this.shufflerMock = new Mock<IShuffleProvider>();
            this.memoryCacheMock = new Mock<IMemoryCache>();
        }

        [TestMethod]
        public void ThrowArgumentNull_WhenCalledWithInvalidFirstParameter()
        {
            var sut = new TestService(
            testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
            mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);

            Assert.ThrowsException<ArgumentNullException>(()
                                        => sut
                                        .IsTestPassed(null, new TestRequestDto(), new TestDto()));
        }

        [TestMethod]
        public void ThrowArgumentNull_WhenCalledWithInvalidSecondParameter()
        {
            var sut = new TestService(
            testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
            mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);

            Assert.ThrowsException<ArgumentNullException>(()
                                         => sut
                                         .IsTestPassed(null, new TestRequestDto(), new TestDto()));
        }
    }
}