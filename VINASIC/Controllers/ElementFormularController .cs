using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class ElementFormularController : BaseController
    {
        private readonly IBllElementFormular _bllElementFormular;
        public ElementFormularController(IBllElementFormular bllElementFormular)
        {
            _bllElementFormular = bllElementFormular;           
        }
        public ActionResult Index()
        {         
            return View();
        }
        [HttpPost]
        public JsonResult GetElementFormulars(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listElementFormular = _bllElementFormular.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listElementFormular;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listElementFormular.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
     
        public JsonResult SaveElementFormular(ModelElementFormular modelElementFormular)
        {
            //try
            //{
            //    if (IsAuthenticate)
            //    {
            //        ResponseBase responseResult;
            //        if (modelElementFormular.Id == 0)
            //        {
            //            modelElementFormular.CreatedUser = UserContext.UserID;
            //            responseResult = _bllElementFormular.Create(modelElementFormular);
            //        }
            //        else
            //        {
            //            modelElementFormular.UpdatedUser = UserContext.UserID;
            //            responseResult = _bllElementFormular.Update(modelElementFormular);                    
            //        }
            //        if (!responseResult.IsSuccess)
            //        {
            //            JsonDataResult.Result = "ERROR";
            //            JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
            //        }
            //        else
            //        {
            //            JsonDataResult.Result = "OK";
            //        }
            //    }
            //    else
            //    {
            //        JsonDataResult.Result = "ERROR";
            //        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //add error
            //    JsonDataResult.Result = "ERROR";
            //    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Lỗi: " + ex.Message });
            //}
            return Json(null);
        }

        [HttpPost]
        public JsonResult DeleteElementFormular(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllElementFormular.DeleteById(id, UserContext.UserID);
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
