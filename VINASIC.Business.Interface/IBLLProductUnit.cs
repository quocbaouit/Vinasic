using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllProductUnit
    {
        ResponseBase Create(ModelProductUnit obj);
        ResponseBase Update(ModelProductUnit obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelProductUnit> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListProductUnit();
        List<ModelProductUnit> GetListProduct();
    }
}
