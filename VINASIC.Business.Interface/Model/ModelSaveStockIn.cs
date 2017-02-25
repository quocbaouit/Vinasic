using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VINASIC.Business.Interface.Model
{
   public class ModelSaveStockIn
    {
        public ModelSaveStockIn()
        {
            Detail = new List<ModelStockDetail>();
        }
        public int StockInId { get; set; }
        public int PartnerId { get; set; }
        public string Description { get; set; }
        public string CustomerName { get; set; }
        public float OrderTotal { get; set; }
        public DateTime? DateDelivery { get; set; }
        public List<ModelStockDetail> Detail { get; set; }
    }

    public class ModelStockDetail
    {
        public int Index { get; set; }
        public string MaterialId { get; set; }
        public string MateriaName { get; set; }
        public string Description { get; set; }       
        public float Quantity { get; set; }
        public float Price { get; set; }
        public float SubTotal { get; set; }
    }  
}
