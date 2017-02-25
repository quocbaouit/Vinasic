using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VINASIC.Business.Interface;
using GPRO.Ultilities.Mail;
using System.Collections;
//using VINASIC.App_Global;
using GPRO.Core;
using System.Web.Routing;
using GPRO.Core.Mvc;
using VINASIC.Business.Interface.Model;

using GPRO.Core.Mvc.Attribute;
using System.Diagnostics;
using GPRO.Core.Security;

namespace VINASIC.Controllers
{
    public class BaseController : ControllerCore
    {
        public string defaultPage = string.Empty;
    }
}
