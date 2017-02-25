using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBLLCommodity
    {
        ResponseBase Create(ModelCommodity obj);
        ResponseBase Update(ModelCommodity obj);
        ResponseBase DeleteById(int id, int userId);
        ResponseBase DeleteByListId(List<int> listId, int userId);
        PagedList<ModelCommodity> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListCommodity();
        List<ModelCommodity> GetListProduct();
    }
}