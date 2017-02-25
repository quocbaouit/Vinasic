using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VINASIC.Models
{
    public class UserRoleModel
    {
        public int UserId { get; set; }
        public List<int> ListRole { get; set; }
    }

    public class ListSelectProductModel
    {
        public int UserId { get; set; }
        public List<int> ListSelectProduct { get; set; }
    }
    
}