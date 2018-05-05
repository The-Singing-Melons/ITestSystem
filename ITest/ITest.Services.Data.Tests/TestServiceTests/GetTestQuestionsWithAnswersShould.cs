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
    public class GetTestQuestionsWithAnswerShould
    {
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
        public void ReturnCorrectData_WhenCalledWithValidParameters()
        {
            // Arrange
            var testId = new Guid();

            var testCategory = new Category()
            {
                Name = "JAVA"
            };

            var testQuestionAnswers = new List<Answer>()
            {
                new Answer()
            };

            var testQuestions = new List<Question>()
            {
                new Question() { Answers  = testQuestionAnswers}
            };

            var test = new Test()
            {
                Id = testId,
                IsPublished = true,
                Category = testCategory,
                Questions = testQuestions
            };

            var testsDomain = new List<Test>() { test };

            var testDtoToReturn = new TestDto();

            this.testRepoMock.Setup(x => x.All)
                .Returns(testsDomain.AsQueryable());

            this.mapperMock.Setup(x => x.MapTo<TestDto>(It.IsAny<Test>()))
                .Returns(testDtoToReturn);

            //this.mapperMock.Verify(x => x.MapTo<TestDto>(test), Times.Once);

            var sut = new TestService(testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object);

            // Act

            var result = sut.GetTestQuestionsWithAnswers(testId.ToString());

            // Assert - is this correct?
            Assert.AreEqual(result, testDtoToReturn);
        }

        [TestMethod]
        public void CallMapperMapTo_WithCorrectQueryParameter()
        {
            // Arrange
            var testId = new Guid();

            var testCategory = new Category()
            {
                Name = "JAVA"
            };

            var testQuestionAnswers = new List<Answer>()
            {
                new Answer()
            };

            var testQuestions = new List<Question>()
            {
                new Question() { Answers  = testQuestionAnswers}
            };

            var test = new Test()
            {
                Id = testId,
                IsPublished = true,
                Category = testCategory,
                Questions = testQuestions
            };

            var testsDomain = new List<Test>() { test };

            var testDtoToReturn = new TestDto();

            this.testRepoMock.Setup(x => x.All)
                .Returns(testsDomain.AsQueryable());

            this.mapperMock.Setup(x => x.MapTo<TestDto>(It.IsAny<Test>()))
                .Returns(testDtoToReturn);

            var sut = new TestService(testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object);

            // Act
            var result = sut.GetTestQuestionsWithAnswers(testId.ToString());

            // Assert
            this.mapperMock.Verify(x => x.MapTo<TestDto>(test), Times.Once);
        }


        [TestMethod]
        public void Throw_WhenCalledWithInvalidParameter()
        {
            // Arrange 
            var sut = new TestService(testRepoMock.Object, questionRepoMock.Object,
             answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
             randomMock.Object, shufflerMock.Object);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => sut.GetTestQuestionsWithAnswers(null));
        }

        [TestMethod]
        public void Throw_WhenTestNotFoundInDatabase()
        {
            // Arrange
            var testId = new Guid();

            var testCategory = new Category()
            {
                Name = "JAVA"
            };

            var testQuestionAnswers = new List<Answer>()
            {
                new Answer()
            };

            var testQuestions = new List<Question>()
            {
                new Question() { Answers  = testQuestionAnswers}
            };

            var test = new Test()
            {
                Id = testId,
                IsPublished = true,
                Category = testCategory,
                Questions = testQuestions
            };

            var testsDomain = new List<Test>() { test };

            var testDtoToReturn = new TestDto();

            this.testRepoMock.Setup(x => x.All)
                .Returns(testsDomain.AsQueryable());

            this.mapperMock.Setup(x => x.MapTo<TestDto>(It.IsAny<Test>()))
                .Returns(testDtoToReturn);

            //this.mapperMock.Verify(x => x.MapTo<TestDto>(test), Times.Once);

            var sut = new TestService(testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => sut.GetTestQuestionsWithAnswers("invalid id"));
        }
    }
}