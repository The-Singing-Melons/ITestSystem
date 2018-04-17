using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITest.Web.Areas.User.Models
{
    public class TakeTestViewModel
    {
        public IEnumerable<QuestionViewModel> Questions { get; set; }
    }
}
