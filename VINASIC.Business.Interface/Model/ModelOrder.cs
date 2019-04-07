using System;
using System.Collections.Generic;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelOrder : T_Order
    {
        public string strIspayment { get; set; }
        public string strIsApproval { get; set; }
        public string strSubTotal { get; set; }
        public string CreateUserName { get; set; }
        public double PrintfPecent { get; set; }
        public string strHaspay { get; set; }
        public string strHaspayTransfer { get; set; }
        public string strHasTax { get; set; }
        public string strOrderStatus { get; set; }

        public string StrHasDelivery { get; set; }
        public string StrPaymentType { get; set; }
        
        public string StrCreatedDate{ get; set; }
        public string StrDeliveryDate { get; set; }
        public double Total { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTaxCode { get; set; }

        public string strFileName { get; set; }
        public string strCost { get; set; }
        public string strIncome { get; set; }

        public List<CostObj> CostObj { get; set; }
    }
    public class BusinessOrder {
        public string BusinessName { get; set; }

        public ModelOrder Orders { get; set; }
    }

    public class ModelExportOrder
    {
        public DateTime NgayTao { get; set; }
        public string TenKhachHang { get; set; }
        public string TenDichVu { get; set; }
        public string MoTa { get; set; }
        public string DonVi { get; set; }
        public decimal ChieuDai { get; set; }
        public decimal ChieuRong { get; set; }
        public decimal DienTich { get; set; }       
        public decimal SoLuong { get; set; }
        public decimal TongDienTich { get; set; }
        public double DonGia { get; set; }
        public double ThanhTien { get; set; }        
    }

    public class ModelViewDetail:T_OrderDetail
    {
        public string CommodityName { get; set; }
        public string strPrice { get; set; }
        public string strSubTotal { get; set; }
        public string strIsComplete { get; set; }

        public string Unit { get; set; }

        public double Total { get; set; }
        public double Total1 { get; set; }
        public double HasPay { get; set; }
        public double HasExist { get; set; }
        public double HasPayTransfer { get; set; }
        public double HasPayTotal { get; set; }
        public double HasExistTotal { get; set; }
        public double HasPayTransferTotal { get; set; }
        public string strTotal { get; set; }
        public string strHaspay { get; set; }      
        public string strHaspayTransfer { get; set; }
        public string isExist { get; set; }
        public string strPrinStatus { get; set; }
        public string strDesignStatus { get; set; }
        public string DesignUserName { get; set; }
        public string PrintUserName { get; set; }
        public int CreatedForUser { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CreateForUserName { get; set; }
        public string CreateForUserMobile { get; set; }
        public string UserProcess { get; set; }
    }
    public class CostObj
    {
        public Guid? Id { get; set; }
        [System.ComponentModel.DefaultValue(1)]
        public string Content { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public float? Amount { get; set; }
    }
}

