using System.Collections.Generic;
using PagedList;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Business.Interface
{
    public interface IBllProduct
    {
        ResponseBase Create(ModelProduct obj);
        ResponseBase Update(ModelProduct obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelProduct> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListProduct(int productTypeId);
    }
}
