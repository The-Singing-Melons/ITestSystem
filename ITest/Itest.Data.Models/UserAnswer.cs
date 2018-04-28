using System;
using Itest.Data.Models.Abstracts;
using ITest.Models;

namespace Itest.Data.Models
{
    public class UserAnswer : IDeletable
    {
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public Answer Answer { get; set; }
        public Guid AnswerId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
