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
    public partial class T_Permission
    {
        public T_Permission()
        {
            this.T_RolePermission = new Collection<T_RolePermission>();
        }
    
        public int Id { get; set; }
        public int FeatureId { get; set; }
        public int PermissionTypeId { get; set; }
        public string SystemName { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDefault { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual T_Feature T_Feature { get; set; }
        public virtual Collection<T_RolePermission> T_RolePermission { get; set; }
    }
    
}
