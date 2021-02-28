using System.Reflection.Emit;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelOrderDetail : T_OrderDetail
    {
        public string CommodityName { get; set; }
        public string strPrice { get; set; }
        public string strSubTotal { get; set; }
        public string strIsComplete { get; set; }

        public string strPrinStatus { get; set; }
        public string strDesignStatus { get; set; }

        public string DesignUserName { get; set; }
        public string PrintUserName { get; set; }
        public string UserProcess { get; set; }
        public string strTransport { get; set; }

    }
}

