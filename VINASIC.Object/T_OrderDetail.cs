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
    public partial class T_OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
        public double SubTotal { get; set; }
        public string Description { get; set; }
        public Nullable<int> DesignUser { get; set; }
        public Nullable<int> DesignStatus { get; set; }
        public Nullable<int> PrintUser { get; set; }
        public Nullable<int> PrintStatus { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> DesignFrom { get; set; }
        public Nullable<System.DateTime> DesignTo { get; set; }
        public Nullable<System.DateTime> PrintFrom { get; set; }
        public Nullable<System.DateTime> PrintTo { get; set; }
        public string FileName { get; set; }
        public string DesignDescription { get; set; }
        public string PrintDescription { get; set; }
        public int Index { get; set; }
        public string DesignView { get; set; }
        public string PrintView { get; set; }
        public Nullable<decimal> Square { get; set; }
        public Nullable<decimal> SumSquare { get; set; }
    
        public virtual T_Order T_Order { get; set; }
        public virtual T_Product T_Product { get; set; }
        public virtual T_User T_User { get; set; }
        public virtual T_User T_User1 { get; set; }
    }
    
}
