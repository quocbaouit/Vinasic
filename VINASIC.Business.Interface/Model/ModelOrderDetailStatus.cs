using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelOrderDetailStatus : T_OrderDetailStatus
    {
        public string ColorCode { get; set; }
    }

    public class ModelOrderDetailStatusPrint : T_OrderDetailStatusPrint
    {
        public string ColorCode { get; set; }
    }
}

