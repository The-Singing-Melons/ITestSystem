using System;
using Itest.Data.Models.Abstracts;
using ITest.Models;
using System.ComponentModel.DataAnnotations;

namespace Itest.Data.Models
{
    public class UserAnswer : DataModel
    {
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid AnswerId { get; set; }
        public Answer Answer { get; set; }
    }
}
