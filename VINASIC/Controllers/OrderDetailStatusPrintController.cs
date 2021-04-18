using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class OrderDetailStatusPrintController : BaseController
    {
        private readonly IBllOrderDetailStatus _bllOrderDetailStatus;
        public OrderDetailStatusPrintController(IBllOrderDetailStatus bllOrderDetailStatus)
        {
            _bllOrderDetailStatus = bllOrderDetailStatus;           
        }
        public ActionResult Index()
        {         
            return View();
        }
        [HttpPost]
        public JsonResult GetOrderDetailStatuss(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listOrderDetailStatus = _bllOrderDetailStatus.GetListPrint(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listOrderDetailStatus;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listOrderDetailStatus.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
     
        public JsonResult SaveOrderDetailStatus(ModelOrderDetailStatusPrint modelOrderDetailStatus)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelOrderDetailStatus.Id == 0)
                    {
                        modelOrderDetailStatus.CreatedUser = UserContext.UserID;
                        responseResult = _bllOrderDetailStatus.CreatePrint(modelOrderDetailStatus);
                    }
                    else
                    {
                        modelOrderDetailStatus.UpdatedUser = UserContext.UserID;
                        responseResult = _bllOrderDetailStatus.UpdatePrint(modelOrderDetailStatus);                    
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
        public JsonResult DeleteOrderDetailStatus(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrderDetailStatus.DeleteByIdPrint(id, UserContext.UserID);
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
