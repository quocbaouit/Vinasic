using System;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Controllers;
using VINASIC.Models;

namespace SystemAccount.Controllers
{
    public class UserProFileController : BaseController
    {
        private readonly IBLLUser bllUser;
        public UserProFileController( IBLLUser _bllUser)
        {
            this.bllUser = _bllUser;
        }

        public ActionResult Index()
        {
            ViewData["User"] = bllUser.GetUserInfoByUserId(UserContext.UserID);
            
            return View();
        }
        public JsonResult SaveUser(UserInfoModel userInfoModel)
        {
            ResponseBase responseResult;
            try
            {
                        ModelUser modelUser = new ModelUser();
                        int status = userInfoModel.Status;
                        string oldPassWord = userInfoModel.OldPassWord;
                        modelUser.Id = userInfoModel.Id;
                        modelUser.UserName = userInfoModel.UserName;
                        modelUser.FisrtName = userInfoModel.FisrtName;
                        modelUser.LastName = userInfoModel.LastName;
                        modelUser.Email = userInfoModel.Email;
                        modelUser.PassWord = userInfoModel.PassWord;
                        modelUser.ImagePath = userInfoModel.ImagePath;
                        responseResult = bllUser.UpdateUserInfo(modelUser,oldPassWord,status);

                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                    else
                    {
                        JsonDataResult.Result = "OK";
                    }                
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        
            
        public JsonResult SaveUserGeneral(string firstName,string lastName, string email,string mobile,int userId)
        {
            try
            {
                var responseResult = bllUser.SaveUserGeneral(firstName, lastName, email, mobile, userId);

                if (!responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
                else
                {
                    JsonDataResult.Result = "OK";
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult SaveUserName(string oldUser,string newUserName,string comfirmUserName,string password,int userId)
        {
            try
            {
                var responseResult = bllUser.SaveUserName(oldUser, newUserName, comfirmUserName, password, userId);

                if (!responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
                else
                {
                    JsonDataResult.Result = "OK";
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult SaveUserPassword(string oldPassword,string newPassword,string comfirmPassword,int userId)
        {
            try
            {
                var responseResult = bllUser.SaveUserPassword(oldPassword, newPassword, comfirmPassword, userId);

                if (!responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
                else
                {
                    JsonDataResult.Result = "OK";
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

    }
}
