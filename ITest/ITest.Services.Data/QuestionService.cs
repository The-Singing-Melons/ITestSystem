using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ITest.Services.Data
{
    public class QuestionService : IQuestionService
    {
        private readonly IDataRepository<Question> questionRepo;
        private readonly IDataSaver dataSaver;
        private readonly IMappingProvider mapper;

        public QuestionService(IDataRepository<Question> questionRepo, IDataSaver dataSaver, IMappingProvider mapper)
        {
            this.questionRepo = questionRepo ?? throw new ArgumentNullException(nameof(questionRepo));
            this.dataSaver = dataSaver ?? throw new ArgumentNullException(nameof(dataSaver));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<AnswerDto> GetAnswersForQuestion(string questionId)
        {
            if (string.IsNullOrEmpty(questionId))
            {
                throw new ArgumentNullException("cannot be null!");
            }

            var answersForQuestion = this.questionRepo.All.
                                        Include(q => q.Answers)
                                        .Where(q => q.Id.ToString() == questionId)
                                        .SelectMany(q => q.Answers);

            var answersDto = mapper.ProjectTo<AnswerDto>(answersForQuestion);

            return answersDto.ToList();
        }
    }
}
