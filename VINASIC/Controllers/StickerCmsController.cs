using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class StickerCmsController : BaseController
    {
        private readonly IBllSticker _bllStickerCms;
        public StickerCmsController(IBllSticker bllStickerCms)
        {
            _bllStickerCms = bllStickerCms;           
        }
        public ActionResult Index()
        {         
            return View();
        }
        [HttpPost]
        public JsonResult GetStickerCms(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listStickerCms = _bllStickerCms.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listStickerCms;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listStickerCms.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
     
        public JsonResult SaveStickerCms(ModelSticker modelStickerCms)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelStickerCms.Id == 0)
                    {
                        modelStickerCms.CreatedUser = UserContext.UserID;
                        responseResult = _bllStickerCms.Create(modelStickerCms);
                    }
                    else
                    {
                        modelStickerCms.UpdatedUser = UserContext.UserID;
                        responseResult = _bllStickerCms.Update(modelStickerCms);                    
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
        public JsonResult DeleteStickerCms(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllStickerCms.DeleteById(id, UserContext.UserID);
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
