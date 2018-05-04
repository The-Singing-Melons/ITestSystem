using ITest.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using Itest.Data.Models;

namespace ITest.Services.Data.Contracts
{
    public interface IUserTestService
    {
        void AddUserToTest(string testId, string userId);

        double GetTimeRemainingForUserTest(string userId, string testId, double testDuration);

        void SubmitUserTest(string testId, string userId, bool isPassed);

        IEnumerable<UserTestResultDto> GetUserTestResults();

        bool CheckForOverdueTestInProgress(string userId);

        bool UserHasCompletedTest(string userId, string testId);
    }
}
