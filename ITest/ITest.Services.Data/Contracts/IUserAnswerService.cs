using System;
using System.Collections.Generic;
using System.Text;
using ITest.DTO.TakeTest;

namespace ITest.Services.Data.Contracts
{
    public interface IUserAnswerService
    {
        void AddAnswersToUser(string userId, IList<QuestionResponseViewModelDto> questions);
    }
}
