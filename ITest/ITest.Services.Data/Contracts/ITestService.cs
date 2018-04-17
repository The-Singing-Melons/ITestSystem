using System;
using System.Collections.Generic;
using System.Text;
using ITest.DTO;
using ITest.Models;

namespace ITest.Services.Data.Contracts
{
    public interface ITestService
    {
        //IEnumerable<TestDto> GetAllTests();
        void AddTestsToUser(ApplicationUser user);
        IEnumerable<TestDto> GetUserTests(string id);
        TestDto GetTestById(string id);
        IEnumerable<QuestionDto> GetTestQuestions(string testId);
    }
}
