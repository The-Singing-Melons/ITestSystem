using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITest.Web.Areas.User.Models
{
    public class TestViewModel
    {
        [Required]
        string Name { get; set; }
    }
}
