using System;

namespace ITest.DTO
{
    public class UserTestDto
    {
        public string UserId { get; set; }
        public ApplicationUserDto User { get; set; }

        public Guid TestId { get; set; }
        public TestDto Test { get; set; }

        public bool? IsPassed { get; set; }
        public bool? IsSubmited { get; set; }
        public double ExecutionTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}