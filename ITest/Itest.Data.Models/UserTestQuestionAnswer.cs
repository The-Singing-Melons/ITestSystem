using System;
using Itest.Data.Models.Abstracts;

namespace Itest.Data.Models
{
    public class UserTestQuestionAnswer : IDeletable
    {
        public string UserId { get; set; }
        public Guid TestId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
