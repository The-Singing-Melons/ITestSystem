using System;
using System.Collections.Generic;
using System.Text;
using ITest.DTO;
using ITest.Models;

namespace ITest.Services.Data.Contracts
{
    public interface ITestService
    {
        IEnumerable<TestDto> GetUserTests(string id);

        TestDto GetTestById(string id);

        TestDto GetTestQuestionsWithAnswers(string testId);

        TestDto GetRandomTest(string categoryName);

        void CreateTest(CreateTestDto testToAdd);
        bool IsTestPassed(int count, int totalCorrectQuestions);
        int CalculateCorrectAnswers(TestDto testToBeCalculated);


        IEnumerable<TestDashBoardDto> GetTestsDashboardInfo();
    }
}
