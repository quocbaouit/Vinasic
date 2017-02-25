namespace VINASIC.Business.Interface.Model
{
    public class UserService
    {
        public int? CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string EmployeeName { get; set; }
        public int[] Features { get; set; }
        public string ImagePath { get; set; }
        public bool IsOwner { get; set; }
        public string LogoCompany { get; set; }
        public string[] Permissions { get; set; }
        public int UserID { get; set; }
        public int employeeId { get; set; }
    }
}
