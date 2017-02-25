using Dynamic.Framework.Generic;
using Dynamic.Framework.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Dynamic.Framework.Mvc.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DynamicAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private string[] _permissionSplit = new string[0];
        private string queryStringKey = "token";
        private string _permission;

        public string Permissions
        {
            get
            {
                return this._permission ?? string.Empty;
            }
            set
            {
                this._permission = value;
                this._permissionSplit = DynamicAuthorizeAttribute.SplitString(value);
            }
        }

        public Dynamic.Framework.Generic.PermissionType[] PermissionType { get; set; }

        public CommandPermissionType CommandPermissionType { get; set; }

        public string FeatureName { get; set; }

        private IEncryptor GetEncryptor(AuthorizationContext filterContext)
        {
            return filterContext.HttpContext.Application["GETENCRYPTOR"] as IEncryptor;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(this.FeatureName))
            {
                string str1 = Convert.ToString(filterContext.RequestContext.RouteData.DataTokens["area"]);
                string str2 = Convert.ToString(filterContext.RequestContext.RouteData.Values["Controller"]);
                Convert.ToString(filterContext.RequestContext.RouteData.Values["Action"]);
                this.FeatureName = str1 + str2;
            }
            if (!AjaxRequestExtensions.IsAjaxRequest(filterContext.RequestContext.HttpContext.Request))
            {
                if (!Authentication.IsAuthenticated)
                {
                    string data = filterContext.RequestContext.HttpContext.Request.QueryString.Get(this.queryStringKey);
                    if (data != null)
                    {
                        ArrayList arrayList = (ArrayList)this.GetEncryptor(filterContext).Deserialize(this.GetEncryptor(filterContext).Decrypt(data));
                        int userId = Convert.ToInt32(arrayList[0]);
                        string str1 = Convert.ToString(arrayList[1]);
                        if (filterContext.HttpContext.Session.SessionID == str1 && userId > 0)
                        {
                            Authentication.Login(userId);
                            string rawUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                            string str2 = this.queryStringKey.Trim().ToLower();
                            string url = rawUrl.Trim().ToLower();
                            int startIndex = url.IndexOf("token");
                            if (startIndex >= 0)
                            {
                                string str3 = url.Substring(startIndex);
                                char[] chArray = new char[1]
                {
                  '&'
                };
                                foreach (string oldValue in str3.Split(chArray))
                                {
                                    if (oldValue.Contains(str2))
                                    {
                                        url = url.Replace(oldValue, "").Replace("&&", "&");
                                        if (url.EndsWith("&"))
                                            url = url.Substring(0, url.Length - 1);
                                    }
                                }
                            }
                            if (url.EndsWith("?"))
                                url = url.Substring(0, url.Length - 1);
                            filterContext.Result = (ActionResult)new RedirectResult(url);
                        }
                    }
                    else
                        filterContext.RequestContext.HttpContext.Session["$set-cookie$"] = (object)true;
                }
                if (!string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Request.QueryString.Get(this.queryStringKey)))
                {
                    string rawUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                    string str1 = this.queryStringKey.Trim().ToLower();
                    string url = rawUrl.Trim().ToLower();
                    int startIndex = url.IndexOf("token");
                    if (startIndex >= 0)
                    {
                        string str2 = url.Substring(startIndex);
                        char[] chArray = new char[1]
            {
              '&'
            };
                        foreach (string oldValue in str2.Split(chArray))
                        {
                            if (oldValue.Contains(str1))
                            {
                                url = url.Replace(oldValue, "").Replace("&&", "&");
                                if (url.EndsWith("&"))
                                    url = url.Substring(0, url.Length - 1);
                            }
                        }
                    }
                    if (url.EndsWith("?"))
                        url = url.Substring(0, url.Length - 1);
                    filterContext.Result = (ActionResult)new RedirectResult(url);
                }
            }
            if (!Authentication.IsAuthenticated)
                this.HandleUnAuthenticatedRequest(filterContext);
            else if (!this.AuthorizeCore())
                this.HandleUnauthorizedRequest(filterContext);
        }

        private void HandleUnAuthenticatedRequest(AuthorizationContext filterContext)
        {
            if (AjaxRequestExtensions.IsAjaxRequest(filterContext.HttpContext.Request))
            {
                filterContext.Result = (ActionResult)new JsonResult()
                {
                    Data = (object)new JsonDataResult()
                    {
                        StatusCode = 403,
                        ErrorMessages = {
              new Dynamic.Framework.Mvc.Error()
              {
                Message = "Bạn chưa đăng nhập hoặc phiên làm việc của bạn đã kết thúc"
              }
            },
                        Message = "Bạn chưa đăng nhập hoặc phiên làm việc của bạn đã kết thúc"
                    }
                };
            }
            else
            {
                string url = FormsAuthentication.LoginUrl + "?token=" + HttpUtility.UrlEncode(this.GetEncryptor(filterContext).Encrypt(this.GetEncryptor(filterContext).Serialize((object)new ArrayList()
        {
          (object) filterContext.HttpContext.Session.SessionID,
          (object) filterContext.HttpContext.Request.Url.AbsoluteUri,
          (object) "Login"
        })));
                filterContext.Result = (ActionResult)new RedirectResult(url);
            }
        }

        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (AjaxRequestExtensions.IsAjaxRequest(filterContext.HttpContext.Request))
            {
                filterContext.Result = (ActionResult)new JsonResult()
                {
                    Data = (object)new JsonDataResult()
                    {
                        StatusCode = 401,
                        ErrorMessages = {
              new Dynamic.Framework.Mvc.Error()
              {
                Message = "Bạn không có quyền truy cập dữ liệu"
              }
            },
                        Message = "Bạn không có quyền truy cập dữ liệu"
                    }
                };
            }
            else
            {
                AuthorizationContext authorizationContext = filterContext;
                PartialViewResult partialViewResult1 = new PartialViewResult();
                partialViewResult1.ViewName = "_Unauthorized";
                PartialViewResult partialViewResult2 = partialViewResult1;
                authorizationContext.Result = (ActionResult)partialViewResult2;
            }
        }

        protected virtual bool AuthorizeCore()
        {
            IEnumerable<IPermissionService> permissionByFeatureName = Authentication.GetPermissionByFeatureName(this.FeatureName);
            List<string> list = new List<string>();
            foreach (string str in this._permissionSplit)
            {
                string item = str;
                if (Enumerable.FirstOrDefault<string>(Enumerable.Where<string>((IEnumerable<string>)list, (Func<string, bool>)(p => p == item))) == null)
                    list.Add(item);
            }
            if (this.PermissionType != null && Enumerable.Count<Dynamic.Framework.Generic.PermissionType>((IEnumerable<Dynamic.Framework.Generic.PermissionType>)this.PermissionType) > 0)
            {
                if (this.CommandPermissionType == CommandPermissionType.And)
                {
                    IEnumerable<IPermissionService> source = permissionByFeatureName;
                    foreach (Dynamic.Framework.Generic.PermissionType permissionType in this.PermissionType)
                    {
                        Dynamic.Framework.Generic.PermissionType item = permissionType;
                        source = (IEnumerable<IPermissionService>)Enumerable.ToList<IPermissionService>(Enumerable.Where<IPermissionService>(source, (Func<IPermissionService, bool>)(p => (Dynamic.Framework.Generic.PermissionType)p.PermissionTypeId == item)));
                    }
                    foreach (IPermissionService permissionService in source)
                    {
                        IPermissionService item = permissionService;
                        if (Enumerable.FirstOrDefault<string>(Enumerable.Where<string>((IEnumerable<string>)list, (Func<string, bool>)(p => p == item.PermissionId))) == null)
                            list.Add(item.PermissionId);
                    }
                }
                else
                {
                    IEnumerable<IPermissionService> source = (IEnumerable<IPermissionService>)Enumerable.ToList<IPermissionService>(Enumerable.Where<IPermissionService>(permissionByFeatureName, (Func<IPermissionService, bool>)(p => Enumerable.Contains<Dynamic.Framework.Generic.PermissionType>((IEnumerable<Dynamic.Framework.Generic.PermissionType>)this.PermissionType, (Dynamic.Framework.Generic.PermissionType)p.PermissionTypeId))));
                    if (Enumerable.Count<IPermissionService>(source) == 0)
                        return false;
                    foreach (IPermissionService permissionService in source)
                    {
                        IPermissionService item = permissionService;
                        if (Enumerable.FirstOrDefault<string>(Enumerable.Where<string>((IEnumerable<string>)list, (Func<string, bool>)(p => p == item.PermissionId))) == null)
                            list.Add(item.PermissionId);
                    }
                }
            }
            this._permissionSplit = list.ToArray();
            return this._permissionSplit.Length <= 0 || Enumerable.Any<string>((IEnumerable<string>)this._permissionSplit, (Func<string, bool>)(p => Enumerable.Contains<string>((IEnumerable<string>)Authentication.User.Permissions, p)));
        }

        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
                return new string[0];
            return Enumerable.ToArray<string>(Enumerable.Select(Enumerable.Where(Enumerable.Select((IEnumerable<string>)original.Split(','), piece =>
            {
                var fAnonymousType0 = new
                {
                    piece = piece,
                    trimmed = piece.Trim()
                };
                return fAnonymousType0;
            }), param0 => !string.IsNullOrEmpty(param0.trimmed)), param0 => param0.trimmed));
        }
    }
}
