using System;
using System.ComponentModel.DataAnnotations;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class ManageAnswerViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter the Answer's content!")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Answers content's length must be maximum 500 symbols!")]
        public string ContentPlaintext { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsCorrect { get; set; }
    }
}