using System;
using System.Collections.Generic;
using System.Linq;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.DTO.TakeTest;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<UserAnswerDto> GetAnswersForTestDoneByUser(string userId, string testId)
        {
            var answers = this.userAnswerRepo.All
                .Where(x => x.UserId == userId)
                .AsNoTracking();
            // if no .Include all the related data is loaded( ie Eager loading)
            // ASK!!
            //.Include(ua => ua.Answer);
            //.ThenInclude(a => a.Question)
            //.ThenInclude(q => q.Test);

            // the data here is null and below the DTO is fully populated 
            // with every property..?? How..

            var answersForTest = answers
                .Where(ua => ua.Answer.Question.Test.Id.ToString() == testId);

            var answersForTestDto = this.mapper.ProjectTo<UserAnswerDto>(answersForTest);

            return answersForTestDto;
        }
    }
}
