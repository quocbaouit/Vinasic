using Dynamic.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VINASIC.Business.Interface;

namespace VINASIC.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IBLLDashBoard _bllDashBoard;
        //
        // GET: /Dashboard/
        public DashboardController(IBLLDashBoard bllDashBoard)
        {
            _bllDashBoard = bllDashBoard;

        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetData(string from,string to)
        {
            try
            {
                var result = _bllDashBoard.GetData(from, to);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get Object", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

    }
}
