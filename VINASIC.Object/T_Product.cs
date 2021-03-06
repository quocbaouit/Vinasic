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
    public partial class T_Product
    {
        public T_Product()
        {
            this.T_OrderDetail = new Collection<T_OrderDetail>();
            this.T_UserProduct = new Collection<T_UserProduct>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> Inventory { get; set; }
        public Nullable<int> ProcessId { get; set; }
    
        public virtual T_ProductType T_ProductType { get; set; }
        public virtual Collection<T_OrderDetail> T_OrderDetail { get; set; }
        public virtual Collection<T_UserProduct> T_UserProduct { get; set; }
        public virtual T_Process T_Process { get; set; }
    }
    
}
