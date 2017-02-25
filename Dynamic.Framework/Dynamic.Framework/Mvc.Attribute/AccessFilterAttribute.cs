using Dynamic.Framework.Generic;
using Dynamic.Framework.Security;
using System.Web.Mvc;

namespace Dynamic.Framework.Mvc.Attribute
{
    public class AccessFilterAttribute : ActionFilterAttribute
    {
        public AllowAccess AllowAccess { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!Authentication.IsAuthenticated)
            {
                filterContext.Result = (ActionResult)new RedirectResult("/");
            }
            else
            {
                switch (this.AllowAccess)
                {
                    case AllowAccess.Administrator:
                        if (Authentication.User.CompanyID.HasValue)
                        {
                            filterContext.Result = (ActionResult)new RedirectResult("/");
                            break;
                        }
                        break;
                    case AllowAccess.Company:
                        if (!Authentication.User.CompanyID.HasValue)
                        {
                            filterContext.Result = (ActionResult)new RedirectResult("/");
                            break;
                        }
                        break;
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }
}
