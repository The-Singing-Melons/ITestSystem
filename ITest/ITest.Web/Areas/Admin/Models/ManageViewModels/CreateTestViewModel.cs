using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class CreateTestViewModel
    {
        [Required(ErrorMessage = "Required!!!")]
        [StringLength(5, MinimumLength = 4, ErrorMessage = "Error")]
        public string TestName { get; set; }

        [Required(ErrorMessage = "Required!!!")]
        [Range(4, 5, ErrorMessage = "Error")]
        public int RequestedTime { get; set; }

        [Required(ErrorMessage = "Required!!!")]
        [StringLength(5, MinimumLength = 4, ErrorMessage = "Error")]
        public string CategoryName { get; set; }

        public IList<CreateQuestionViewModel> Questions { get; set; }
    }
}
