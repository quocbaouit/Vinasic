using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class ContentPageController : BaseController
    {
        private readonly IBllContent _bllContent;
        public ContentPageController(IBllContent bllContent)
        {
            _bllContent = bllContent;           
        }
        public ActionResult Index()
        {         
            return View();
        }
        public ActionResult IndexView(int? code=null)
        {
            int requestCode = code ?? 1;
            ViewBag.Type = requestCode;
            return View();
        }
        [HttpPost]
        public JsonResult GetContents(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listContent = _bllContent.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listContent;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listContent.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
     
        public JsonResult SaveContent(ModelContent modelContent)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
         
                        modelContent.UpdatedUser = UserContext.UserID;
                        responseResult = _bllContent.Update(modelContent);                    
                 
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
        public JsonResult GetContent(int code)
        {
            try
            {
                var content = _bllContent.GetContentByType(code);
                return Json(new { Result = "OK", Records = content });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteContent(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllContent.DeleteById(id, UserContext.UserID);
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
