using System.Collections.Generic;

namespace ITest.DTO
{
    public class CreateQuestionDto
    {
        public string Body { get; set; }

        public IEnumerable<CreateAnswerDto> Answers { get; set; }
    }
}
