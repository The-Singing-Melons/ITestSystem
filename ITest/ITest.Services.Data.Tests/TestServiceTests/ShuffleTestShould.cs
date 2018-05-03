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

        //[TestMethod]
        // how exactly to assert model has changed state?
        public void ChangeParameterState_WhenCalledWithValidArgument()
        {
            // Arrange
            var testId = new Guid().ToString();

            var testCategory = new CategoryDto()
            {
                Name = "JAVA"
            };

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
                Id = testId,
                IsPublished = true,
                Category = testCategory,
                Questions = testQuestionsDto
            };

            var testsDomainDto = new List<TestDto>() { testDto };

            var sut = new TestService(userRepoMock.Object, testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object);

        }
    }
}