using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class TestScoreUserAnswerViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string QuestionContent { get; set; }
        public string AnswerContent { get; set; }
        public string TestName { get; set; }
        public string TestCategory { get; set; }
    }
}
