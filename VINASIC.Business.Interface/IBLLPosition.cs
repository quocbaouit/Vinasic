using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllPosition
    {
        ResponseBase Create(ModelPosition obj);
        ResponseBase Update(ModelPosition obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelPosition> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListPosition();
        List<ModelPosition> GetListProduct();
    }
}
