using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllSysStatus
    {
        ResponseBase Create(ModelSysStatus obj);
        ResponseBase Update(ModelSysStatus obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelSysStatus> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListSysStatus();
        List<ModelSysStatus> GetListProduct();
    }
}
