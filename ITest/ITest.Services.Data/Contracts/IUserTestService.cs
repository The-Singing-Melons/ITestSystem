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


        bool UserStartedTest(string testId, string userId);

        DateTime GetStartingTimeForUserTest(string userId, string testId);

        void SubmitUserTest(string testId, string userId, bool isPassed);

        bool CheckForCompletedUserTestInCategory(string userId, string categoryName, IEnumerable<UserTest> testsTakenByUser);

        IEnumerable<UserTest> GetAllTestsDoneByUser(string userId);

        IEnumerable<UserTestResultDto> GetUserTestResults();
        UserTestDto CheckForTestInProgress(string userId);
    }
}
