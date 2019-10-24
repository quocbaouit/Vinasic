using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VINASIC.Business.Interface.Model
{
    public class ModelSaveOrder
    {
        public ModelSaveOrder()
        {
            Detail = new List<ModelDetail>();
        }
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerMail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTaxCode { get; set; }
        public bool Tax { get; set; }
        public float OrderTotal { get; set; }
        public float OrderTotalExcludeTax { get; set; }
        public DateTime? DateDelivery { get; set; }
        public List<ModelDetail> Detail { get; set; }
        public float Deposit { get; set; }
    }

    public class ModelDetail
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
