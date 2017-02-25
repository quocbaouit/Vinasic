using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VINASIC.Models
{
    public class RolePermissionModel
    {
        public int RoleId { get; set; }
        public List<int> ListPermission { get; set; }
    }
}