using Dynamic.Framework.Generic;
using Dynamic.Framework.Security;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Dynamic.Framework.Mvc
{
    public class ControllerCore : Controller
    {
        public List<Error> ErrorMessages { get; set; }

        public JsonDataResult JsonDataResult { get; set; }

        public string Layout { get; set; }

        public IUserService UserContext
        {
            get
            {
                return Authentication.User;
            }
        }

        public ControllerCore()
        {
            this.JsonDataResult = new JsonDataResult();
            this.ErrorMessages = new List<Error>();
            this.JsonDataResult.Result = "OK";
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}
