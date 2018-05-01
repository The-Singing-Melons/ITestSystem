using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class TestScoreUserAnswerViewModel
    {
        public TestScoreUserAnswerViewModel()
        {
            this.UserDetailsViewModel = new TestScoreUserDetailsViewModel();
            this.AnswerForTestViewModels = new HashSet<TestScoreAnswerForTestViewModel>();
        }

        public TestScoreUserDetailsViewModel UserDetailsViewModel { get; set; }

        public IEnumerable<TestScoreAnswerForTestViewModel> AnswerForTestViewModels { get; set; }
    }
}
