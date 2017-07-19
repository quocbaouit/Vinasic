using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface.Enum;

namespace VINASIC.Controllers
{
    public class BaseController : ControllerCore
    {
        public int UserId = 1;
        public string DefaultPage = string.Empty;
        public bool IsAuthenticate = true;

        protected override void Initialize(RequestContext requestContext)
        {

            var controllerName = requestContext.RouteData.GetRequiredString("controller");
            var actionName = requestContext.RouteData.GetRequiredString("action");
            var accessingResource = "/" + controllerName + "/" + actionName;
            var routeDefault = ((Route)requestContext.RouteData.Route).Defaults;
            if (routeDefault != null)
            {
                var valuesDefault = routeDefault.Values.ToList();
                DefaultPage = "/" + valuesDefault[0] + "/" + valuesDefault[1];
            }
            if (UserContext == null)
            {
                if (!controllerName.Equals("Authenticate"))
                {
                    if (requestContext.HttpContext.Request.IsAjaxRequest())
                    {
                        requestContext.HttpContext.Response.StatusCode = 401;
                        requestContext.HttpContext.Response.End();
                    }
                    else
                        requestContext.HttpContext.Response.Redirect("/Authenticate/Login?Url=" + accessingResource);
                }
                else
                    base.Initialize(requestContext);
            }
            else
            {

                //if (controllerName.Equals("Authenticate") && actionName.Equals("Login"))

                //{

                //    base.Initialize(requestContext);

                //}

                //else if (!controllerName.Equals("Error") && !actionName.Equals("Logout"))

                //{

                //    var permissions = UserContext.Permissions;

                //    var havePermissions = false;

                //    if (permissions.Any())

                //    {

                //        havePermissions = permissions.Select(x => x.Trim().ToLower().Contains(accessingResource.Trim().ToLower())).FirstOrDefault(x => x);

                //    }

                //    if (havePermissions == false)

                //    {

                //        if (requestContext.HttpContext.Request.IsAjaxRequest())

                //        {

                //            JsonDataResult.Result = "ERROR";

                //            JsonDataResult.Message = eErrorMessage.NoPermission;

                //            base.Initialize(requestContext);

                //        }

                //        else

                //            requestContext.HttpContext.Response.Redirect("~/Error/Index?ErrorType=" + (int)eErrorType.NoPermission, true);

                //    }

                //    else

                //    {

                //        if (requestContext.HttpContext.Request.IsAjaxRequest())

                //        {

                //            IsAuthenticate = true;

                //        }

                //        base.Initialize(requestContext);

                //    }

                //}

                //else

                base.Initialize(requestContext);
            }

        }
    }
}
