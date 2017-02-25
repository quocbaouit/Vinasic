using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllMaterialType
    {
        ResponseBase Create(ModelMaterialType obj);
        ResponseBase Update(ModelMaterialType obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelMaterialType> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListMaterialType();
        List<ModelMaterialType> GetListProduct();
    }
}
