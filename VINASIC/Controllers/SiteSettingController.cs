using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class SiteSettingController : BaseController
    {
        private readonly IBllSiteSetting _bllSiteSetting;
        public SiteSettingController(IBllSiteSetting bllSiteSetting)
        {
            _bllSiteSetting = bllSiteSetting;           
        }
        public ActionResult Index()
        {         
            return View();
        }
        [HttpPost]
        public JsonResult GetSiteSettings(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listSiteSetting = _bllSiteSetting.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listSiteSetting;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listSiteSetting.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
     
        public JsonResult SaveSiteSetting(ModelSiteSetting modelSiteSetting)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelSiteSetting.Id == 0)
                    {
                        modelSiteSetting.CreatedUser = UserContext.UserID;
                        responseResult = _bllSiteSetting.Create(modelSiteSetting);
                    }
                    else
                    {
                        modelSiteSetting.UpdatedUser = UserContext.UserID;
                        responseResult = _bllSiteSetting.Update(modelSiteSetting);                    
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
        public JsonResult DeleteSiteSetting(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllSiteSetting.DeleteById(id, UserContext.UserID);
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
