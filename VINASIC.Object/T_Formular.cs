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
    public partial class T_Formular
    {
        public T_Formular()
        {
            this.T_UserFormular = new Collection<T_UserFormular>();
            this.T_FormularDetail = new Collection<T_FormularDetail>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Formular { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual Collection<T_UserFormular> T_UserFormular { get; set; }
        public virtual Collection<T_FormularDetail> T_FormularDetail { get; set; }
    }
    
}
