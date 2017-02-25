using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelUser : T_User
    {
        public List<ModelUserRole> UserRoles { get; set; }

        public DateTime HireDate { get; set; }
        public string Position { get; set; }

        public string PositionName { get; set; }

        public string stringRoleName { get; set; }

        public List<int> ListRoleId { get; set; } 
    }
}
