using System;
using Itest.Data.Models.Abstracts;
using ITest.Models;
using System.ComponentModel.DataAnnotations;

namespace Itest.Data.Models
{
    public class UserTest : DataModel
    {
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid TestId { get; set; }
        public Test Test { get; set; }

        public bool? IsPassed { get; set; }

        public bool? IsSubmited { get; set; }

        public double ExecutionTime { get; set; }

        public DateTime StartedOn { get; set; }
    }
}
