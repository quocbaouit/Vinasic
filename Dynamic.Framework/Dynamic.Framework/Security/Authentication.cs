using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dynamic.Framework.Generic;

namespace Dynamic.Framework.Security
{
    public class Authentication
    {
        public static bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        public static int UserId
        {
            get
            {
                return User.UserID;
            }
        }

        public static int? CompanyID
        {
            get
            {
                return User.CompanyID;
            }
        }

        public static string EmployeeName
        {
            get
            {
                if (string.IsNullOrEmpty(User.EmployeeName))
                    return "Chưa cập nhật";
                return string.Format("{0}", User.EmployeeName);
            }
        }

        public static bool IsOwner
        {
            get
            {
                return User.IsOwner;
            }
        }

        public static IUserService User
        {
            get
            {
                return UserOnline.FirstOrDefault(p => p.SesssionId == HttpContext.Current.Session.SessionID) ?? null;
            }
        }

        public static List<IUserService> UserOnline
        {
            get
            {
                if (HttpContext.Current == null)
                    return new List<IUserService>();
                List<IUserService> list = (List<IUserService>)HttpContext.Current.Application["APP_AUTHENTICATION"];
                if (list == null)
                {
                    list = new List<IUserService>();
                    HttpContext.Current.Application["APP_AUTHENTICATION"] = list;
                }
                return list;
            }
            private set
            {
                HttpContext.Current.Application["APP_AUTHENTICATION"] = value;
            }
        }

        public static bool CheckUserOnline(string email)
        {
            return UserOnline.FirstOrDefault(p => p.Email == email) != null;
        }

        public static void Logout()
        {
            if (!IsAuthenticated)
                return;
            if (HttpContext.Current.Application["APP_MENU_DISPLAY" + User.UserID] != null)
                HttpContext.Current.Application["APP_MENU_DISPLAY" + User.UserID] = null;
            foreach (IUserService userService in UserOnline.Where(p => p.UserID == User.UserID).ToList())
                UserOnline.Remove(userService);
            HttpContext.Current.Session["SS_AUTHENTICATION"] = null;
            HttpContext.Current.Session.Abandon();
        }

        public static void LogOutOther(string email)
        {
            foreach (IUserService userService in UserOnline.Where(p => p.Email == email).ToList())
                UserOnline.Remove(userService);
        }

        public static void ChangeStoreWoking(int StoreID)
        {
            if (!IsAuthenticated)
                return;
            User.StoreID = StoreID;
        }

        public static void Login(int userId)
        {
            if (IsAuthenticated)
                Logout();
            var membershipService = HttpContext.Current.Application["IMEMBERSHIP_SERVICE"] as IMembershipService;
            if (membershipService != null)
            {
                IUserService objUser = membershipService.GetUserService(userId);
                if (objUser == null)
                    return;
                foreach (IUserService userService in UserOnline.Where(p => p.UserID == objUser.UserID).ToList())
                    UserOnline.Remove(userService);
                objUser.SesssionId = HttpContext.Current.Session.SessionID;
                UserOnline.Add(objUser);
                HttpContext.Current.Session["SS_AUTHENTICATION"] = objUser;
            }
        }

        public static IEnumerable<IPermissionService> GetPermissionByFeatureName(string featureName)
        {
            if (!IsAuthenticated)
                return null;
            IUserService user = User;
            lock (user.State)
            {
                if (user.PermissionServices == null)
                    user.PermissionServices = new List<IPermissionService>();
                if (Enumerable.FirstOrDefault<int>(Enumerable.Select(Enumerable.Where(user.PermissionServices, (p => p.FeatureName.ToLower() == featureName.ToLower())),(p => p.FeatureId))) > 0)
                    return Enumerable.Where(user.PermissionServices, p => p.FeatureName.ToLower() == featureName.ToLower());
                IPermissionService[] local_4 = (HttpContext.Current.Application["IMEMBERSHIP_SERVICE"] as IMembershipService).GetPermissionService(featureName);
                user.PermissionServices.AddRange(local_4);
                return local_4;
            }
        }

        public static bool IsPermission(string permissionId)
        {
            return User.Permissions.Contains(permissionId);
        }

        public static bool IsPermission(PermissionType permissionType, string featureName)
        {
            IPermissionService permissionService = GetPermissionByFeatureName(featureName).FirstOrDefault(p => (PermissionType)p.PermissionTypeId == permissionType);
            if (permissionService == null)
                return false;
            return IsPermission(permissionService.PermissionId);
        }

        public static bool IsPermission(PermissionType permissionType, Type type, string area = "")
        {
            string featureName = type.Name.Replace("Controller", "");
            if (!string.IsNullOrEmpty(area))
                featureName = area + featureName;
            return IsPermission(permissionType, featureName);
        }
    }
}
