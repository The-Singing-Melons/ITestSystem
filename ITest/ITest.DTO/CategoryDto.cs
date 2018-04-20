using System.Collections.Generic;

namespace ITest.DTO
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<TestDto> Tests { get; set; }
    }
}