using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class MaterialTypeController : BaseController
    {
        private readonly IBllMaterialType _bllMaterialType;
        public MaterialTypeController(IBllMaterialType bllMaterialType)
        {
            _bllMaterialType = bllMaterialType;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetMaterialTypes(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listMaterialType = _bllMaterialType.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listMaterialType;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listMaterialType.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }

        public JsonResult SaveMaterialType(ModelMaterialType modelMaterialType)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelMaterialType.Id == 0)
                    {
                        modelMaterialType.CreatedUser = UserContext.UserID;
                        responseResult = _bllMaterialType.Create(modelMaterialType);
                    }
                    else
                    {
                        modelMaterialType.UpdatedUser = UserContext.UserID;
                        responseResult = _bllMaterialType.Update(modelMaterialType);
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
        public JsonResult DeleteMaterialType(int id)
        {
            try
            {
                //if (!IsAuthenticate)
                //{
                    var responseResult = _bllMaterialType.DeleteById(id, UserContext.UserID);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                //}
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
