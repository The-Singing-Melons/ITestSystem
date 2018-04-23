using System;
using Itest.Data.Models.Abstracts;
using ITest.Models;

namespace Itest.Data.Models
{
    public class UserTestQuestionAnswer : IDeletable
    {
        public Guid Id { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public Test Test { get; set; }
        public Guid TestId { get; set; }

        public Question Question { get; set; }
        public Guid QuestionId { get; set; }

        public Answer Answer { get; set; }
        public Guid AnswerId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
