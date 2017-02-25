using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Dynamic.Framework.Security;
using VINASIC.App_Global;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Enum;
using VINASIC.Business.Interface.Model;
using Dynamic.Framework.Mvc;
namespace VINASIC.Controllers
{
    public class AuthenticateController : BaseController
    {
        //
        // GET: /Authenticate/

         private readonly IBLLUser ibllUser;

         public AuthenticateController(IBLLUser _ibllUser)
         {
            this.ibllUser = _ibllUser; 
        }

        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Login(string token, string url)
        {
            if (UserContext != null)
            {
                if (string.IsNullOrEmpty(token))
                {
                    if (string.IsNullOrEmpty(url))
                        return Redirect(DefaultPage);
                    else
                        return Redirect(url);
                }
                else
                {
                    ArrayList SSOTokenReceive = (ArrayList)Dynamic.Framework.Security.SerializeObject.Deserialize(Dynamic.Framework.Security.SerializeObject.Decrypt(token));
                    if (SSOTokenReceive != null)
                    {
                        var sessionId = Convert.ToString(SSOTokenReceive[0]);
                        var returnUrlByToken = Convert.ToString(SSOTokenReceive[1]);
                        if (!string.IsNullOrEmpty(returnUrlByToken))
                        {
                            ArrayList SSOToken = new ArrayList();
                            SSOToken.Add(UserContext.UserID);                           
                            SSOToken.Add(sessionId);
                            var tokenRedirect = HttpUtility.UrlEncode(Dynamic.Framework.Security.SerializeObject.Encrypt(Dynamic.Framework.Security.SerializeObject.Serialize(SSOToken)));
                            return Redirect(returnUrlByToken + "?token=" + tokenRedirect);                            
                        }
                    }
                }

            }
            else
            {
                Session["Url"] = url;
                Session["Token"] = token;                
            }
            return View();
        }

        [HttpPost]
        public JsonResult LoginAction(string userName, string password, string captcha)
        {

            ResponseBase responseResult;
            string mesage = string.Empty;
            try
            {
                List<ModelCountLoginFail> loginFail = Session["UserInfo"] as List<ModelCountLoginFail>;
                responseResult = ibllUser.GetUserByUserNamePassword(userName, password, loginFail, AppGlobal.LoginConfig.GetTimeLock(), AppGlobal.LoginConfig.GetLoginCount(), Session["Captcha"].ToString(), captcha);
                if (!responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    var listLoginFail = responseResult.Data as List<ModelCountLoginFail>;
                    if (listLoginFail != null && listLoginFail.Count > 0)
                    {
                        var modelLoginFail = listLoginFail.Find(x => x.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()));
                        if (modelLoginFail != null)
                        {
                            JsonDataResult.Data = modelLoginFail.isCaptcha;
                            Session["UserInfo"] = listLoginFail;
                        }
                    }
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
                else
                {
                    JsonDataResult.Result = "OK";
                    int userId = (int)responseResult.Data;
                    LoginSuccessHandle(userId);
                    var tokenEncrypt = Session["Token"] != null ? Session["Token"].ToString() : string.Empty;
                    var url = Session["Url"]!=null?Session["Url"].ToString():string.Empty;
                    if (!string.IsNullOrEmpty(tokenEncrypt))
                    {
                        ArrayList SSOTokenReceive = (ArrayList)Dynamic.Framework.Security.SerializeObject.Deserialize(Dynamic.Framework.Security.SerializeObject.Decrypt(tokenEncrypt));
                        if (SSOTokenReceive != null)
                        {
                            var sessionId = Convert.ToString(SSOTokenReceive[0]);
                            var returnUrlByToken = Convert.ToString(SSOTokenReceive[1]);

                            if (!string.IsNullOrEmpty(returnUrlByToken))
                            {
                                ArrayList SSOToken = new ArrayList();
                                SSOToken.Add(Authentication.UserId);
                                SSOToken.Add(sessionId);
                                var token = HttpUtility.UrlEncode(Dynamic.Framework.Security.SerializeObject.Encrypt(Dynamic.Framework.Security.SerializeObject.Serialize(SSOToken)));
                                JsonDataResult.Data = GPRO.Ultilities.GlobalFunction.UpdateQueryString("token", token, returnUrlByToken);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(url))
                            JsonDataResult.Data = url;
                        else
                            JsonDataResult.Data = DefaultPage;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                //add Error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult RequestPassword(string userName, string emailOrNote, int actionRequest)
        {
            ResponseBase responseResult;
            string mesage = string.Empty;
            try
            {
                responseResult = ibllUser.ForgotPasswordRequest(userName, emailOrNote, actionRequest);
                if (!responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
                else
                {
                    if (actionRequest == eForgotPasswordActionType.ByMail)
                    {
                        // send mail
                        List<string> emails = new List<string>();
                        emails.Add("quocbaouitn@gmail.com");
                        string comment = "Mật khẩu mới của bạn là :" + responseResult.Data as string;
                        //if (!SendMailForEmployee(emails, "Tạo Mới Mật Khẩu", "Đây là content", comment, null, null, null))
                        //{
                        //    JsonDataResult.Result = "ERROR";
                        //    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Gửi Mail", Message = "Lỗi: Không thể gửi mail tới địa chỉ " + emailOrNote });
                        //}
                        //else
                        //{
                        //    JsonDataResult.Result = "OK";
                        //    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //add Error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        public ActionResult Logout()
        {
            Authentication.Logout();
            return RedirectToAction("Login");
        }

        private void LoginSuccessHandle(int userId)
        {
            try
            {
                Authentication.Login(userId);               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
