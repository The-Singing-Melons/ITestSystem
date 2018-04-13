using System;
using System.Collections.Generic;
using System.Text;

namespace Itest.Data.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Test> Tests { get; set; }
    }
}
