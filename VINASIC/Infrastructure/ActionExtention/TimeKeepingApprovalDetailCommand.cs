namespace VINASIC.Infrastructure.ActionExtention
{
    public class TimeKeepingApprovalDetailCommand : BaseCommand
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}