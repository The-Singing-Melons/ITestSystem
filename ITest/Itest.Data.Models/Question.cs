using System;
using System.Collections.Generic;
using System.Text;
using Itest.Data.Models.Abstracts;

namespace Itest.Data.Models
{
    public class Question : DataModel
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }
        public string Body { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public Guid TestId { get; set; }
        public Test Test { get; set; }
    }
}
