using System;

namespace ITest.DTO
{
    public class UserTestDto
    {
        public string Id { get; set; }
        public TestDto Test { get; set; }
        public DateTime StartedOn { get; set; }
    }
}