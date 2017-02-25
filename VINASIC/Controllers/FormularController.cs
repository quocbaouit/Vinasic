using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class FormularController : BaseController
    {
        private readonly IBllFormular _bllFormular;
        public FormularController(IBllFormular bllFormular)
        {
            _bllFormular = bllFormular;           
        }
        public ActionResult Index()
        {         
            return View();
        }
        [HttpPost]
        public JsonResult GetFormulars(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listFormular = _bllFormular.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listFormular;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listFormular.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
     
        public JsonResult SaveFormular(ModelFormular modelFormular)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelFormular.Id == 0)
                    {
                        modelFormular.CreatedUser = UserContext.UserID;
                        responseResult = _bllFormular.Create(modelFormular);
                    }
                    else
                    {
                        modelFormular.UpdatedUser = UserContext.UserID;
                        responseResult = _bllFormular.Update(modelFormular);                    
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
        public JsonResult DeleteFormular(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllFormular.DeleteById(id, UserContext.UserID);
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
