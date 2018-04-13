using System;
using System.Collections.Generic;
using System.Text;

namespace Itest.Data.Models
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public string Body { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public Guid TestId { get; set; }
        public Test Test { get; set; }
    }
}
