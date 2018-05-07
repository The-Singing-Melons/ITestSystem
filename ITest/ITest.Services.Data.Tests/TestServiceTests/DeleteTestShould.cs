using System;
using System.Collections.Generic;
using System.Linq;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.Infrastructure.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Caching.Memory;

namespace ITest.Services.Data.Tests.TestServiceTests
{
    [TestClass]
    public class DeleteTestShould
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
        public void CallDataSaverSaveChanges_WhenInvokedWithValidParameters()
        {
            // Arrange
            var testId = new Guid();
            var testName = "test";
            var category = new Category() { Name = "JAVA" };

            var questionAnswers = new List<Answer>()
            {
                new Answer()
                {
                    IsDeleted = false
                }
            };

            var testQuestions = new List<Question>()
            {
                new Question()
                {
                    Answers = questionAnswers,
                    IsDeleted = false
                }
            };

            var testStub = new Test()
            {
                Name = testName,
                Id = testId,
                Questions = testQuestions,
                Category = category
            };

            var testList = new List<Test>() { testStub };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testList.AsQueryable());

            var sut = new TestService(
              testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
              mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);


            // Act
            sut.DeleteTest(testId.ToString());

            // Assert
            this.dataSaverMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void ThrowArgumentNullException_WhenCalledWithInvalidParameters()
        {
            // Arrange
            var sut = new TestService(testRepoMock.Object, questionRepoMock.Object,
             answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
             randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => sut.DeleteTest(null));
        }

        [TestMethod]
        public void CallTestRepoDeleteWithCorrectParameter_WhenInvokedWithValidParameters()
        {
            // Arrange
            var testId = new Guid();
            var testName = "test";
            var category = new Category() { Name = "JAVA" };

            var questionAnswers = new List<Answer>()
            {
                new Answer()
                {
                    IsDeleted = false
                }
            };

            var testQuestions = new List<Question>()
            {
                new Question()
                {
                    Answers = questionAnswers,
                    IsDeleted = false
                }
            };

            var testStub = new Test()
            {
                Name = testName,
                Id = testId,
                Questions = testQuestions,
                Category = category
            };

            var testList = new List<Test>() { testStub };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testList.AsQueryable());

            var sut = new TestService(
              testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
              mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);


            // Act
            sut.DeleteTest(testId.ToString());

            // Assert
            this.testRepoMock.Verify(x => x.Delete(testStub), Times.Once);
        }

        [TestMethod]
        public void CallQuestionRepoDeleteWithCorrectParameter_WhenInvokedWithValidParameters()
        {
            // Arrange
            var testId = new Guid();
            var testName = "test";
            var category = new Category() { Name = "JAVA" };

            var questionAnswers = new List<Answer>()
            {
                new Answer()
                {
                    IsDeleted = false
                }
            };

            var testQuestions = new List<Question>()
            {
                new Question()
                {
                    Answers = questionAnswers,
                    IsDeleted = false
                }
            };

            var testStub = new Test()
            {
                Name = testName,
                Id = testId,
                Questions = testQuestions,
                Category = category
            };

            var testList = new List<Test>() { testStub };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testList.AsQueryable());

            var sut = new TestService(
              testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
              mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);


            // Act
            sut.DeleteTest(testId.ToString());

            // Assert
            this.questionRepoMock.Verify(x => x.Delete(testQuestions[0]), Times.Once);
        }


        [TestMethod]
        public void CallAnswerRepoDeleteWithCorrectParameter_WhenInvokedWithValidParameters()
        {
            // Arrange
            var testId = new Guid();
            var testName = "test";
            var category = new Category() { Name = "JAVA" };

            var questionAnswers = new List<Answer>()
            {
                new Answer()
                {
                    IsDeleted = false
                }
            };

            var testQuestions = new List<Question>()
            {
                new Question()
                {
                    Answers = questionAnswers,
                    IsDeleted = false
                }
            };

            var testStub = new Test()
            {
                Name = testName,
                Id = testId,
                Questions = testQuestions,
                Category = category
            };

            var testList = new List<Test>() { testStub };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testList.AsQueryable());

            var sut = new TestService(
              testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
              mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);


            // Act
            sut.DeleteTest(testId.ToString());

            // Assert
            this.answerRepoMock.Verify(x => x.Delete(questionAnswers[0]), Times.Once);
        }
    }
}