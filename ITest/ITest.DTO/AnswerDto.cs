namespace ITest.DTO
{
    public class AnswerDto
    {
        public string Id { get; set; }
        public QuestionDto Question { get; set; }
        public string QuestionId { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        public string SelectedAnswerId { get; set; }
    }
}