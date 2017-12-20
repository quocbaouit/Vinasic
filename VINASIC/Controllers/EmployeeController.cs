using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VINASIC.Business.Interface;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using VINASIC.Business.Interface.Model;
using VINASIC.Hubs;
using Dynamic.Framework.Mvc;
using VINASIC.Models;
using System.Threading;

namespace VINASIC.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IBllEmployee _bllEmployee;
        private readonly IBLLRole _bllRole;
        private readonly IBllPosition _bllPosition;
        public EmployeeController(IBllEmployee bllEmployee, IBllPosition bllPosition, IBLLRole bllRole)
        {
            _bllEmployee = bllEmployee;
            _bllRole = bllRole;
            _bllPosition = bllPosition;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DesignIndex()
        {
            return View();
        }

        public ActionResult PrintIndex()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetEmployees(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listEmployee = _bllEmployee.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listEmployee;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listEmployee.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetJobForDesign(string keyword, int jtStartIndex, int jtPageSize, string jtSorting, string fromDate = "", string toDate = "", int employee=0)
        {
            try
            {

                IsAuthenticate = UserContext.Permissions.Contains("isAdmin");
                var listEmployee = _bllEmployee.GetListForDesign(keyword, jtStartIndex, jtPageSize, jtSorting, UserContext.UserID, fromDate, toDate,IsAuthenticate, employee);
                JsonDataResult.Records = listEmployee;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listEmployee.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        [HttpPost]
        public JsonResult GetListDetailForBusiness(string keyword, int jtStartIndex, int jtPageSize, string jtSorting, string fromDate = "", string toDate = "", int employee = 0)
        {
            try
            {

                IsAuthenticate = UserContext.Permissions.Contains("isAdmin");
                var listEmployee = _bllEmployee.GetListDetailForBusiness(keyword, jtStartIndex, jtPageSize, jtSorting, UserContext.UserID, fromDate, toDate, IsAuthenticate, employee);
                JsonDataResult.Records = listEmployee;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listEmployee.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        public JsonResult ResetPass(int empId)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllEmployee.ResetPass(empId);
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
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        [HttpPost]
        public JsonResult GetJobForPrint(string keyword, int jtStartIndex, int jtPageSize, string jtSorting, string fromDate = "", string toDate = "", int employee = 0)
        {
            try
            {
                var auth = UserContext.Permissions.Contains("isAdmin");
                var listEmployee = _bllEmployee.GetListForPrint(keyword, jtStartIndex, jtPageSize, jtSorting, UserContext.UserID, fromDate, toDate, auth, employee);
                JsonDataResult.Records = listEmployee;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listEmployee.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        public JsonResult UpdateLock(int userId, bool isLock)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllEmployee.UpdateLock(userId, isLock, UserContext.UserID);
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
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        [HttpPost]
        public JsonResult SaveEmployee(ModelUser modelEmployee)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelEmployee.Id == 0)
                    {
                        modelEmployee.CreatedUser = UserContext.UserID;
                        responseResult = _bllEmployee.Create(modelEmployee);
                    }
                    else
                    {
                        modelEmployee.UpdatedUser = UserContext.UserID;
                        responseResult = _bllEmployee.Update(modelEmployee);
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
        public JsonResult DeleteEmployee(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllEmployee.DeleteById(id, UserContext.UserID);
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
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Delete ", Message = "Tài Khoản của bạn không có quyền này." });
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
        public JsonResult DesignUpdateOrderDeatail(int id, int status)
        {
            if (IsAuthenticate)
            {
                try
                {
                    var result = _bllEmployee.DesignUpdateOrderDeatail(id, status, UserContext.UserID,UserContext.EmployeeName);
                    JsonDataResult.Records = result;
                    JsonDataResult.Result = JsonDataResult.Result = result.IsSuccess ? JsonDataResult.Result = "OK" : JsonDataResult.Result = "ERROR";

                }
                catch (Exception ex)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

                }
            }
            else
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
            }
            return Json(JsonDataResult);
        }
        [HttpPost]
        public JsonResult PrintUpdateOrderDeatail(int id, int status)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var result = _bllEmployee.PrintUpdateOrderDeatail(id, status, UserContext.UserID,UserContext.EmployeeName);
                    JsonDataResult.Records = result;
                    JsonDataResult.Result = result.IsSuccess ? JsonDataResult.Result = "OK" : JsonDataResult.Result = "ERROR";
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
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        public JsonResult GetEmployeePosition()
        {
            try
            {
                List<SelectListItem> listValues = new List<SelectListItem>();
                var listProductType = _bllPosition.GetListPosition();
                if (listProductType != null)
                {
                    listValues = listProductType.Select(c => new SelectListItem()
                    {
                        Value = c.Value.ToString(),
                        Text = c.Name
                    }).ToList();
                }
                return Json(listValues, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult GetRoleIdByUserId(int userId)
        {
            try
            {
                var RoleIds = _bllRole.GetListRoleByUser(userId).Select(x => x.Id);
                JsonDataResult.Records = RoleIds;
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        [HttpPost]
        public JsonResult GetProductIdByUserId(int userId)
        {
            try
            {
                var productIds = _bllEmployee.GetProductByUserId(userId);
                JsonDataResult.Records = productIds;
                JsonDataResult.Result = "OK";
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        public JsonResult GetProductForUser(string keyword, string jtSorting, int userId = 0)
        {
            try
            {

                var listProduct = _bllEmployee.ListProductIdByUser(userId, keyword, 1, 1000, jtSorting);
                JsonDataResult.Records = listProduct;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listProduct.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult GetSimpleEmployee()
        {
            try
            {
                Thread.Sleep(200);
                var employees = _bllEmployee.GetSimpleEmployee();
                return Json(new { Result = "OK", Records = employees });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        [HttpPost]
        public JsonResult SaveUserProduct(ListSelectProductModel userRole)
        {
            try
            {
              
                    var responseResult = _bllEmployee.UpdateUserProduct(userRole.UserId, userRole.ListSelectProduct);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
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
