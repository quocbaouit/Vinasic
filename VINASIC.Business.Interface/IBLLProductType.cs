using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllProductType
    {
        ResponseBase Create(ModelProductType obj);
        ResponseBase Update(ModelProductType obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelProductType> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListProductType();
        List<ModelProductType> GetListProduct();
    }
}
