using System;
using System.Collections.Generic;
using System.Text;
using ITest.DTO;

namespace ITest.Services.Data.Contracts
{
    public interface IQuestionService
    {
        IEnumerable<AnswerDto> GetAnswersForQuestion(string questionId);
    }
}
