using System.ComponentModel.DataAnnotations;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class ManageAnswerViewModel
    {
        [Required(ErrorMessage = "Please enter the Answer's content!")]
        [StringLength(500, ErrorMessage = "Answers content's length must be maximum 500 symbols!")]
        public string ContentPlaintext { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsCorrect { get; set; }
    }
}