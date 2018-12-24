using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllPaymentVoucher
    {
        //ResponseBase Create(ModelPaymentVoucher obj);
        //ResponseBase Update(ModelPaymentVoucher obj);
        ResponseBase UpdatedOrder(ModelSavePaymentVoucher obj, int userId, bool isAdmin);
        ResponseBase CreateOrder(ModelSavePaymentVoucher obj, int userId);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelPaymentVoucher> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListPaymentVoucher();
        List<ModelPaymentVoucher> GetListProduct();
        List<ModelOrderDetail> GetListOrderDetailByOrderId(int orderId);
    }
}