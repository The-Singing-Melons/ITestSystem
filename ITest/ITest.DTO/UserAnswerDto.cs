using System;
using System.Collections.Generic;
using System.Text;

namespace ITest.DTO
{
    public class UserAnswerDto
    {
        public ApplicationUserDto User { get; set; }
        public string UserId { get; set; }

        public AnswerDto Answer { get; set; }
        public Guid AnswerId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
