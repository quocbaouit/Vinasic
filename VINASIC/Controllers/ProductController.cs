using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IBllProduct _bllProduct;
        private readonly IBllProductType _bllProductType;
        public ProductController(IBllProduct bllProduct, IBllProductType bllProductType)
        {
            _bllProduct = bllProduct;
            _bllProductType = bllProductType;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetProducts(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var listProduct = _bllProduct.GetList(keyword, jtStartIndex, jtPageSize, jtSorting);
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

        public JsonResult SaveProduct(ModelProduct modelProduct)
        {
            try
            {
                if (IsAuthenticate)
                {
                    ResponseBase responseResult;
                    if (modelProduct.Id == 0)
                    {
                        modelProduct.CreatedUser = UserContext.UserID;
                        responseResult = _bllProduct.Create(modelProduct);
                    }
                    else
                    {
                        modelProduct.UpdatedUser = UserContext.UserID;
                        responseResult = _bllProduct.Update(modelProduct);
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
        public JsonResult DeleteProduct(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllProduct.DeleteById(id, UserContext.UserID);
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
        public JsonResult GetProductType()
        {
            try
            {
                List<SelectListItem> listValues = new List<SelectListItem>();
                var listProductType = _bllProductType.GetListProductType();
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
