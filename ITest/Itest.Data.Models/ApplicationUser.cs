using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Itest.Data.Models;
using Itest.Data.Models.Abstracts;
using Microsoft.AspNetCore.Identity;

namespace ITest.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IEditable, IDeletable
    {
        public ApplicationUser()
        {
            this.Tests = new HashSet<Test>();
            //this.UserTests = new HashSet<UserTest>();
        }
        public ICollection<Test> Tests { get; set; }


        public ICollection<UserTest> UserTests { get; set; }
        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
    }
}
