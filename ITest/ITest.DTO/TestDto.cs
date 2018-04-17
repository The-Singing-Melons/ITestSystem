using System;
using System.Collections.Generic;
using System.Text;

namespace ITest.DTO
{
    public class TestDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public bool IsPublished { get; set; }

        public string CreatedByUserId { get; set; }
        public ApplicationUserDto CreatedByUser { get; set; }

        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }

        public ICollection<QuestionDto> Questions { get; set; }
        public ICollection<UserTestDto> UserTests { get; set; }
    }
}
