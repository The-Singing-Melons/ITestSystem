using System.Collections.Generic;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            this.CreatedTests = new HashSet<CreatedTestViewModel>();
            this.TestResults = new HashSet<TestResultViewModel>();
        }

        public ICollection<CreatedTestViewModel> CreatedTests { get; set; }

        public ICollection<TestResultViewModel> TestResults { get; set; }
    }
}
