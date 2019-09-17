using System;
using System.Collections.Generic;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelPaymentVoucher : T_PaymentVoucher
    {
        public string StrCreatedDate { get; set; }
    }
    public class ModelSavePaymentVoucher
    {
        public ModelSavePaymentVoucher()
        {
            Detail = new List<ModelPaymentVoucherDetail>();
        }
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerMail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTaxCode { get; set; }
        public string Content { get; set; }
        public bool Tax { get; set; }
        public float OrderTotal { get; set; }
        public float totalInclude { get; set; }
        public DateTime? DateDelivery { get; set; }
        public int PaymentType { get; set; }
        public float HasPay { get; set; }
        public string strHasPay { get; set; }
        public List<ModelPaymentVoucherDetail> Detail { get; set; }
    }

    public class ModelPaymentVoucherDetail
    {
        public int Id { get; set; }
        public string CommodityId { get; set; }
        public string CommodityName { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public Decimal Width { get; set; }
        public Decimal Height { get; set; }
        public Decimal Square { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal SumSquare { get; set; }
        public float Price { get; set; }
        public float Subtotal { get; set; }
    }
}

