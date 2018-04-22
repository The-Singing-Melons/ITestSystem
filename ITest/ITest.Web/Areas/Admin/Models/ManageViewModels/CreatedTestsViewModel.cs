using System.Collections.Generic;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class CreatedTestsViewModel
    {
        public CreatedTestsViewModel()
        {
            this.CreatedTests = new HashSet<TestViewModel>();
        }

        public ICollection<TestViewModel> CreatedTests { get; set; }
    }
}
