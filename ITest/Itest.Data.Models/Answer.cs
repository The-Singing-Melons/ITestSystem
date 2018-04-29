using System;
using System.Collections.Generic;
using Itest.Data.Models.Abstracts;
using ITest.Models;

namespace Itest.Data.Models
{
    public class Answer : DataModel
    {

        public Answer()
        {
            this.UserAnswers = new HashSet<UserAnswer>();
        }

        public Question Question { get; set; }
        public Guid QuestionId { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }

    }
}