using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllMachine
    {
        ResponseBase Create(ModelMachine obj);
        ResponseBase Update(ModelMachine obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelMachine> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListMachine();
        List<ModelMachine> GetListProduct();
    }
}
