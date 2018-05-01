using System.Collections.Generic;

namespace ITest.DTO
{
    public class ManageTestDto
    {
        public string Id { get; set; }

        public string TestName { get; set; }

        public string CategoryName { get; set; }

        public int RequestedTime { get; set; }

        public bool IsPublished { get; set; }

        public bool IsDisabled { get; set; }

        public string CreatedByUserId { get; set; }

        public ICollection<ManageQuestionDto> Questions { get; set; }
    }
}
