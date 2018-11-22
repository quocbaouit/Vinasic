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
    public partial class T_Order
    {
        public T_Order()
        {
            this.T_OrderDetail = new Collection<T_OrderDetail>();
            this.T_Quittance = new Collection<T_Quittance>();
            this.T_ReceiptVoucher = new Collection<T_ReceiptVoucher>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public double SubTotal { get; set; }
        public bool IsPayment { get; set; }
        public bool IsApproval { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<double> HasPay { get; set; }
        public int CreatedForUser { get; set; }
        public Nullable<bool> HasTax { get; set; }
        public string OrderView { get; set; }
        public int PaymentMethol { get; set; }
        public int IsDelivery { get; set; }
        public bool Process { get; set; }
        public Nullable<double> OrderStatus { get; set; }
        public Nullable<double> HaspayTransfer { get; set; }
        public Nullable<bool> IsQuittance { get; set; }
    
        public virtual T_Customer T_Customer { get; set; }
        public virtual T_User T_User { get; set; }
        public virtual Collection<T_OrderDetail> T_OrderDetail { get; set; }
        public virtual Collection<T_Quittance> T_Quittance { get; set; }
        public virtual Collection<T_ReceiptVoucher> T_ReceiptVoucher { get; set; }
    }
    
}
