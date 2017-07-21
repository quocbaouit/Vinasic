using System;
using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllOrder
    {
        PagedList<ModelOrder> GetList(int listEmployee, int startIndexRecord, int pageSize, string sorting, string fromDate, string toDate, int employee,string keyword, float orderStatus = -1);

        PagedList<ModelViewDetail> GetListViewDetail(string keyWord, int startIndexRecord, int pageSize, string sorting, int orderId);
        List<ModelOrderDetail> GetListOrderDetailByOrderId(int orderId);
        ResponseBase UpdateApproval(int orderId,bool isAppvroval, int userId);
        ResponseBase UpdateDelivery(int orderId, int status, int userId);
        ResponseBase UpdatePayment(int orderId,float payment,int paymentType, int userId);
        ResponseBase UpdateHasTax(int orderId, int id, int userId);
        ResponseBase UpdatedOrder(ModelSaveOrder obj,int userId);
        ResponseBase CreateOrder(ModelSaveOrder obj,int userId);
        ResponseBase UpdatePrintUser(int detailId, int printId,string description);
        ResponseBase UpdateHaspay(int orderId,string haspay);
        ResponseBase UpdateDesignUser(int detailId, int printId,string description);
        List<ModelViewDetail> ExportReport(DateTime fromDate, DateTime toDate, int employee, string keyWord, int delivery = 0, int paymentStatus = 0);
        ResponseBase DeleteById(int id, int userId);
        ResponseBase UpdateHaspayCustom(int orderId, string haspay, int paymentType);
        List<ModelViewDetail> GetOrderComplex(int orderId);
        double GetPriceForCustomerAndProduct(int customerId, int productId);
        ResponseBase UpdateOrderStatus(int orderId, float status, int userId);
        ResponseBase UpdateDetailStatus(int detailId, int status);
    }
}