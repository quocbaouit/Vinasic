
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VINASIC.Object;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Controllers;
using Dynamic.Framework.Mvc;
using VINASIC.Models;

namespace SystemAccount.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IBLLRole ibllRole;
        private readonly IBLLUserRole bllUserRole;
        private readonly IBLLRolePermission bllRolePermission;
        public RoleController(IBLLRole _ibllRole, IBLLRolePermission _bllRolePermission, IBLLUserRole _bllUserRole)
        {
            this.ibllRole = _ibllRole;
            this.bllRolePermission = _bllRolePermission;
            this.bllUserRole = _bllUserRole;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetRoles(string keyWord, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "")
        {
            try
            {
                //if (!IsAuthenticate)
                //{
                var roles = ibllRole.GetListRole(keyWord, jtStartIndex, jtPageSize, jtSorting, UserContext.UserID, UserContext.CompanyID ?? 0, UserContext.IsOwner);
                JsonDataResult.Records = roles;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = roles.TotalItemCount;
                //}
            }
            catch (Exception ex)
            {
                //add Error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        [HttpPost]
        public JsonResult GetRolesForUser(string keyWord, string jtSorting = "", int UserId = 0)
        {
            try
            {
                //if (!IsAuthenticate)
                //{
                var roles = ibllRole.GetListRoleForUser(keyWord, 0, 1000, jtSorting, UserId);
                JsonDataResult.Records = roles;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = roles.TotalItemCount;
                //}
            }
            catch (Exception ex)
            {
                //add Error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        public JsonResult CreateRole(ModelRole modelRole)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelRole.Id == 0)
                    {
                        modelRole.CreatedUser = UserContext.UserID;
                        responseResult = ibllRole.Create(modelRole);
                    }
                    else
                    {
                        modelRole.UpdatedUser = UserContext.UserID;
                        responseResult = ibllRole.Update(modelRole);
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
        public JsonResult Delete(int id)
        {
            ResponseBase responseResult;
            try
            {
                if (IsAuthenticate)
                {
                    responseResult = new ResponseBase();
                    responseResult = ibllRole.DeleteById(id, UserContext.UserID);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                    JsonDataResult.Result = "OK";
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }
            }
            catch (Exception ex)
            {
                //add Error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult SaveRolePermission(RolePermissionModel rolePermission)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = bllRolePermission.UpdateRolePermision(rolePermission.RoleId, rolePermission.ListPermission);
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
        public JsonResult SaveUserRole(UserRoleModel userRole)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = bllUserRole.UpdateUserRole(userRole.UserId, userRole.ListRole);
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
