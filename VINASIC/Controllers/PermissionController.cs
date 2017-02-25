using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Models;

namespace VINASIC.Controllers
{
    public class PermissionController : BaseController
    {
        private readonly IBLLPermission _bllPermission;
        public PermissionController(IBLLPermission bllPermission)
        {
            _bllPermission = bllPermission;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetPermissions(string keyword, string jtSorting)
        {
            try
            {

                var listPermission = _bllPermission.GetList(keyword, 1, 1000, jtSorting);
                JsonDataResult.Records = listPermission;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listPermission.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        public JsonResult GetPermissionsForRole(string keyword, string jtSorting, int roleId = 0)
        {
            try
            {

                var listPermission = _bllPermission.GetListPermissionForRole(roleId, keyword, 1, 1000, jtSorting);
                JsonDataResult.Records = listPermission;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listPermission.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }

        public JsonResult SavePermission(ModelPermission modelPermission)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelPermission.Id == 0)
                    {
                        modelPermission.CreatedUser = UserContext.UserID;
                        responseResult = _bllPermission.Create(modelPermission);
                    }
                    else
                    {
                        modelPermission.UpdatedUser = UserContext.UserID;
                        responseResult = _bllPermission.Update(modelPermission);
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
        public JsonResult DeletePermission(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllPermission.DeleteById(id, UserContext.UserID);
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

        [HttpPost]
        public JsonResult GetPermissionIdsByRole(int roleId)
        {
            try
            {
                var listPermission = _bllPermission.GetListPermissinIdbyRoleId(roleId);
                JsonDataResult.Records = listPermission;
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }



    }
}
