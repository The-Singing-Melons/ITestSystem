using System.ComponentModel.DataAnnotations;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class CreateAnswerViewModel
    {
        //[Required(ErrorMessage = "Required!!!")]
        //[StringLength(5, MinimumLength = 4, ErrorMessage = "Error")]
        public string Content { get; set; }

        public bool IsCorrect { get; set; }
    }
}