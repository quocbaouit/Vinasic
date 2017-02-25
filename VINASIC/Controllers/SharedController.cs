using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Enum;
using VINASIC.Business.Interface.Model;
using VINASIC.Models;

namespace VINASIC.Controllers
{
    public class SharedController : BaseController
    {

        private readonly IBLLMenuCategory _bllCategory;
        private readonly IBllNotification _bllNotification;
        public SharedController(IBLLMenuCategory bllCategory, IBllNotification bllNotification)
        {
            _bllCategory = bllCategory;
            _bllNotification = bllNotification;
        }
        public ActionResult HeadMasterPartial()
        {
            ViewBag.ClientName = UserContext.EmployeeName;
            ViewBag.ClientId = UserContext.UserID;
            var userNotification = GetListNotification();
            ViewData["UserNotification"] = userNotification;
            return PartialView(UserContext);
        }
        public List<string> GetListNotification()
        {
            var jss = new JavaScriptSerializer();
            var returnValues = _bllNotification.GetListNotification();
            return (from returnValue in returnValues let lstUser = returnValue.ListUser.Split(',') where lstUser.Contains(UserContext.UserID.ToString()) select returnValue.Description).ToList();
        }

        [HttpPost]
        public JsonResult UpdateNotification()
        {
            try
            {
                _bllNotification.UpdateNotification(UserContext.UserID.ToString());
                return Json(new { Result = "OK", Records = "True" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult MenuLeftMasterPartial()
        {
            List<ModelMenuCategory> listMenuCategory = null;
            try
            {
                listMenuCategory = _bllCategory.GetMenusAndMenuCategoriesByUserId(1, eMenuCategoryType.Left);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(listMenuCategory);
        }

        public ActionResult MenuTopMasterPartial()
        {
            return PartialView();
        }

    }
}
