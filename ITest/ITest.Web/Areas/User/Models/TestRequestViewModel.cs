using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITest.Web.Areas.User.Models
{
    public class TestRequestViewModel
    {
        public string Id { get; set; }

        public string CategoryName { get; set; }

        public IList<QuestionResponseModel> Questions { get; set; }
    }
}
