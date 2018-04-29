using System.Collections.Generic;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public ICollection<CreatedTestViewModel> CreatedTests { get; set; }

        public ICollection<TestResultViewModel> TestResults { get; set; }
    }
}
