using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllOrderDetailStatus
    {
        ResponseBase Create(ModelOrderDetailStatus obj);
        ResponseBase Update(ModelOrderDetailStatus obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelOrderDetailStatus> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListOrderDetailStatus();
        List<ModelOrderDetailStatus> GetListProduct();


        ResponseBase CreatePrint(ModelOrderDetailStatusPrint obj);
        ResponseBase UpdatePrint(ModelOrderDetailStatusPrint obj);
        ResponseBase DeleteByIdPrint(int id, int userId);
        PagedList<ModelOrderDetailStatusPrint> GetListPrint(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListOrderDetailStatusPrint();
        List<ModelOrderDetailStatusPrint> GetListProductPrint();
    }
}
