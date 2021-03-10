using System;
using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
using System.Threading.Tasks;

namespace VINASIC.Business.Interface
{
    public interface IBllOrder
    {
        PagedList<ModelOrder> GetList(int listEmployee, int startIndexRecord, int pageSize, string sorting, string fromDate, string toDate, int employee,string keyword, float orderStatus = -1);

        PagedList<ModelViewDetail> GetListViewDetail(string keyWord, int startIndexRecord, int pageSize, string sorting, int orderId);
        List<ModelOrderDetail> GetListOrderDetailByOrderId(int orderId);
        ResponseBase UpdateApproval(int orderId,bool isAppvroval, int userId);
        ResponseBase UpdateDelivery(int orderId, int status, int userId);
        ResponseBase UpdatePayment(int orderId,float payment,int paymentType, int userId,string tranferDescription);
        ResponseBase UpdateHasTax(int orderId, int id, int userId);
        ResponseBase UpdatedOrder(ModelSaveOrder obj,int userId,bool isAdmin);
        ResponseBase CreateOrder(ModelSaveOrder obj,int userId);
        ResponseBase UpdatePrintUser(int detailId, int printId,string description);
        ResponseBase UpdateHaspay(int orderId,string haspay);
        ResponseBase UpdateDesignUser(int detailId, int printId,string description);
        List<ModelViewDetail> ExportReport(DateTime fromDate, DateTime toDate, int employee, string keyWord, int delivery = 0, int paymentStatus = 0,int type=0,List<int> orderIds=null);
        ResponseBase DeleteById(int id, int userId,bool isAdmin);
        ResponseBase UpdateHaspayCustom(int orderId, string haspay, int paymentType);
        List<ModelViewDetail> GetOrderComplex(int orderId);
        double GetPriceForCustomerAndProduct(int customerId, int productId);
        //ResponseBase UpdateOrderStatusAsync(int orderId, float status, int userId,bool isAdmin);
        ResponseBase UpdateDetailStatus(int detailId, int status, int employeeId,string content);
        ResponseBase UpdateDetailStatus2(int detailId, int status, int employeeId);
        ResponseBase DesignUpdateOrderDetail(int orderId, string fileName, string description);
        //ResponseBase UpdateOrderStatus(int orderId, float status, int userId, bool isAdmin);
        ResponseBase UpdateOrderStatus(int orderId, float status, int userId, bool isAdmin, bool sendSMS = false, bool sendEmail = false);
        ResponseBase GetJobDescriptionForEmployee(int detailId, int status, int employeeId, string content);
        //ResponseBase UpdateOrderStatus(int orderId, float status, int userId, bool isAdmin);
        ResponseBase UpdateCost(List<CostObj> costObj, int orderId, float haspay);
    }
}