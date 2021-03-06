﻿using System.Collections.Generic;

namespace ITest.DTO
{
    public class ManageQuestionDto
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public ICollection<ManageAnswerDto> Answers { get; set; }
    }
}
