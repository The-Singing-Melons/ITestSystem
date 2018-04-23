using System.Collections.Generic;

namespace ITest.DTO
{
    public class CreateTestDto
    {
        public string TestName { get; set; }

        public string CategoryName { get; set; }

        public int RequestedTime { get; set; }

        public bool IsPublished { get; set; }

        public string CreatedByUserId { get; set; }

        public IEnumerable<CreateQuestionDto> Questions { get; set; }
    }
}
