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
    public class EditTestShould
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
        public void ThrowArgumentNullException_WhenCalledWithInvalidParameter()
        {
            var sut = new TestService(
               testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
               mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object);

            Assert.ThrowsException<ArgumentNullException>(() => sut.EditTest(null));
        }

        [TestMethod]
        public void SucessfullyCallDataSaverSaveChanges_WhenInvokedWithValidParameters()
        {
            // Arrange
            // Domain
            var testId = new Guid();
            var questionId = new Guid();
            var answerId = new Guid();
            var category = new Category() { Name = "JAVA" };
            var questionAnswers = new List<Answer>()
            {
                new Answer()
                {
                    Id = answerId,
                    IsDeleted = false
                }
            };

            var testQuestions = new List<Question>()
            {
                new Question()
                {
                    Id= questionId,
                    Answers = questionAnswers,
                    IsDeleted = false
                }
            };

            var testStub = new Test()
            {
                Id = testId,
                Questions = testQuestions,
                Category = category
            };

            var testList = new List<Test>() { testStub };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testList.AsQueryable());

            this.categoryRepoMock.Setup(x => x.All)
                .Returns(new List<Category>() { category }.AsQueryable());

            // DTO

            var dtoQuestion = new ManageQuestionDto() { Id = questionId.ToString() };
            var dtoAnswer = new ManageAnswerDto() { Id = answerId.ToString() };
            var dtoAnswersList = new List<ManageAnswerDto>() { dtoAnswer };
            dtoQuestion.Answers = dtoAnswersList;
            var dtoQuestionsList = new List<ManageQuestionDto>() { dtoQuestion };

            var testDtoArgument = new ManageTestDto()
            {
                Id = testId.ToString(),
                CategoryName = category.Name,
                Questions = dtoQuestionsList
            };

            var sut = new TestService(
              testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
              mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object);

            // Act

            sut.EditTest(testDtoArgument);

            // Assert

            this.dataSaverMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void SucessfullyCallQuestionRepoAdd_WhenInvokedWithValidParameters()
        {
            // Arrange
            // Domain
            var testId = new Guid();
            var questionId = new Guid();
            var answerId = new Guid();
            var category = new Category() { Name = "JAVA" };
            var questionAnswers = new List<Answer>()
            {
                new Answer()
                {
                    Id = answerId,
                    IsDeleted = false
                }
            };

            var testQuestions = new List<Question>()
            {
                new Question()
                {
                    Id= questionId,
                    Answers = questionAnswers,
                    IsDeleted = false
                }
            };

            var testStub = new Test()
            {
                Id = testId,
                Questions = testQuestions,
                Category = category
            };

            var testList = new List<Test>() { testStub };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testList.AsQueryable());

            this.categoryRepoMock.Setup(x => x.All)
                .Returns(new List<Category>() { category }.AsQueryable());

            // DTO
            //var questiontoAdd = this.mapper.MapTo<Question>(updatedQuestion);

            var guidString = "e376ce00-89b0-44f3-bd1c-a248535585b9";
            var questionDomainToReturn = new Question() { Id = Guid.Parse(guidString) };
            this.mapperMock.Setup(x => x.MapTo<Question>(It.IsAny<ManageQuestionDto>()))
                .Returns(questionDomainToReturn);
            var dtoQuestion = new ManageQuestionDto() { Id = guidString };
            var dtoAnswer = new ManageAnswerDto() { Id = answerId.ToString() };
            var dtoAnswersList = new List<ManageAnswerDto>() { dtoAnswer };
            dtoQuestion.Answers = dtoAnswersList;
            var dtoQuestionsList = new List<ManageQuestionDto>() { dtoQuestion };

            var testDtoArgument = new ManageTestDto()
            {
                Id = testId.ToString(),
                CategoryName = category.Name,
                Questions = dtoQuestionsList
            };

            var sut = new TestService(
              testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
              mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object);

            // Act

            sut.EditTest(testDtoArgument);

            // Assert

            this.questionRepoMock.Verify(x => x.Add(questionDomainToReturn), Times.Once);
        }

        [TestMethod]
        public void SucessfullyCallTestRepoUpdate_WhenInvokedWithValidParameters()
        {
            // Arrange
            // Domain
            var testId = new Guid();
            var questionId = new Guid();
            var answerId = new Guid();
            var category = new Category() { Name = "JAVA" };
            var questionAnswers = new List<Answer>()
            {
                new Answer()
                {
                    Id = answerId,
                    IsDeleted = false
                }
            };

            var testQuestions = new List<Question>()
            {
                new Question()
                {
                    Id= questionId,
                    Answers = questionAnswers,
                    IsDeleted = false
                }
            };

            var testStub = new Test()
            {
                Id = testId,
                Questions = testQuestions,
                Category = category
            };

            var testList = new List<Test>() { testStub };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testList.AsQueryable());

            this.categoryRepoMock.Setup(x => x.All)
                .Returns(new List<Category>() { category }.AsQueryable());

            // DTO

            var dtoQuestion = new ManageQuestionDto() { Id = questionId.ToString() };
            var dtoAnswer = new ManageAnswerDto() { Id = answerId.ToString() };
            var dtoAnswersList = new List<ManageAnswerDto>() { dtoAnswer };
            dtoQuestion.Answers = dtoAnswersList;
            var dtoQuestionsList = new List<ManageQuestionDto>() { dtoQuestion };

            var testDtoArgument = new ManageTestDto()
            {
                Id = testId.ToString(),
                CategoryName = category.Name,
                Questions = dtoQuestionsList
            };

            var sut = new TestService(
              testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
              mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object);

            // Act

            sut.EditTest(testDtoArgument);

            // Assert

            this.testRepoMock.Verify(x => x.Update(testStub), Times.Once);
        }

        [TestMethod]
        public void SucessfullyCallQuestionRepoUpdate_WhenInvokedWithValidParameters()
        {
            // Arrange
            // Domain
            var testId = new Guid();
            var questionId = new Guid();
            var answerId = new Guid();
            var category = new Category() { Name = "JAVA" };
            var questionAnswers = new List<Answer>()
            {
                new Answer()
                {
                    Id = answerId,
                    IsDeleted = false
                }
            };

            var testQuestions = new List<Question>()
            {
                new Question()
                {
                    Id= questionId,
                    Answers = questionAnswers,
                    IsDeleted = false
                }
            };

            var testStub = new Test()
            {
                Id = testId,
                Questions = testQuestions,
                Category = category
            };

            var testList = new List<Test>() { testStub };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testList.AsQueryable());

            this.categoryRepoMock.Setup(x => x.All)
                .Returns(new List<Category>() { category }.AsQueryable());

            // DTO

            var dtoQuestion = new ManageQuestionDto() { Id = questionId.ToString() };
            var dtoAnswer = new ManageAnswerDto() { Id = answerId.ToString() };
            var dtoAnswersList = new List<ManageAnswerDto>() { dtoAnswer };
            dtoQuestion.Answers = dtoAnswersList;
            var dtoQuestionsList = new List<ManageQuestionDto>() { dtoQuestion };

            var testDtoArgument = new ManageTestDto()
            {
                Id = testId.ToString(),
                CategoryName = category.Name,
                Questions = dtoQuestionsList
            };

            var sut = new TestService(
              testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
              mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object);

            // Act

            sut.EditTest(testDtoArgument);

            // Assert

            this.questionRepoMock.Verify(x => x.Update(testQuestions[0]), Times.Once);
        }

        [TestMethod]
        public void SucessfullyCallAnswerRepoUpdate_WhenInvokedWithValidParameters()
        {
            // Arrange
            // Domain
            var testId = new Guid();
            var questionId = new Guid();
            var answerId = new Guid();
            var category = new Category() { Name = "JAVA" };
            var questionAnswers = new List<Answer>()
            {
                new Answer()
                {
                    Id = answerId,
                    IsDeleted = false
                }
            };

            var testQuestions = new List<Question>()
            {
                new Question()
                {
                    Id= questionId,
                    Answers = questionAnswers,
                    IsDeleted = false
                }
            };

            var testStub = new Test()
            {
                Id = testId,
                Questions = testQuestions,
                Category = category
            };

            var testList = new List<Test>() { testStub };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testList.AsQueryable());

            this.categoryRepoMock.Setup(x => x.All)
                .Returns(new List<Category>() { category }.AsQueryable());

            // DTO

            var dtoQuestion = new ManageQuestionDto() { Id = questionId.ToString() };
            var dtoAnswer = new ManageAnswerDto() { Id = answerId.ToString() };
            var dtoAnswersList = new List<ManageAnswerDto>() { dtoAnswer };
            dtoQuestion.Answers = dtoAnswersList;
            var dtoQuestionsList = new List<ManageQuestionDto>() { dtoQuestion };

            var testDtoArgument = new ManageTestDto()
            {
                Id = testId.ToString(),
                CategoryName = category.Name,
                Questions = dtoQuestionsList
            };

            var sut = new TestService(
              testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
              mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object);

            // Act

            sut.EditTest(testDtoArgument);

            // Assert

            this.answerRepoMock.Verify(x => x.Update(questionAnswers[0]), Times.Once);
        }
    }
}
