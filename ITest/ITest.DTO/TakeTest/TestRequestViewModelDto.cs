using System.Collections.Generic;

namespace ITest.DTO.TakeTest
{
    public class TestRequestViewModelDto
    {
        public string Id { get; set; }

        public string CategoryName { get; set; }

        public IList<QuestionResponseViewModelDto> Questions { get; set; }
    }
}
