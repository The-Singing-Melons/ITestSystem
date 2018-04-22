using System.Collections.Generic;

namespace ITest.Web.Areas.User.Models
{
    public class QuestionViewModel
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public IList<TakeTestAnswerViewModel> Answers { get; set; }
    }
}