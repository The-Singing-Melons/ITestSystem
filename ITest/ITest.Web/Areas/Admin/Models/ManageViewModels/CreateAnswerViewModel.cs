using System.ComponentModel.DataAnnotations;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class CreateAnswerViewModel
    {
        //[Required(ErrorMessage = "Please enter the Answer's content!")]
        //[StringLength(50, ErrorMessage = "Answers content's length must be maximum 50 symbols!")]
        public string Content { get; set; }

        public bool IsCorrect { get; set; }
    }
}