using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Controllers;
using VINASIC.Models;

namespace SystemAccount.Controllers
{
    public class UserController : BaseController
    {

        private readonly IBLLUser ibllUser;
        private readonly IBLLUserRole bllUserRole;


        public UserController(IBLLUser _ibllUser, IBLLUserRole _bllUserRole)
        {
            this.ibllUser = _ibllUser;
            this.bllUserRole = _bllUserRole;

        }
        public ActionResult Index(string myuploader)
        {
            List<ModelSelectItem> roles = null;
            List<SelectListItem> rolesItem = new List<SelectListItem>();
            try
            {
                roles = bllUserRole.GetUserRolesModelByUserId(UserContext.UserID, UserContext.IsOwner, UserContext.CompanyID ?? 0);
                if (roles == null)
                {
                    //return Error Page
                }
                rolesItem.AddRange(roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Value.ToString() }).ToList());
                ViewData["roles"] = rolesItem;
            }
            catch (Exception ex)
            {
                // add Error
                throw ex;
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetUsers(string keyWord, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "")
        {
            try
            {
                //if (!IsAuthenticate)
                //{
                    var Users = ibllUser.GetListUser(keyWord, jtStartIndex, jtPageSize, jtSorting, UserContext.UserID);
                    JsonDataResult.Records = Users;
                    JsonDataResult.Result = "OK";
                    JsonDataResult.TotalRecordCount = Users.TotalItemCount;
               // }
            }
            catch (Exception ex)
            {
                //add Error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        public JsonResult SaveUser(ModelUser modelUser, string userRoles)
        {
            var a= new DateTime(DateTime.Now.AddHours(14).Year, DateTime.Now.AddHours(14).Month, DateTime.Now.AddHours(14).Day, 0, 0, 0, 0);
            var b = new DateTime(DateTime.Now.AddHours(14).Year, DateTime.Now.AddHours(14).Month, DateTime.Now.AddHours(14).Day, 23, 59, 59, 999);
            ResponseBase responseResult = null;
            try
            {
                //if (!IsAuthenticate)
                //{
                    if (modelUser.Id == 0)
                    {
                        responseResult = ibllUser.CreateUser(modelUser, UserContext.UserID);
                    }
                    else
                    {
                        responseResult = ibllUser.UpdateUser(modelUser, UserContext.UserID, UserContext.IsOwner);
                    }
                    if (!responseResult.IsSuccess)
                    {
                        if (modelUser.ImagePath != "0")
                        {
                            string path = modelUser.ImagePath.Split(',').ToList().First();
                            if (System.IO.File.Exists(Server.MapPath(path)))
                            {
                                System.IO.File.Delete(Server.MapPath(path));
                            }
                        } 
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = responseResult.Errors.First().MemberName, Message = "Lỗi: " + responseResult.Errors.First().Message });
                    }
                    JsonDataResult.Result = "OK";

                    //if ( responseResult.IsSuccess && modelUser.Id == UserContext.UserID)
                    //{
                    //    // reset imagePath
                    //    //UserContext.ImagePath = responseResult.Data;
                    //    //UserContext.Email = modelUser.Email;
                    //    //UserContext.EmployeeName = modelUser.FisrtName + " " + modelUser.LastName;
                    //     RedirectToAction("Index","User");
                        
                    //}
                //}
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
        public JsonResult DeleteUser(int id)
        {
            ResponseBase responseResult;
            try
            {
                //if (!IsAuthenticate)
                //{
                    responseResult = new ResponseBase();
                    responseResult = ibllUser.DeleteById(id, UserContext.UserID);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = responseResult.Errors.First().MemberName, Message = "Lỗi: " + responseResult.Errors.First().Message });
                    }
                    JsonDataResult.Result = "OK";
                //}
            }
            catch (Exception ex)
            {
                //add Error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        //public JsonResult SaveUser(UserInfoModel userInfoModel)
        //{
        //    ResponseBase responseResult;
        //    try
        //    {
        //        ModelUser modelUser = new ModelUser();
        //        int status = userInfoModel.Status;
        //        string oldPassWord = userInfoModel.OldPassWord;
        //        modelUser.Id = userInfoModel.Id;
        //        modelUser.UserName = userInfoModel.UserName;
        //        modelUser.FisrtName = userInfoModel.FisrtName;
        //        modelUser.LastName = userInfoModel.LastName;
        //        modelUser.Email = userInfoModel.Email;
        //        modelUser.PassWord = userInfoModel.PassWord;
        //        modelUser.ImagePath = userInfoModel.ImagePath;
        //        responseResult = ibllUser.UpdateUserInfo(modelUser, oldPassWord, status);

        //        if (!responseResult.IsSuccess)
        //        {
        //            JsonDataResult.Result = "ERROR";
        //            JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
        //        }
        //        else
        //        {
        //            JsonDataResult.Result = "OK";
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        //add error
        //        JsonDataResult.Result = "ERROR";
        //        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Lỗi: " + ex.Message });
        //    }
        //    return Json(JsonDataResult);
        //}

        [HttpPost]
        public JsonResult UnLockTimeUser(int id)
        {
            ResponseBase responseResult;
            try
            {
                //if (!IsAuthenticate)
                //{
                    responseResult = ibllUser.UnLockTimeByAccountId(UserContext.UserID, id);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = responseResult.Errors.First().MemberName, Message = "Lỗi: " + responseResult.Errors.First().Message });
                    }
                    JsonDataResult.Result = "OK";
               // }
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult ChangeUserState(int id)
        {
            ResponseBase responseResult;
            try
            {
                //if (!IsAuthenticate)
                //{
                    responseResult = ibllUser.ChangeUserStateByAccountId(UserContext.UserID, id);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = responseResult.Errors.First().MemberName, Message = "Lỗi: " + responseResult.Errors.First().Message });
                    }
                    JsonDataResult.Result = "OK";
                //}
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

        [HttpPost]
        public JsonResult ChangePassword(string id, string Password)
        {
            ResponseBase responseResult = null;
            try
            {
                //if (!IsAuthenticate)
                //{
                    responseResult = ibllUser.UpdatePassword(UserContext.UserID, int.Parse(id), Password);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = responseResult.Errors.First().MemberName, Message = "Lỗi: " + responseResult.Errors.First().Message });
                    }
                    JsonDataResult.Result = "OK";
               // }
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Lỗi Dữ Liệu", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

    }
}
