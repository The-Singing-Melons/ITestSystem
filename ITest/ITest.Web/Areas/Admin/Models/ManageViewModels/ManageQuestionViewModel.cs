using ITest.Web.Areas.Admin.Models.ManageViewModels.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class ManageQuestionViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter the Question's description!")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Question description's length must be maximum 500 symbols!")]
        public string BodyPlaintext { get; set; }

        [Required]
        public string Body { get; set; }

        [Required(ErrorMessage = "Please add Answers to your Question!")]
        [CollectionLegth(2, ErrorMessage = "Please add atleast two Aswers to your Question!")]
        public IList<ManageAnswerViewModel> Answers { get; set; }
    }
}
