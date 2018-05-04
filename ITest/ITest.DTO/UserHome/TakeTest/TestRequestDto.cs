using System.Collections.Generic;

namespace ITest.DTO.TakeTest
{
    public class TestRequestDto
    {
        public string Id { get; set; }

        public string CategoryName { get; set; }

        public IList<QuestionResponseDto> Questions { get; set; }
    }
}
