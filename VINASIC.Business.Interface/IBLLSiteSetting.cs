using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllSiteSetting
    {
        ResponseBase Create(ModelSiteSetting obj);
        ResponseBase Update(ModelSiteSetting obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelSiteSetting> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListSiteSetting();
        List<ModelSiteSetting> GetListProduct();
    }
}
