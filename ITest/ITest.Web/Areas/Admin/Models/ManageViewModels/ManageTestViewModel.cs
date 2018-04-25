using ITest.Web.Areas.Admin.Models.ManageViewModels.ValidationAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class ManageTestViewModel
    {
        [Required(ErrorMessage = "Please enter the Test's name!")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Test name's length must be atleast 4 and maximum 50 symbols!")]
        public string TestName { get; set; }

        [Required(ErrorMessage = "Please specify the Test's duration!")]
        [Range(10, 120, ErrorMessage = "Test's duration must be atleast 10 minutes and maximum 2 hours!")]
        public int RequestedTime { get; set; }

        [Required(ErrorMessage = "Please specify the Test's category!")]
        public string CategoryName { get; set; }

        public IList<string> CategoryNames { get; set; }

        public bool IsPublished { get; set; }

        [Required(ErrorMessage = "Please add Questions to your Test!")]
        [CollectionLegth(1, ErrorMessage = "Please add atleast one Question to your Test!")]
        public IList<ManageQuestionViewModel> Questions { get; set; }
    }
}
