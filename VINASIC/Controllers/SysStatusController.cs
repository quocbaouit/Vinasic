using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class SysStatusController : BaseController
    {
        private readonly IBllSysStatus _bllSysStatus;
        public SysStatusController(IBllSysStatus bllSysStatus)
        {
            _bllSysStatus = bllSysStatus;           
        }
        public ActionResult Index()
        {         
            return View();
        }
        [HttpPost]
        public JsonResult GetSysStatuss(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listSysStatus = _bllSysStatus.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listSysStatus;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listSysStatus.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
     
        public JsonResult SaveSysStatus(ModelSysStatus modelSysStatus)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelSysStatus.Id == 0)
                    {
                        modelSysStatus.CreatedUser = UserContext.UserID;
                        responseResult = _bllSysStatus.Create(modelSysStatus);
                    }
                    else
                    {
                        modelSysStatus.UpdatedUser = UserContext.UserID;
                        responseResult = _bllSysStatus.Update(modelSysStatus);                    
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
        public JsonResult DeleteSysStatus(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllSysStatus.DeleteById(id, UserContext.UserID);
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
