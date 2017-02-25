using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelStockIn : T_StockIn
    {
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTaxCode { get; set; }
    }

    public class ModelViewStockDetail : T_StockInDetail
    {
        public int StockId{ get; set; }
        public string Name { get; set; }
        public string strTotal { get; set; }
        public double Total { get; set; }
        public string StockDescription { get; set; }
    }
}

