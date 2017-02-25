using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class PositionController : BaseController
    {
        private readonly IBllPosition _bllPosition;
        private readonly IBllOrganization _bllOrganization;
        public PositionController(IBllPosition bllPosition, IBllOrganization bllOrganization)
        {
            _bllPosition = bllPosition;
            _bllOrganization = bllOrganization;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetPositions(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listPosition = _bllPosition.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listPosition;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listPosition.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }

        public JsonResult SavePosition(ModelPosition modelPosition)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelPosition.Id == 0)
                    {
                        modelPosition.CreatedUser = UserContext.UserID;
                        responseResult = _bllPosition.Create(modelPosition);
                    }
                    else
                    {
                        modelPosition.UpdatedUser = UserContext.UserID;
                        responseResult = _bllPosition.Update(modelPosition);
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
        public JsonResult DeletePosition(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllPosition.DeleteById(id, UserContext.UserID);
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
        public JsonResult GetOrganization()
        {
            try
            {
                List<SelectListItem> listValues = new List<SelectListItem>();
                var listProductType = _bllOrganization.GetListOrganization();
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

    }
}
