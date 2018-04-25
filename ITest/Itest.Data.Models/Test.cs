using System;
using System.Collections.Generic;
using System.Text;
using Itest.Data.Models.Abstracts;
using ITest.Models;

namespace Itest.Data.Models
{
    public class Test : DataModel
    {
        public Test()
        {
            this.Questions = new HashSet<Question>();
            //this.UserTests = new HashSet<UserTest>();
        }
        public string Name { get; set; }
        public int Duration { get; set; }
        public bool IsPublished { get; set; }

        public string CreatedByUserId { get; set; }
        public ApplicationUser CreatedByUser { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<UserTest> UserTests { get; set; }
    }
}
