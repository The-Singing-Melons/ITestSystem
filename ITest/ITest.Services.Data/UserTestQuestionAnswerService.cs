using System;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Services.Data.Contracts;

namespace ITest.Services.Data
{
    public class UserTestQuestionAnswerService : IUserTestQuestionAnswerService
    {

        private readonly IDataRepository<UserTestQuestionAnswer> userTestQuestionAnswerRepo;
        private readonly IDataSaver dataSaver;
        private readonly IMappingProvider mapper;

        public UserTestQuestionAnswerService(IDataRepository<UserTestQuestionAnswer> userTestQuestionAnswerRepo,
            IDataRepository<Test> testRepo, IDataSaver dataSaver, IMappingProvider mapper)
        {
            this.userTestQuestionAnswerRepo = userTestQuestionAnswerRepo;
            this.dataSaver = dataSaver;
            this.mapper = mapper;
        }

        public void RecordUserAnswerToQuestionInTest(string userId, TestDto submitedTest)
        {
            var testId = submitedTest.Id;
            foreach (var question in submitedTest.Questions)
            {
                foreach (var answer in question.Answers)
                {
                    var userTestQuestionAnswerModel = new UserTestQuestionAnswer
                    {
                        UserId = userId,
                        TestId = Guid.Parse(testId),
                        QuestionId = Guid.Parse(question.Id),
                        AnswerId = Guid.Parse(answer.Id)
                    };
                    this.userTestQuestionAnswerRepo.Add(userTestQuestionAnswerModel);
                }
            }
            this.dataSaver.SaveChanges();
        }
    }
}
