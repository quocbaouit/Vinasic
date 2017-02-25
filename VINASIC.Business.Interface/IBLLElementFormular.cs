using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllElementFormular
    {
        ResponseBase Create(ModelElementFormular obj);
        ResponseBase Update(ModelElementFormular obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelElementFormular> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListElementFormular();
    }
}
