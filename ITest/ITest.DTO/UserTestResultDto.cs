namespace ITest.DTO
{
    public class UserTestResultDto
    {
        public string TestName { get; set; }
        public string UserName { get; set; }
        public string CategoryName { get; set; }
        public int RequestedTime { get; set; }
        public float ExecutionTime { get; set; }
        public bool IsPassed { get; set; }
    }
}
