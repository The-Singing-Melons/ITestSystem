using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITest.Web.Areas.User.Models
{
    public class TakeTestViewModel
    {
        public Dictionary<QuestionViewModel, IEnumerable<TakeTestAnswerViewModel>>
            QuestionWithAnswersVM
        { get; set; }
        public TestViewModel TestViewModel { get; set; }
    }
}
