using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBLLCommodityType
    {
        ResponseBase Create(ModelCommodityType obj);
        ResponseBase Update(ModelCommodityType obj);
        ResponseBase DeleteById(int id, int userId);
        ResponseBase DeleteByListId(List<int> listId, int userId);
        PagedList<ModelCommodityType> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListCommodityType();
        List<ModelCommodityType> GetListProduct();
    }
}