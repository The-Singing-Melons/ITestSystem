using System;
using System.Collections.Generic;
using System.Text;
using ITest.DTO;
using ITest.DTO.TakeTest;
using ITest.Models;

namespace ITest.Services.Data.Contracts
{
    public interface ITestService
    {
        IEnumerable<TestDto> GetUserTests(string id);

        TestDto GetTestById(string id);

        TestDto GetTestQuestionsWithAnswers(string testId);

        TestDto GetRandomTest(string categoryName);

        void CreateTest(ManageTestDto testToAdd);
        bool IsTestPassed(string userId, TestRequestViewModelDto submitedTest);


        IEnumerable<TestDashBoardDto> GetTestsDashboardInfo();

        ManageTestDto GetTestByNameAndCategory(string name, string category);
        void EditTest(ManageTestDto createTestDto);
    }
}
