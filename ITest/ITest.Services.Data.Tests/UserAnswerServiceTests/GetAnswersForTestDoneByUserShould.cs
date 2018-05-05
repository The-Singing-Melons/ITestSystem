using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITest.Services.Data.Tests.UserAnswerServiceTests
{
    [TestClass]
    public class GetAnswersForTestDoneByUserShould
    {
        [TestMethod]
        public void Invoke_MappingProvider_ProjectTo_With_ResultsFrom_UserAnswerRepo()
        {
            // Arrange
            var fakeUserId = "userId";
            var fakeTestId = new Guid();

            var fakeUserAnswer = new UserAnswer()
            {
                UserId = fakeUserId,
                Answer = new Answer() { Question = new Question() { Test = new Test() { Id = fakeTestId } } }
            };
            var fakeUserAnswers = new List<UserAnswer>() { fakeUserAnswer }.AsQueryable();

            var userAnswerRepoStub = new Mock<IDataRepository<UserAnswer>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mockMappingProvider = new Mock<IMappingProvider>();
            var answerRepoStub = new Mock<IDataRepository<Answer>>();

            var userAnswerService = new UserAnswerService(userAnswerRepoStub.Object, dataSaverStub.Object, mockMappingProvider.Object, answerRepoStub.Object);

            userAnswerRepoStub.SetupGet(uar => uar.All).Returns(fakeUserAnswers);

            // Act
            userAnswerService.GetAnswersForTestDoneByUser(fakeUserId, fakeTestId.ToString());

            // Assert
            mockMappingProvider.Verify(mp => mp.ProjectTo<UserAnswerDto>(It.IsAny<IQueryable<UserAnswer>>()), Times.Once);
        }

        [TestMethod]
        public void Throw_ArgumentException_When_NoAnswers_AreFound()
        {
            // Arrange
            var fakeUserId = "userId";
            var fakeTestId = new Guid();

            var fakeUserAnswers = new List<UserAnswer>() { }.AsQueryable();

            var userAnswerRepoStub = new Mock<IDataRepository<UserAnswer>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var answerRepoStub = new Mock<IDataRepository<Answer>>();

            var userAnswerService = new UserAnswerService(userAnswerRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, answerRepoStub.Object);

            userAnswerRepoStub.SetupGet(uar => uar.All).Returns(fakeUserAnswers);

            Action executingGetAnswersForTestDoneByUser = () => userAnswerService.GetAnswersForTestDoneByUser(fakeUserId, fakeTestId.ToString());

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(executingGetAnswersForTestDoneByUser);
        }

        [TestMethod]
        public void Throw_ArgumentNullException_WhenUserId_IsNullOrEmpty()
        {
            // Arrange
            var fakeUserId = "";
            var fakeTestId = new Guid();

            var userAnswerRepoStub = new Mock<IDataRepository<UserAnswer>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var answerRepoStub = new Mock<IDataRepository<Answer>>();

            var userAnswerService = new UserAnswerService(userAnswerRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, answerRepoStub.Object);

            Action executingGetAnswersForTestDoneByUser = () => userAnswerService.GetAnswersForTestDoneByUser(fakeUserId, fakeTestId.ToString());

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(executingGetAnswersForTestDoneByUser);
        }

        [TestMethod]
        public void Throw_ArgumentNullException_WhenTestId_IsNullOrEmpty()
        {
            // Arrange
            var fakeUserId = "userId";
            var fakeTestId = "";

            var userAnswerRepoStub = new Mock<IDataRepository<UserAnswer>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var answerRepoStub = new Mock<IDataRepository<Answer>>();

            var userAnswerService = new UserAnswerService(userAnswerRepoStub.Object, dataSaverStub.Object, mappingProviderStub.Object, answerRepoStub.Object);

            Action executingGetAnswersForTestDoneByUser = () => userAnswerService.GetAnswersForTestDoneByUser(fakeUserId, fakeTestId.ToString());

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(executingGetAnswersForTestDoneByUser);
        }
    }
}
