using System;
using System.Collections.Generic;

namespace ITest.DTO
{
    public class QuestionDto
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public IList<AnswerDto> Answers { get; set; }
        public Guid TestId { get; set; }
        public TestDto Test { get; set; }
    }
}