//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VINASIC.Object
{
    public partial class T_RoLe
    {
        public T_RoLe()
        {
            this.T_RolePermission = new Collection<T_RolePermission>();
            this.T_UserRole = new Collection<T_UserRole>();
            this.T_ProcessDetail = new Collection<T_ProcessDetail>();
        }
    
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Decription { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public bool IsStaff { get; set; }
    
        public virtual Collection<T_RolePermission> T_RolePermission { get; set; }
        public virtual Collection<T_UserRole> T_UserRole { get; set; }
        public virtual Collection<T_ProcessDetail> T_ProcessDetail { get; set; }
    }
    
}
