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
    public partial class T_Customer
    {
        public T_Customer()
        {
            this.T_Quittance = new Collection<T_Quittance>();
            this.T_ReceiptVoucher = new Collection<T_ReceiptVoucher>();
            this.T_Order = new Collection<T_Order>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CompanyName { get; set; }
    
        public virtual Collection<T_Quittance> T_Quittance { get; set; }
        public virtual Collection<T_ReceiptVoucher> T_ReceiptVoucher { get; set; }
        public virtual Collection<T_Order> T_Order { get; set; }
    }
    
}
