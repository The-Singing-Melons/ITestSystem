﻿using System.ComponentModel.DataAnnotations;

namespace ITest.Web.Areas.User.Models
{
    public class DashboardTestViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
