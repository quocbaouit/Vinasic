using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IBllCustomer _bllCustomer;
        public CustomerController(IBllCustomer bllCustomer)
        {
            _bllCustomer = bllCustomer;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetCustomers(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listCustomer = _bllCustomer.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listCustomer;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listCustomer.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }

        public JsonResult SaveCustomer(ModelCustomer modelCustomer)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelCustomer.Id == 0)
                    {
                        modelCustomer.CreatedUser = UserContext.UserID;
                        responseResult = _bllCustomer.Create(modelCustomer);
                    }
                    else
                    {
                        modelCustomer.UpdatedUser = UserContext.UserID;
                        responseResult = _bllCustomer.Update(modelCustomer);
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
        public JsonResult DeleteCustomer(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllCustomer.DeleteById(id, UserContext.UserID);
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
