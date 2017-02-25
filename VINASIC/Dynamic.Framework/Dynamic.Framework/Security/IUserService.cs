using System.Collections.Generic;

namespace Dynamic.Framework.Security
{
    public interface IUserService
    {
        object State { get; set; }

        int? CompanyID { get; set; }

        string CompanyName { get; set; }

        string Description { get; set; }

        string Email { get; set; }

        string ImagePath { get; set; }

        bool IsOwner { get; set; }

        int UserID { get; set; }

        int employeeId { get; set; }

        string EmployeeName { get; set; }

        string[] Permissions { get; set; }

        int[] Features { get; set; }

        string LogoCompany { get; set; }

        int StoreID { get; set; }

        string SesssionId { get; set; }

        string DepartmentName { get; set; }

        List<IPermissionService> PermissionServices { get; set; }
    }
}
