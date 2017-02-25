using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllFormular
    {
        ResponseBase Create(ModelFormular obj);
        ResponseBase Update(ModelFormular obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelFormular> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListFormular();
    }
}
