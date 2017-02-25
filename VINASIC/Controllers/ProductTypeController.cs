using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class ProductTypeController : BaseController
    {
        private readonly IBllProductType _bllProductType;
        public ProductTypeController(IBllProductType bllProductType)
        {
            _bllProductType = bllProductType;           
        }
        public ActionResult Index()
        {         
            return View();
        }
        [HttpPost]
        public JsonResult GetProductTypes(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listProductType = _bllProductType.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listProductType;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listProductType.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
     
        public JsonResult SaveProductType(ModelProductType modelProductType)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelProductType.Id == 0)
                    {
                        modelProductType.CreatedUser = UserContext.UserID;
                        responseResult = _bllProductType.Create(modelProductType);
                    }
                    else
                    {
                        modelProductType.UpdatedUser = UserContext.UserID;
                        responseResult = _bllProductType.Update(modelProductType);                    
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
        public JsonResult DeleteProductType(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllProductType.DeleteById(id, UserContext.UserID);
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
