using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemAccount.App_Global;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Enum;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Controllers
{
    public class SharedController : BaseController
    {
        //private readonly IBLLMenu bllMenu;
        private readonly IBLLMenuCategory bllCategory;
        //private readonly IBLLRole bllRole;

        public SharedController(IBLLMenuCategory _bllCategory)
        {
            //this.bllMenu = _bllMenu;
            this.bllCategory = _bllCategory;
            //this.bllRole = _bllRole;
        }
        public ActionResult HeadMasterPartial()
        {
            //ViewData["Module"] = bllRole.GetListModuleByUserId(UserContext.UserID);

            return PartialView();
        }

        public ActionResult MenuLeftMasterPartial()
        {
            List<ModelMenuCategory> listMenuCategory = null;
            try
            {
                listMenuCategory = bllCategory.GetMenusAndMenuCategoriesByUserId(1, eMenuCategoryType.Left);
            }
            catch (Exception ex)
            {
                // return ErrorPage
                throw ex;
            }
            ViewData["defaultPage"] = defaultPage;
            return PartialView(listMenuCategory);
        }

        public ActionResult MenuTopMasterPartial()
        {
            //List<ModelMenuCategory> listMenuCategory = null;
            //try
            //{
            //    listMenuCategory = bllCategory.GetMenusAndMenuCategoriesByUserId(UserContext.UserID, eMenuCategoryType.Top);
            //}
            //catch (Exception ex)
            //{
            //    // return ErrorPage
            //    throw ex;
            //}
            //return PartialView(listMenuCategory);
            return PartialView();
        }

    }
}
