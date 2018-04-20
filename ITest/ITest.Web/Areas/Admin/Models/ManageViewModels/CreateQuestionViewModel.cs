using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class CreateQuestionViewModel
    {
        [Required(ErrorMessage = "Required!!!")]
        [StringLength(5, MinimumLength = 4, ErrorMessage = "Error")]
        public string Body { get; set; }

        public IList<CreateAnswerViewModel> Answers { get; set; }
    }
}
