using ITest.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITest.Services.Data.Contracts
{
    public interface IUserTestService
    {
        void AddUserToTest(string testId, string userId);
        bool CheckForCompletedUserTestInCategory(string userId, string categoryName);
        bool UserStartedTest(string testId, string userId);
        DateTime GetStartingTimeForUserTest(string userId, string testId);
        void SubmitUserTest(string testId, string userId, bool isPassed);
        IEnumerable<UserTestResultDto> GetUserTestResults();
    }
}
