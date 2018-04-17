using System;
using System.Collections.Generic;

namespace ITest.DTO
{
    public class QuestionDto
    {
        public string Body { get; set; }
        public ICollection<AnswerDto> Answers { get; set; }
        public Guid TestId { get; set; }
        public TestDto Test { get; set; }
    }
}