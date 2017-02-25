using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllMaterial
    {
        ResponseBase Create(ModelMaterial obj);
        ResponseBase Update(ModelMaterial obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelMaterial> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListMaterial(int productTypeId);
    }
}
