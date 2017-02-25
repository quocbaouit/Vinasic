using System;
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
        public JsonResult GetPaymentVouchers(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listPaymentVoucher = _bllPaymentVoucher.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listPaymentVoucher;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listPaymentVoucher.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }

        public JsonResult SavePaymentVoucher(ModelPaymentVoucher modelPaymentVoucher)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelPaymentVoucher.Id == 0)
                    {
                        modelPaymentVoucher.CreatedUser = UserContext.UserID;
                        responseResult = _bllPaymentVoucher.Create(modelPaymentVoucher);
                    }
                    else
                    {
                        modelPaymentVoucher.UpdatedUser = UserContext.UserID;
                        responseResult = _bllPaymentVoucher.Update(modelPaymentVoucher);
                    }
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
