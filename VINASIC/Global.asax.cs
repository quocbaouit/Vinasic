using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Dynamic.Framework;
using Dynamic.Framework.Security;
using VINASIC.App_Start;
using VINASIC.Business.Interface;
using VINASIC.App_Global;
using  VINASIC;
namespace VINASIC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
           
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.Run();
            //ScheduledTask.Start();
            var membership = Application[Constant.IMEMBERSHIP_SERVICE] as IPermissionService;
            if (membership == null)
            {
                Application[Constant.IMEMBERSHIP_SERVICE] = new InnerMembershipService();
            }

            var encryptor = Application[Constant.GETENCRYPTOR] as IEncryptor;
            if (encryptor == null)
            {
                Application[Constant.GETENCRYPTOR] = new InnerEncryptor();
            }
        }

        protected void Application_End()
        {
            Dynamic.Framework.Security.Authentication.Logout();
        }

        protected void Session_End()
        {
            Authentication.Logout();

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        }

        public class InnerMembershipService : IMembershipService
        {
            public IUserService GetUserService(int userId)
            {
                var bllUser = DependencyResolver.Current.GetService<IBLLUser>();
                return new InnerUserService(bllUser.GetUserService(userId));
            }

            public IPermissionService[] GetPermissionService(string featureName)
            {
                return null;
            }
        }

        public class InnerUserService : VINASIC.Business.Interface.Model.UserService, IUserService
        {
            public InnerUserService(VINASIC.Business.Interface.Model.UserService userService)
            {
                this.employeeId = userService.employeeId;
                this.CompanyID = userService.CompanyID;
                this.CompanyName = userService.CompanyName;
                this.DepartmentName = userService.DepartmentName;
                this.Description = userService.Description;
                this.Email = userService.Email;
                this.EmployeeName = userService.EmployeeName;
                this.Features = userService.Features;
                this.ImagePath = userService.ImagePath;
                this.IsOwner = userService.IsOwner;
                this.LogoCompany = userService.LogoCompany;
                this.Permissions = userService.Permissions;
                this.UserID = userService.UserID;
                this.RoleID = userService.RoleID;
                State = new object();
            }
            public int StoreID { get; set; }
            public List<IPermissionService> PermissionServices { get; set; }
            public string SesssionId { get; set; }
            public object State { get; set; }
        }

        private class InnerEncryptor : IEncryptor
        {
            public string Encrypt(byte[] data)
            {
                return SerializeObject.Encrypt(data);
            }

            public byte[] Decrypt(string data)
            {
                return SerializeObject.Decrypt(data);
            }

            public object Deserialize(byte[] bytes)
            {
                return SerializeObject.Deserialize(bytes);
            }


            public byte[] Serialize(object obj)
            {
                return SerializeObject.Serialize(obj);
            }
        }
    }
}