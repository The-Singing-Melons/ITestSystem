using System;
using System.Collections.Generic;
using System.Text;
using Itest.Data.Models.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Itest.Data.Models
{
    public class Category : DataModel
    {
        public Category()
        {
            this.Tests = new HashSet<Test>();
        }

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Test> Tests { get; set; }
    }
}
