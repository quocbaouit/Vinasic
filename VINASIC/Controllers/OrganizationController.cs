using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class OrganizationController : BaseController
    {
        private readonly IBllOrganization _bllOrganization;
        public OrganizationController(IBllOrganization bllOrganization)
        {
            _bllOrganization = bllOrganization;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetOrganizations(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listOrganization = _bllOrganization.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listOrganization;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listOrganization.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }

        public JsonResult SaveOrganization(ModelOrganization modelOrganization)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelOrganization.Id == 0)
                    {
                        modelOrganization.CreatedUser = UserContext.UserID;
                        responseResult = _bllOrganization.Create(modelOrganization);
                    }
                    else
                    {
                        modelOrganization.UpdatedUser = UserContext.UserID;
                        responseResult = _bllOrganization.Update(modelOrganization);
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
        public JsonResult DeleteOrganization(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrganization.DeleteById(id, UserContext.UserID);
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
