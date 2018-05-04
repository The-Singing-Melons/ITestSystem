using System;
using System.Collections.Generic;
using System.Text;
using ITest.DTO;
using ITest.DTO.TakeTest;

namespace ITest.Services.Data.Contracts
{
    public interface IUserAnswerService
    {
        void AddAnswersToUser(string userId, IList<QuestionResponseDto> questions);

        IEnumerable<UserAnswerDto> GetAnswersForTestDoneByUser(string userId, string testId);
    }
}
