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
    public class ShuffleTestShould
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
        public void CallShufflerShuffleQuestions_WithCorrectParameter()
        {
            // Arrange
            var questionsDto = new List<QuestionDto>();

            var testDto = new TestDto()
            {
                Questions = questionsDto
            };


            this.shufflerMock.Setup(x => x.Shuffle<QuestionDto>(It.IsAny<List<QuestionDto>>()))
                    .Returns(questionsDto);

            var sut = new TestService(userRepoMock.Object, testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object);

            // act
            sut.ShuffleTest(testDto);

            // Assert
            this.shufflerMock.Verify(x => x.Shuffle<QuestionDto>(testDto.Questions), Times.Once);
        }

        [TestMethod]
        public void CallShufflerShuffleAnswers_WithCorrectParameter()
        {
            // Arrange
            var answersDto = new List<AnswerDto>();
            var questionsDto = new List<QuestionDto>()
            {
                new QuestionDto()
                {
                    Answers = answersDto
                }
            };

            var testDto = new TestDto()
            {
                Questions = questionsDto
            };

            this.shufflerMock.Setup(x => x.Shuffle<QuestionDto>(It.IsAny<List<QuestionDto>>()))
                 .Returns(questionsDto);

            this.shufflerMock.Setup(x => x.Shuffle<AnswerDto>(It.IsAny<List<AnswerDto>>()))
                    .Returns(answersDto);

            var sut = new TestService(userRepoMock.Object, testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object);

            // act
            sut.ShuffleTest(testDto);

            // Assert
            this.shufflerMock.Verify(x => x.Shuffle<AnswerDto>(testDto.Questions[0].Answers), Times.Once);
        }

        [TestMethod]
        public void ChangeTestAnswerState_WhenCalledWithValidArgument()
        {
            // Arrange
            var testQuestionAnswersDto = new List<AnswerDto>()
            {
                new AnswerDto()
            };

            var testQuestionsDto = new List<QuestionDto>()
            {
                new QuestionDto() { Answers  = testQuestionAnswersDto}
            };

            var testDto = new TestDto()
            {
                Questions = testQuestionsDto
            };

            var testsDomainDto = new List<TestDto>() { testDto };

            this.shufflerMock.Setup(x => x.Shuffle<QuestionDto>(It.IsAny<List<QuestionDto>>()))
                .Returns(testQuestionsDto);

            this.shufflerMock.Setup(x => x.Shuffle<AnswerDto>(It.IsAny<List<AnswerDto>>()))
                    .Returns(new List<AnswerDto>());

            var sut = new TestService(userRepoMock.Object, testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object);

            // Act
            sut.ShuffleTest(testDto);

            // Assert
            Assert.AreNotEqual(testQuestionAnswersDto, testDto.Questions[0].Answers);
        }

        [TestMethod]
        public void ChangeTestQuestionState_WhenCalledWithValidArgument()
        {
            // Arrange
            var testQuestionsDto = new List<QuestionDto>();

            var testDto = new TestDto()
            {
                Questions = testQuestionsDto
            };

            var testsDomainDto = new List<TestDto>() { testDto };

            this.shufflerMock.Setup(x => x.Shuffle<QuestionDto>(It.IsAny<List<QuestionDto>>()))
                .Returns(new List<QuestionDto>());

            var sut = new TestService(userRepoMock.Object, testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object);

            // Act
            sut.ShuffleTest(testDto);

            // Assert
            Assert.AreNotEqual(testQuestionsDto, testDto.Questions);
        }

        [TestMethod]
        public void ThrowArgumentNull_WhenCalledWithInvalidParameter()
        {
            // Arrange
            var sut = new TestService(userRepoMock.Object, testRepoMock.Object, questionRepoMock.Object,
            answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
            randomMock.Object, shufflerMock.Object);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => sut.ShuffleTest(null));
        }

    }
}