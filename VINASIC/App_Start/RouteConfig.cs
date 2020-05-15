using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VINASIC
{
    public class SubdomainRoute : RouteBase
    {

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var host = httpContext.Request.Url.Host;
            var index = host.IndexOf(".");
            string[] segments = httpContext.Request.Url.PathAndQuery.Split('.');

            if (index < 0)
                return null;

            var subdomain = host.Substring(0, index);
            string controller = (segments.Length > 0) ? segments[0] : "DashBoard";
            string action = (segments.Length > 1) ? segments[1] : "Index";

            if (subdomain.Contains("sticker"))
            {
                controller = "Sticker";
                action = "Index";
            }
            var routeData = new RouteData(this, new MvcRouteHandler());

            routeData.Values.Add("controller", controller); //Goes to the relevant Controller  class
            routeData.Values.Add("action", action); //Goes to the relevant action method on the specified Controller
            //routeData.Values.Add("subdomain", subdomain); //pass subdomain as argument to action method
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //Implement your formating Url formating here
            return null;
        }
    }
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.Add(new SubdomainRoute());
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Sticker", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}