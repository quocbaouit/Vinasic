using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class StandardSaleController : BaseController
    {
        private readonly IBllStandardSale _bllStandardSale;
        public StandardSaleController(IBllStandardSale bllStandardSale)
        {
            _bllStandardSale = bllStandardSale;           
        }
        public ActionResult Index()
        {         
            return View();
        }
        [HttpPost]
        public JsonResult GetStandardSales(string keyword, string jtSorting)
        {
            try
            {

                var listStandardSale = _bllStandardSale.GetList(keyword, 1, 100, jtSorting);
                JsonDataResult.Records = listStandardSale;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listStandardSale.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
     
        public JsonResult SaveStandardSale(ModelStandardSale modelStandardSale)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelStandardSale.Id == 0)
                    {
                        modelStandardSale.CreatedUser = UserContext.UserID;
                        responseResult = _bllStandardSale.Create(modelStandardSale);
                    }
                    else
                    {
                        modelStandardSale.UpdatedUser = UserContext.UserID;
                        responseResult = _bllStandardSale.Update(modelStandardSale);                    
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
        public JsonResult DeleteStandardSale(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllStandardSale.DeleteById(id, UserContext.UserID);
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
