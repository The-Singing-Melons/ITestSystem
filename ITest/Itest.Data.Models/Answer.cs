using System;
using System.Collections.Generic;
using Itest.Data.Models.Abstracts;

namespace Itest.Data.Models
{
    public class Answer : DataModel
    {
        public Answer()
        {
            this.UsersTestsQuestionsAnswers = new HashSet<UserTestQuestionAnswer>();
        }
        public Question Question { get; set; }
        public Guid QuestionId { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }

        public ICollection<UserTestQuestionAnswer> UsersTestsQuestionsAnswers { get; set; }
    }
}