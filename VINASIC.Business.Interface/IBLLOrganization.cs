using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllOrganization
    {
        ResponseBase Create(ModelOrganization obj);
        ResponseBase Update(ModelOrganization obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelOrganization> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListOrganization();
        List<ModelOrganization> GetListProduct();
    }
}
