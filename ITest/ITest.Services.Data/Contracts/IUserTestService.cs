using System;
using System.Collections.Generic;
using System.Text;

namespace ITest.Services.Data.Contracts
{
    public interface IUserTestService
    {
        void AddUserToTest(string testId, string userId, bool isPassed);
    }
}
