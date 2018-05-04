using System;
using System.Collections.Generic;
using System.Text;

namespace ITest.DTO.TakeTest
{
    public class QuestionResponseDto
    {
        public string Id { get; set; }

        // Change name with [Name] annotation - how?
        public string Answers { get; set; }
    }
}
