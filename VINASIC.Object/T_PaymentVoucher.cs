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
    public partial class T_PaymentVoucher
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public double Money { get; set; }
        public Nullable<int> Debit { get; set; }
        public Nullable<int> Credit { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string ReceiptName { get; set; }
        public string ReceiptAddress { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
    }
    
}
