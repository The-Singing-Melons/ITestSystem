using System;
using System.Collections.Generic;
using System.Text;
using ITest.DTO;

namespace ITest.Services.Data.Contracts
{
    public interface IUserTestQuestionAnswerService
    {
        void RecordUserAnswerToQuestionInTest(string userId, TestDto submitedTest);
    }
}
