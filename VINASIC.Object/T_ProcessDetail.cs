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
    public partial class T_ProcessDetail
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public int RoleId { get; set; }
        public int Index { get; set; }
        public string RolesName { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual T_Process T_Process { get; set; }
        public virtual T_RoLe T_RoLe { get; set; }
    }
    
}
