using System;
using Itest.Data.Models.Abstracts;
using ITest.Models;

namespace Itest.Data.Models
{
    public class UserAnswer : DataModel
    {
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public Answer Answer { get; set; }
        public Guid AnswerId { get; set; }

    }
}
