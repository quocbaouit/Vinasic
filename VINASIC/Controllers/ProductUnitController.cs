using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class ProductUnitController : BaseController
    {
        private readonly IBllProductUnit _bllProductUnit;
        public ProductUnitController(IBllProductUnit bllProductUnit)
        {
            _bllProductUnit = bllProductUnit;           
        }
        public ActionResult Index()
        {         
            return View();
        }
        [HttpPost]
        public JsonResult GetProductUnits(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listProductUnit = _bllProductUnit.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listProductUnit;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listProductUnit.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
     
        public JsonResult SaveProductUnit(ModelProductUnit modelProductUnit)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelProductUnit.Id == 0)
                    {
                        modelProductUnit.CreatedUser = UserContext.UserID;
                        responseResult = _bllProductUnit.Create(modelProductUnit);
                    }
                    else
                    {
                        modelProductUnit.UpdatedUser = UserContext.UserID;
                        responseResult = _bllProductUnit.Update(modelProductUnit);                    
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
        public JsonResult DeleteProductUnit(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllProductUnit.DeleteById(id, UserContext.UserID);
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
