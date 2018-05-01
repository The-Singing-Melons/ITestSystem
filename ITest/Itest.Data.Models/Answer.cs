using System;
using System.Collections.Generic;
using Itest.Data.Models.Abstracts;
using ITest.Models;
using System.ComponentModel.DataAnnotations;

namespace Itest.Data.Models
{
    public class Answer : DataModel
    {
        public Answer()
        {
            this.UserAnswers = new HashSet<UserAnswer>();
        }

        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public bool IsCorrect { get; set; }

        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}