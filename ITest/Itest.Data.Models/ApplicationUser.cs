using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Itest.Data.Models;
using Itest.Data.Models.Abstracts;
using Microsoft.AspNetCore.Identity;

namespace ITest.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Tests = new HashSet<Test>();
            this.UserTests = new HashSet<UserTest>();
        }

        public ICollection<Test> Tests { get; set; }

        public ICollection<UserTest> UserTests { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
