using System;
using System.Collections.Generic;
using System.Text;
using Itest.Data.Models.Abstracts;

namespace Itest.Data.Models
{
    public class Category : DataModel
    {
        public string Name { get; set; }
        public ICollection<Test> Tests { get; set; }
    }
}
