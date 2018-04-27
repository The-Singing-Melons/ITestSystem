using System;
using System.Collections.Generic;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO.TakeTest;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Services.Data.Contracts;

namespace ITest.Services.Data
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly IDataRepository<UserAnswer> userAnswerRepo;
        private readonly IDataSaver dataSaver;
        private readonly IMappingProvider mapper;

        public UserAnswerService(IDataRepository<UserAnswer> userAnswerRepo,
            IDataSaver dataSaver, IMappingProvider mapper)
        {
            this.userAnswerRepo = userAnswerRepo;
            this.dataSaver = dataSaver;
            this.mapper = mapper;
        }

        public void AddAnswersToUser(string userId, IList<QuestionResponseViewModelDto> questions)
        {
            foreach (var answer in questions)
            {
                var userAnswer = new UserAnswer
                {
                    AnswerId = Guid.Parse(answer.Answers),
                    UserId = userId,
                    IsDeleted = false
                };

                this.userAnswerRepo.Add(userAnswer);
            }

            this.dataSaver.SaveChanges();

        }
    }
}
