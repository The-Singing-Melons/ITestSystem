using System.Collections.Generic;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class CreateTestViewModel
    {
        public string TestName { get; set; }

        public int RequestedTime { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<CreateQuestonViewModel> Questions { get; set; }
    }
}
