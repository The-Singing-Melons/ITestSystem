using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITest.Web.Areas.User.Models
{
    public class TestViewModel
    {
        public string Id { get; set; }

        public int Duration { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public double TimeRemaining { get; set; }

        public IList<QuestionViewModel> Questions { get; set; }
    }
}
