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
        private readonly IDataRepository<Answer> answerRepo;
        private readonly IDataSaver dataSaver;
        private readonly IMappingProvider mapper;

        public UserAnswerService(IDataRepository<UserAnswer> userAnswerRepo,
            IDataSaver dataSaver, IMappingProvider mapper, IDataRepository<Answer> answerRepo)
        {
            this.userAnswerRepo = userAnswerRepo ?? throw new ArgumentNullException(nameof(userAnswerRepo));
            this.answerRepo = answerRepo ?? throw new ArgumentNullException(nameof(answerRepo));
            this.dataSaver = dataSaver ?? throw new ArgumentNullException(nameof(dataSaver));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void AddAnswersToUser(string userId, IList<QuestionResponseViewModelDto> questions)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

            if (questions == null)
            {
                throw new ArgumentNullException(nameof(questions));
            }

            for (int i = 0; i < questions.Count; i++)
            {

                if (questions[i].Answers == null)
                {
                    continue;

                    //var defaultAnswerContent = "No question selected";
                    //var defaultAnswer = this.answerRepo.All
                    //    .Where(a => a.Content == defaultAnswerContent)
                    //    .FirstOrDefault();

                    //if (defaultAnswer == null)
                    //{
                    //    defaultAnswer = new Answer()
                    //    {
                    //        IsDeleted = false,
                    //        Content = defaultAnswerContent,
                    //        QuestionId = Guid.Parse(questions[i].Id)
                    //    };

                    //    this.answerRepo.Add(defaultAnswer);

                    //    this.dataSaver.SaveChanges();
                    //}

                    //questions[i].Answers = defaultAnswer.Id.ToString();
                }

                var userAnswer = new UserAnswer
                {
                    AnswerId = Guid.Parse(questions[i].Answers),
                    UserId = userId,
                    IsDeleted = false
                };

                this.userAnswerRepo.Add(userAnswer);

            }

            this.dataSaver.SaveChanges();

        }

        public IEnumerable<UserAnswerDto> GetAnswersForTestDoneByUser(string userId, string testId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            var answers = this.userAnswerRepo.All
                .Where(x => x.UserId == userId)
                 .Include(ua => ua.Answer)
                    .ThenInclude(a => a.Question)
                        .ThenInclude(q => q.Test);

            var answersForTest = answers
                .Where(ua => ua.Answer.Question.Test.Id.ToString() == testId);


            if (answersForTest.Count() == 0)
            {
                throw new ArgumentException("No Answers!");
            }

            var answersForTestDto = this.mapper.ProjectTo<UserAnswerDto>(answersForTest);

            return answersForTestDto;
        }
    }
}
