using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllOrderStatus
    {
        ResponseBase Create(ModelOrderStatus obj);
        ResponseBase Update(ModelOrderStatus obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelOrderStatus> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListOrderStatus();
        List<ModelOrderStatus> GetListProduct();
    }
}
