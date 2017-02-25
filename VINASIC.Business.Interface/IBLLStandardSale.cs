using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllStandardSale
    {
        ResponseBase Create(ModelStandardSale obj);
        ResponseBase Update(ModelStandardSale obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelStandardSale> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListStandardSale();
    }
}
