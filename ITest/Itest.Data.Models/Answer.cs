using System;

namespace Itest.Data.Models
{
    public class Answer
    {
        public Guid AnswerId { get; set; }
        public Question Question { get; set; }
        public Guid QuestionId { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }
}