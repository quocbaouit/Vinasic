namespace VINASIC.Infrastructure.ActionExtention
{
    public class BaseCommand
    {
        public bool IsCanAdd { get; set; }
        public bool IsCanUpdate { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanApprove { get; set; }
        public bool IsCanLock { get; set; }
        public bool IsCanViewEmployeeDetail { get; set; }
    }
}