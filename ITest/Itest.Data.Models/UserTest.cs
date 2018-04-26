using System;
using Itest.Data.Models.Abstracts;
using ITest.Models;

namespace Itest.Data.Models
{
    public class UserTest : IDeletable
    {
        // Possibly Add DateTime Started / Finished
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid TestId { get; set; }
        public Test Test { get; set; }
        public bool? IsPassed { get; set; }
        public bool? IsSubmited { get; set; }
        public float ExecutionTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
