using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class OrderStatusController : BaseController
    {
        private readonly IBllOrderStatus _bllOrderStatus;
        public OrderStatusController(IBllOrderStatus bllOrderStatus)
        {
            _bllOrderStatus = bllOrderStatus;           
        }
        public ActionResult Index()
        {         
            return View();
        }
        [HttpPost]
        public JsonResult GetOrderStatuss(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listOrderStatus = _bllOrderStatus.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listOrderStatus;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listOrderStatus.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
     
        public JsonResult SaveOrderStatus(ModelOrderStatus modelOrderStatus)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelOrderStatus.Id == 0)
                    {
                        modelOrderStatus.CreatedUser = UserContext.UserID;
                        responseResult = _bllOrderStatus.Create(modelOrderStatus);
                    }
                    else
                    {
                        modelOrderStatus.UpdatedUser = UserContext.UserID;
                        responseResult = _bllOrderStatus.Update(modelOrderStatus);                    
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
        public JsonResult DeleteOrderStatus(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrderStatus.DeleteById(id, UserContext.UserID);
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
