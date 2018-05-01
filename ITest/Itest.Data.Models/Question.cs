using System;
using System.Collections.Generic;
using System.Text;
using Itest.Data.Models.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Itest.Data.Models
{
    public class Question : DataModel
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }

        public Guid Id { get; set; }

        [Required]
        public string Body { get; set; }

        public Guid TestId { get; set; }
        public Test Test { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
