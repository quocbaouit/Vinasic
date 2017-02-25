using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VINASIC.Business.Interface;
using PagedList;
using Hugate.Framework.Mvc.Attribute;
using GPRO.Core.Generic;
using GPRO.Core.Mvc;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
namespace VINASIC.Controllers
{
    public class ExamController : BaseController
    {
         private readonly IBLLProductType bllProductType;
         private readonly IBLLMenuCategory bllMenu;

         public ExamController(IBLLProductType _bllProductType, IBLLMenuCategory _bllMenu)
            {
                this.bllProductType = _bllProductType;
                this.bllMenu = _bllMenu;
            }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetProductTypes(string keyword, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
             
                    var listProductType = bllProductType.GetList(keyword, jtStartIndex, jtPageSize, jtSorting, 1);
                    JsonDataResult.Records = listProductType;
                    JsonDataResult.Result = "OK";
                    JsonDataResult.TotalRecordCount = listProductType.TotalItemCount;
                
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
