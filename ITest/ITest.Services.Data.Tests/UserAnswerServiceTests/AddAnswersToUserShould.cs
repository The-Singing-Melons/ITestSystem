using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO.TakeTest;
using ITest.Infrastructure.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace ITest.Services.Data.Tests.UserAnswerServiceTests
{
    [TestClass]
    public class AddAnswersToUserShould
    {
        [TestMethod]
        public void Invoke_AddMethod_UserAnswerRepo_OncePerAnswer()
        {
            // Arrange
            var fakeUserId = "userId";

            var fakeQuestionResponseDto = new QuestionResponseDto()
            {
                Answers = new Guid().ToString()
            };
            var fakeQuestionResponseDtos = new List<QuestionResponseDto>() { fakeQuestionResponseDto };
            var answersCount = fakeQuestionResponseDtos.Count;

            var mockUserAnswerRepo = new Mock<IDataRepository<UserAnswer>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var answerRepoStub = new Mock<IDataRepository<Answer>>();

            var userAnswerService = new UserAnswerService(mockUserAnswerRepo.Object, dataSaverStub.Object, mappingProviderStub.Object, answerRepoStub.Object);

            // Act
            userAnswerService.AddAnswersToUser(fakeUserId, fakeQuestionResponseDtos);

            // Assert
            mockUserAnswerRepo.Verify(uar => uar.Add(It.IsAny<UserAnswer>()), Times.Exactly(answersCount));
        }

        [TestMethod]
        public void Invoke_AddMethod_UserAnswerRepo_OnlyForAnsweredQuestions()
        {
            // Arrange
            var fakeUserId = "userId";

            var fakeAnsweredQuestionDto = new QuestionResponseDto()
            {
                Answers = new Guid().ToString()
            };
            var fakeNotAnsweredQuestionDto = new QuestionResponseDto();

            var fakeQuestionResponseDtos = new List<QuestionResponseDto>() { fakeAnsweredQuestionDto, fakeNotAnsweredQuestionDto };

            var mockUserAnswerRepo = new Mock<IDataRepository<UserAnswer>>();
            var dataSaverStub = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var answerRepoStub = new Mock<IDataRepository<Answer>>();

            var userAnswerService = new UserAnswerService(mockUserAnswerRepo.Object, dataSaverStub.Object, mappingProviderStub.Object, answerRepoStub.Object);

            // Act
            userAnswerService.AddAnswersToUser(fakeUserId, fakeQuestionResponseDtos);

            // Assert
            mockUserAnswerRepo.Verify(uar => uar.Add(It.IsAny<UserAnswer>()), Times.Once);
        }

        [TestMethod]
        public void Invoke_SaveChangesMethod_DataSaver()
        {
            // Arrange
            string fakeUserId = "userId";

            var fakeQuestionResponseDtos = new List<QuestionResponseDto>();

            var userAnswerRepoStub = new Mock<IDataRepository<UserAnswer>>();
            var mockDataSaver = new Mock<IDataSaver>();
            var mappingProviderStub = new Mock<IMappingProvider>();
            var answerRepoStub = new Mock<IDataRepository<Answer>>();

            var userAnswerService = new UserAnswerService(userAnswerRepoStub.Object, mockDataSaver.Object, mappingProviderStub.Object, answerRepoStub.Object);

            // Act
            userAnswerService.AddAnswersToUser(fakeUserId, fakeQuestionResponseDtos);

            // Assert
            mockDataSaver.Verify(ds => ds.SaveChanges(), Times.Once);
        }
    }
}
