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

        TestDto GetTestById(string id);

        TestDto GetTestQuestionsWithAnswers(string testId);

        TestDto GetRandomTest(string categoryName);

        void CreateTest(ManageTestDto testToAdd);
        bool IsTestPassed(string userId, TestRequestDto submitedTest);


        IEnumerable<TestDashBoardDto> GetTestsDashboardInfo();

        ManageTestDto GetTestByNameAndCategory(string name, string category);

        void EditTest(ManageTestDto createTestDto);

        void PublishTest(string name, string category);

        void DeleteTest(string name, string category);

        void DisableTest(string testName, string categoryName);

        void ShuffleTest(TestDto testToShuffle);
    }
}
