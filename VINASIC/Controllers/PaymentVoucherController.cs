using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class PaymentVoucherController : BaseController
    {
        private readonly IBllPaymentVoucher _bllPaymentVoucher;
        public PaymentVoucherController(IBllPaymentVoucher bllPaymentVoucher)
        {
            _bllPaymentVoucher = bllPaymentVoucher;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetPaymentVouchers(string keyword, int jtStartIndex, int jtPageSize, string jtSorting, string fromDate = "", string toDate = "",int type=0)
        {
            try
            {

                var listPaymentVoucher = _bllPaymentVoucher.GetList(keyword, jtStartIndex, jtPageSize, jtSorting,fromDate,toDate, type);
                JsonDataResult.Records = listPaymentVoucher;
                dynamic Sum = new System.Dynamic.ExpandoObject();
                var sumHaspay = listPaymentVoucher.Sum(x => x.HasPay);
                var sumSubTotal = listPaymentVoucher.Sum(x => x.Money);
                var sumRemaining = sumSubTotal - (sumHaspay);
                Sum.sumHaspay = sumHaspay;
                Sum.sumRemaining = sumRemaining;
                JsonDataResult.Result = "OK";
                JsonDataResult.Data = Sum;
                JsonDataResult.TotalRecordCount = listPaymentVoucher.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult SaveOrder(int orderId, int employeeId, int customerId, string customerName, string customerPhone, string customerMail, string customerAddress, string customerTaxCode, float orderTotal, List<ModelPaymentVoucherDetail> listDetail, string content, float totalInclude,float haspay)
        {
            try
            {
                var IsAdmin = UserContext.Permissions.Contains("isAdmin");
                if (IsAuthenticate)
                {
                    var payment_type = 0;
                    if (listDetail != null && listDetail.Count>0)
                    {
                        payment_type = 1;
                    }
                    var saveOrder = new ModelSavePaymentVoucher
                    {
                        OrderId = orderId,
                        EmployeeId = employeeId,
                        OrderTotal = orderTotal,
                        CustomerId = customerId,
                        CustomerName = customerName,
                        CustomerPhone = customerPhone,
                        CustomerMail = customerMail,
                        CustomerAddress = customerAddress,
                        CustomerTaxCode = customerTaxCode,
                        Content = content,
                        totalInclude=totalInclude,
                        DateDelivery = DateTime.Now,
                        Detail = listDetail,
                        HasPay=haspay,
                        PaymentType= payment_type,
                    };
                    var responseResult = saveOrder.OrderId == 0 ? _bllPaymentVoucher.CreateOrder(saveOrder, UserContext.UserID) : _bllPaymentVoucher.UpdatedOrder(saveOrder, UserContext.UserID, IsAdmin);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                    else
                    {
                        JsonDataResult.Result = "OK";
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }


            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult ListOrderDetail(int orderId)
        {
            try
            {
                Thread.Sleep(200);
                var orderDetail = _bllPaymentVoucher.GetListOrderDetailByOrderId(orderId);
                return Json(new { Result = "OK", Records = orderDetail });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeletePaymentVoucher(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllPaymentVoucher.DeleteById(id, UserContext.UserID);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Delete BankBranch", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

    }
}
