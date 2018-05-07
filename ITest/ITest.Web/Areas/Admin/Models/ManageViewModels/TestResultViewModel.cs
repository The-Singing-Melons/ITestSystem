
namespace ITest.Web.Areas.Admin.Models.ManageViewModels
{
    public class TestResultViewModel
    {
        public string UserId { get; set; }
        public string TestId { get; set; }
        public string TestName { get; set; }
        public string UserName { get; set; }
        public string CategoryName { get; set; }
        public int RequestedTime { get; set; }
        public double ExecutionTime { get; set; }
        public bool IsPassed { get; set; }
    }
}
