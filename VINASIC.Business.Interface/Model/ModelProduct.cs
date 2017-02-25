using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelProduct :T_Product
    {
        public string ProductTypeName { get; set; }
    }

    public class UserProduct : T_Product
    {
        public string ProductTypeName { get; set; }
        public bool Selected { get; set; }
    }
}

