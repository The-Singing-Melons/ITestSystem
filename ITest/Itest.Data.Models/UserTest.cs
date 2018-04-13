using System;
using System.Collections.Generic;
using System.Text;
using ITest.Models;

namespace Itest.Data.Models
{
    public class UserTest
    {
        // Possibly Add DateTime Started / Finished
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid TestId { get; set; }
        public Test Test { get; set; }
        public bool IsPassed { get; set; }
        public float ExecutionTime { get; set; }
    }
}
