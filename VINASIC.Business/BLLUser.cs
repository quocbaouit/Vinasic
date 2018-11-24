using System;
using System.Collections.Generic;
using System.Linq;
using GPRO.Ultilities;
using Dynamic.Framework;
using Dynamic.Framework.Infrastructure.Data;
using Dynamic.Framework.Mvc;
using PagedList;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Enum;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
using VINASIC.Data.Repositories;
using VINASIC.Object;
using Newtonsoft.Json;

namespace VINASIC.Business
{
    public class BLLUser : IBLLUser
    {
        private readonly IT_UserRepository repUser;
        private readonly IT_UserRoleRepository repUserRole;
        private readonly IBLLUserRole bllUserRole;
        private readonly IBLLRolePermission bllRolePermission;
        private readonly IT_RoLeRepository repRole;

        private readonly IUnitOfWork<VINASICEntities> unitOfWork;
        public BLLUser(IUnitOfWork<VINASICEntities> _unitOfWork, IT_UserRepository _repUser, IT_UserRoleRepository _repUserRole, IBLLUserRole _bllUserRole, IBLLRolePermission _bllRolePermission, IT_RoLeRepository _repRole)
        {
            this.unitOfWork = _unitOfWork;
            this.repUser = _repUser;
            this.repUserRole = _repUserRole;
            this.bllUserRole = _bllUserRole;
            this.bllRolePermission = _bllRolePermission;
            this.repRole = _repRole;
        }

        private void SaveChange()
        {
            try
            {
                this.unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private T_User CheckName(string userName, int? AccountId)
        {
            T_User user = null;
            if (AccountId == null)
            {
                user = repUser.GetMany(x => !x.IsDeleted && x.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper())).FirstOrDefault();
            }
            else
            {
                user = repUser.GetMany(x => !x.IsDeleted && x.Id != AccountId && x.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper())).FirstOrDefault();
            }
            return user;
        }
        public ModelUser GetUserByCompanyId(int companyId)
        {
            var user = repUser.GetMany(c => !c.IsDeleted).Select(c => new ModelUser()
            {
                Id = c.Id,
                UserName = c.UserName,
                FisrtName = c.FisrtName,
                LastName = c.LastName,
                Email = c.Email,
            }).FirstOrDefault();
            return user;
        }
        public ResponseBase CreateUser(ModelUser Modeluser, int userId)
        {
            ResponseBase result = null;
            try
            {
                result = new ResponseBase();
                if (CheckName(Modeluser.UserName, null) != null)
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { Message = "Tên đăng nhập đã tồn tại. Vui lòng chọn lại tên khác!", MemberName = "Thêm Mới Tài Khoản" });
                }
                else
                {
                    List<int> rolesId = null;
                    if (Modeluser.NoteForgotPassword != null)
                    {
                        rolesId = new List<int>(Array.ConvertAll(Modeluser.NoteForgotPassword.Split(','), int.Parse));
                    }
                    #region add user
                    T_User user = new T_User();
                    Parse.CopyObject(Modeluser, ref user);
                    if (!string.IsNullOrEmpty(Modeluser.ImagePath))
                    user.ImagePath = Modeluser.ImagePath != "0" ? Modeluser.ImagePath.Split(',').ToList().First() : null;
                    user.IsLock = false;
                    user.IsRequireChangePW = true;
                    user.NoteForgotPassword = null;
                    user.PassWord = GlobalFunction.EncryptMD5(Modeluser.PassWord);
                    user.CreatedUser = userId;
                    user.CreatedDate = DateTime.Now.AddHours(14);
                    repUser.Add(user);
                    SaveChange();
                    result.Data = user.Id;
                    #endregion

                    #region add user role
                    if (rolesId != null)
                    {
                        T_UserRole userRole;
                        foreach (var role in rolesId)
                        {
                            userRole = new T_UserRole();
                            userRole.RoleId = role;
                            userRole.UserId = user.Id;
                            userRole.Decription = null;
                            userRole.CreatedUser = userId;
                            userRole.CreatedDate = DateTime.Now.AddHours(14);
                            repUserRole.Add(userRole);
                            
                        }
                    }
                    #endregion

                    SaveChange();
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase UpdateUser(ModelUser user, int actionUserId, bool IsOwner)
        {
            throw new NotImplementedException();
        }
        public ResponseBase SaveUserGeneral(string firstName, string lastName, string email, string mobile,int userId)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                T_User user = repUser.Get(x => x.Id == userId && !x.IsDeleted);
                if (user!=null)
                {
                    user.FisrtName = firstName;
                    user.LastName = lastName;
                    user.Email = email;
                    user.Mobile = mobile;
                    user.UpdatedUser = userId;
                    user.UpdatedDate = DateTime.Now.AddHours(14);
                    repUser.Update(user);
                    SaveChange();
                    result.IsSuccess = true;

                }
                else
                {
                    result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại!" });
                }

            }
            catch (Exception ex)
            {
                result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Kiểm tra lại thông tin!" });
            }
            return result;
        }
        public ResponseBase SaveUserName(string oldUser, string newUserName, string comfirmUserName, string password, int userId)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                T_User user = repUser.Get(x => x.Id == userId && !x.IsDeleted);
                if (user!=null)
                {
                    if (oldUser.Trim().ToLower() != user.UserName.Trim().ToLower())
                    {
                        result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Tên Đang nhập cũ không đúng" });
                        goto end;
                    }
                    if (newUserName.Trim().ToLower() != comfirmUserName.Trim().ToLower())
                    {
                        result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Xác Nhận tên đang nhập không đúng" });
                        goto end;
                    }
                    if (GlobalFunction.EncryptMD5(password.Trim()).ToLower() != user.PassWord.Trim().ToLower())
                    {
                        result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Mật khẩu không đúng" });
                        goto end;
                    }
                    user.UserName = newUserName;
                    user.UpdatedUser = userId;
                    user.UpdatedDate = DateTime.Now.AddHours(14);
                    repUser.Update(user);
                    SaveChange();
                    result.IsSuccess = true;

                }
                else
                {
                    result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại!" });
                }

            }
            catch (Exception ex)
            {
                result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Kiểm tra lại thông tin!" });
            }
            end:
            return result;
        }
        public ResponseBase SaveUserPassword(string oldPassword, string newPassword, string comfirmPassword, int userId)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                T_User user = repUser.GetMany(x => x.Id == userId && !x.IsDeleted).FirstOrDefault();
                if (user!=null)
                {
                    if (GlobalFunction.EncryptMD5(oldPassword.Trim()) != user.PassWord.Trim().ToLower())
                    {
                        result.Errors.Add(new Error() { MemberName = "Update Account", Message ="Mật khẩu cũ không đúng"  });
                        goto end;
                    }
                    if (comfirmPassword.Trim().ToLower() != newPassword.Trim().ToLower())
                    {
                        result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Xác nhận mật khẩu không đúng" });
                        goto end;
                    }
                    user.PassWord = GlobalFunction.EncryptMD5(newPassword.Trim());
                    user.UpdatedUser = userId;
                    user.UpdatedDate = DateTime.Now.AddHours(14);
                    repUser.Update(user);
                    SaveChange();
                    result.IsSuccess = true;

                }
                else
                {
                    result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại!" });
                }

            }
            catch (Exception ex)
            {
                result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Kiểm tra lại thông tin!" });
            }
                end:
            return result;
        }
        public ResponseBase UpdateUserInfo(ModelUser modeluser, string oldPassWord, int status)
        {
            ResponseBase result = new ResponseBase();
            try
            {
                T_User user = repUser.GetMany(x => x.Id == modeluser.Id && !x.IsDeleted).FirstOrDefault();
                if (user != null)
                {
                    if (status == 1)
                    {
                        user.FisrtName = modeluser.FisrtName;
                        user.LastName = modeluser.LastName;
                    }
                    else if (status == 2)
                    {
                        oldPassWord = GlobalFunction.EncryptMD5(oldPassWord.Trim());
                        if (oldPassWord != user.PassWord)
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Mật Khẩu Cũ Không Đúng!" });
                            goto end;
                        }
                        else
                        {
                            user.PassWord = GlobalFunction.EncryptMD5(modeluser.PassWord.Trim());
                        }
                    }

                    else if (status == 3)
                    {
                        user.Email = modeluser.Email;
                    }
                    else
                    {
                        user.ImagePath = modeluser.ImagePath;
                    }
                    user.UpdatedUser = modeluser.Id;
                    user.UpdatedDate = DateTime.Now.AddHours(14);
                    repUser.Update(user);
                    SaveChange();
                    result.IsSuccess = true;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        end:
            return result;
        }

        public ResponseBase CreateUser(ModelUser user, int actionUserId, int companyId)
        {
            throw new NotImplementedException();
        }

        public ResponseBase UpdateUser(ModelUser Modeluser, int userId, bool IsOwner, int companyId)
        {
            ResponseBase result = null;
            try
            {
                result = new ResponseBase();
                T_User user = repUser.GetMany(x => x.Id == Modeluser.Id && !x.IsDeleted).FirstOrDefault();
                if (user != null)
                {
                    List<int> listRoleIdNew = null;
                    if (Modeluser.NoteForgotPassword != null)
                    {
                        //Convert char array To list int
                        listRoleIdNew = new List<int>(Array.ConvertAll(Modeluser.NoteForgotPassword.Split(','), int.Parse));
                    }

                    #region update user detail

                    user.PassWord = user.IsRequireChangePW ? GlobalFunction.EncryptMD5(Modeluser.PassWord) : user.PassWord;
                    user.FisrtName = Modeluser.FisrtName;
                    user.LastName = Modeluser.LastName;
                    user.NoteForgotPassword = null;
                    if (Modeluser.ImagePath != "0")
                    {
                        user.ImagePath = Modeluser.ImagePath.Split(',').ToList().First();
                    }
                    user.Email = Modeluser.Email;
                    user.UpdatedUser = userId;
                    user.UpdatedDate = DateTime.Now.AddHours(14);
                    repUser.Update(user);
                    #endregion

                    //kt neu user la isowner va co dang update thong tin cua minh hay khong
                    IsOwner = false;

                    //Update user role
                    var listUserRolesOld = bllUserRole.GetUserRolesModelByUserId(user.Id, IsOwner, companyId);
                    T_UserRole userRole;
                    if (listRoleIdNew != null)
                    {
                        #region sử lý nếu list user role new != null
                        if (listUserRolesOld != null && listUserRolesOld.Count > 0)
                        {
                            // change state of UserRoll                           
                            foreach (var oldItem in listUserRolesOld)
                            {
                                // find  role
                                var userRoleFind = listRoleIdNew.Find(x => x == oldItem.Value);
                                // if not exists in new list   ==> remove old item
                                if (userRoleFind == 0)
                                {
                                    userRole = repUserRole.GetMany(x => x.Id == oldItem.Value && !x.IsDeleted).FirstOrDefault();
                                    if (userRole != null)
                                    {
                                        userRole.IsDeleted = true;
                                        userRole.DeletedUser = userId;
                                        userRole.DeletedDate = DateTime.Now.AddHours(14);
                                        repUserRole.Update(userRole);
                                    }
                                }
                                else
                                {
                                    // remove userRole out of new list if exists
                                    listRoleIdNew.Remove(userRoleFind);
                                }
                            }
                            //
                            if (listRoleIdNew != null && listRoleIdNew.Count > 0)
                            {
                                CreateUserRollByListRoleId(userId, user.Id, listRoleIdNew);
                            }
                            CreateUserRollByListRoleId(userId, user.Id, listRoleIdNew);
                        }

                        #endregion
                    }
                    else
                    {
                        #region sử lý nếu list user role new is null
                        foreach (var oldItem in listUserRolesOld)
                        {
                            userRole = repUserRole.GetMany(x => x.Id == oldItem.Value && !x.IsDeleted).FirstOrDefault();
                            if (userRole != null)
                            {
                                userRole.IsDeleted = true;
                                userRole.DeletedUser = userId;
                                userRole.DeletedDate = DateTime.Now.AddHours(14);
                                repUserRole.Update(userRole);
                            }
                        }
                        #endregion
                    }
                    result.Data = Modeluser.ImagePath != "0" ? Modeluser.ImagePath : "0";
                    SaveChange();
                    result.IsSuccess = true;

                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Update Account", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại!" });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase UnLockTimeByAccountId(int userId, int accountId)
        {
            ResponseBase userResult = null;
            try
            {
                userResult = new ResponseBase();
                T_User user = repUser.GetMany(x => x.Id == accountId && !x.IsDeleted).FirstOrDefault();
                if (user == null)
                {
                    userResult.IsSuccess = false;
                    userResult.Errors.Add(new Error() { MemberName = "Mở Khóa Thời Gian", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại." });
                }
                else
                {
                    user.LockedTime = null;
                    user.UpdatedUser = userId;
                    user.UpdatedDate = DateTime.Now.AddHours(14);
                    repUser.Update(user);
                    SaveChange();
                    userResult.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                //add Error
                throw ex;
            }
            return userResult;
        }

        public ResponseBase UpdatePassword(int userId, int accountId, string password)
        {
            ResponseBase userResult = null;
            try
            {
                userResult = new ResponseBase();
                T_User user = repUser.GetMany(x => x.Id == accountId && !x.IsDeleted).FirstOrDefault();
                if (user == null)
                {
                    userResult.IsSuccess = false;
                    userResult.Errors.Add(new Error() { MemberName = "Mở Khóa Thời Gian", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại." });
                }
                else
                {
                    //GlobalFunction.RandomPass(lengh); // function random mat khau truyen vao length cua pass sau do kiem tra neu co mail thi chuyen wa mail neu ko co mail yeu cau admin change pass
                    user.IsRequireChangePW = true;
                    user.NoteForgotPassword = null;
                    user.IsForgotPassword = false;
                    user.PassWord = GlobalFunction.EncryptMD5(password);
                    user.UpdatedUser = userId;
                    user.UpdatedDate = DateTime.Now.AddHours(14);
                    repUser.Update(user);
                    SaveChange();
                    userResult.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                //add Error
                throw ex;
            }
            return userResult;
        }

        public ResponseBase DeleteById(int accountId, int actionUserId)
        {
            ResponseBase result = null;
            try
            {
                result = new ResponseBase();
                T_User user = repUser.GetMany(x => x.Id == accountId && !x.IsDeleted).FirstOrDefault();
                if (user != null)
                {
                    user.IsDeleted = true;
                    user.DeletedUser = actionUserId;
                    user.DeletedDate = DateTime.Now.AddHours(14);
                    repUser.Update(user);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Delete Account", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại!" });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase DeleteByListId(List<int> listId, int userId)
        {
            ResponseBase userResult = null;
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userResult;
        }

        public List<ModelUser> GetListUserByUserId(int userId, int companyId, string sorting)
        {
            throw new NotImplementedException();
        }

        public ModelUser GetUserByCompanyId()
        {
            throw new NotImplementedException();
        }

        public ModelUser GetUserInfoByUserId(int userId)
        {
            ModelUser user = null;
            user = repUser.GetMany(x => x.Id == userId && !x.IsDeleted).Select(x => new ModelUser()
            {
                Id = x.Id,
                UserName = x.UserName,
                PassWord = x.PassWord,
                IsLock = x.IsLock,
                IsRequireChangePW = x.IsRequireChangePW,
                IsForgotPassword = x.IsForgotPassword,
                NoteForgotPassword = x.NoteForgotPassword,
                Email = x.Email,
                ImagePath = x.ImagePath,
                LockedTime = x.LockedTime,
                FisrtName = x.FisrtName,
                LastName = x.LastName,
                CreatedDate = x.CreatedDate,
                //T_UserRole = x.T_UserRole,
                //Address = x.T_Employee.Address,
                Mobile = x.Mobile,
                //HireDate = x.T_Employee.HireDate??DateTime.Now.AddHours(14),
               // Position = x.T_Employee.T_Position.Name,               
            }).FirstOrDefault();
            return user;
        }

        public List<ModelUser> GetListUserByUserId(int userId, string sorting)
        {
            List<ModelUser> users = null;
            List<T_RoLe> roles = null;
            try
            {
                roles = repRole.GetMany(x => !x.IsDeleted).ToList();
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                users = repUser.GetMany(x =>!x.IsDeleted).Select(x => new ModelUser()
               {
                   Id = x.Id,
                   UserName = x.UserName,
                   PassWord = x.PassWord,
                   IsLock = x.IsLock,
                   IsRequireChangePW = x.IsRequireChangePW,
                   IsForgotPassword = x.IsForgotPassword,
                   NoteForgotPassword = x.NoteForgotPassword,
                   Email = x.Email,
                   ImagePath = x.ImagePath,
                   LockedTime = x.LockedTime,
                   FisrtName = x.FisrtName,
                   LastName = x.LastName,
                   CreatedDate = x.CreatedDate,
                   //T_UserRole = x.T_UserRole,
               }).OrderBy(sorting).ToList();
                
                if ((roles != null && roles.Count > 0) || users != null && users.Count > 0)
                {
                    //foreach (var item in users)
                    //{
                    //    if (item.T_UserRole.Count > 0)
                    //    {
                    //        item.UserRoles = new List<ModelUserRole>(); // khoi tao list userrole
                    //        foreach (var userrole in item.T_UserRole)
                    //        {
                    //            if (!userrole.IsDeleted)
                    //            {
                    //                var roleDetail = roles.Find(x => x.Id == userrole.RoleId);
                    //                if (roleDetail != null)
                    //                {
                    //                    item.UserRoles.Add(new ModelUserRole() { Id = roleDetail.Id, RoleName = roleDetail.RoleName });
                    //                }
                    //            }

                    //        }
                    //    }
                    //    item.T_UserRole = null;
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        public PagedList<ModelUser> GetListUser(string keyWord, int startIndexRecord, int pageSize, string sorting, int userId)
        {
            List<ModelUser> users = null;
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                users = GetListUserByUserId(userId, sorting);
                var pageNumber = (startIndexRecord / pageSize) + 1;
                return new PagedList<ModelUser>(users, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public ResponseBase UserSubscribe(PushNotificationSubscribe request, int userId)
        {
            ResponseBase userResult = null;
            try
            {
              
                userResult = new ResponseBase();
                T_User user = repUser.GetMany(x => x.Id == userId).FirstOrDefault();
                if (user == null)
                {
                    userResult.IsSuccess = false;
                    userResult.Errors.Add(new Error() { MemberName = "Thay Đổi Trạng Thái", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại." });
                }
                else
                {
                    List<Notification> listSubcription = new List<Notification>();
                    if (!string.IsNullOrEmpty(user.Subscription))
                    {
                        listSubcription = JsonConvert.DeserializeObject<List<Notification>>(user.Subscription);
                    }                   
                    var isExist = listSubcription.Where(x => x.Endpoint.Contains(request.Subscription.endpoint)).FirstOrDefault();
                    if (isExist==null)
                    {
                        listSubcription.Add(new Notification()
                        {
                            BrowserName = request.Subscription.BrowserName,
                            Keys = request.Subscription.keys,
                            BrowserVersion = request.Subscription.BrowserVersion,
                            OsName = request.Subscription.OsName,
                            OsVersion = request.Subscription.OsVersion,
                            Endpoint = request.Subscription.endpoint,
                            Guid = Guid.NewGuid(),
                            Unsubscribed = false,
                        });
                    }
                    user.Subscription = JsonConvert.SerializeObject(listSubcription);
                    user.UpdatedUser = userId;
                    user.UpdatedDate = DateTime.Now.AddHours(14);
                    repUser.Update(user);
                    SaveChange();
                    userResult.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                //add Error
                throw ex;
            }
            return userResult;
        }
        public ResponseBase ChangeUserStateByAccountId(int userId, int accountId)
        {
            ResponseBase userResult = null;
            try
            {
                userResult = new ResponseBase();
                T_User user = repUser.GetMany(x => x.Id == accountId && !x.IsDeleted).FirstOrDefault();
                if (user == null)
                {
                    userResult.IsSuccess = false;
                    userResult.Errors.Add(new Error() { MemberName = "Thay Đổi Trạng Thái", Message = "Tài Khoản đang thao tác không tồn tại. Vui lòng kiểm tra lại." });
                }
                else
                {
                    user.IsLock = (user.IsLock ? false : true);
                    user.UpdatedUser = userId;
                    user.UpdatedDate = DateTime.Now.AddHours(14);
                    repUser.Update(user);
                    SaveChange();
                    userResult.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                //add Error
                throw ex;
            }
            return userResult;
        }

        public ResponseBase GetUserByUserNamePassword(string userName, string password, List<ModelCountLoginFail> listModelCountLoginFail, int timeLock, int loginCount, string systemCaptcha, string userCaptcha)
        {
            ResponseBase result = null;
            try
            {
                result = new ResponseBase();
                userName = userName.Trim().ToUpper();
                int index = -1;
                if (listModelCountLoginFail != null && listModelCountLoginFail.Count > 0)
                {
                    for (int i = 0; i < listModelCountLoginFail.Count; i++)
                    {
                        if (listModelCountLoginFail[i].UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()))
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index != -1)
                    {
                        if (listModelCountLoginFail[index].isCaptcha)
                        {
                            if (!systemCaptcha.Equals(userCaptcha))
                            {
                                result.IsSuccess = false;
                                result.Errors.Add(new Error() { MemberName = "Login Fail", Message = "Trả lời Xác nhận không đúng." });
                                result.Data = listModelCountLoginFail;
                            }
                            else
                            {
                                result = CheckUserNamePassword(userName, password, listModelCountLoginFail, timeLock, loginCount, index);
                            }
                        }
                        else
                        {
                            result = CheckUserNamePassword(userName, password, listModelCountLoginFail, timeLock, loginCount, index);
                        }
                    }
                    else
                    {
                        result = CheckUserNamePassword(userName, password, listModelCountLoginFail, timeLock, loginCount, index);
                    }
                }
                else
                {
                    result = CheckUserNamePassword(userName, password, listModelCountLoginFail, timeLock, loginCount, index);
                }
            }
            catch (Exception ex)
            {
                //add Error
                throw;
            }
            return result;
        }

        private ResponseBase CheckUserNamePassword(string userName, string password, List<ModelCountLoginFail> listModelCountLoginFail, int timeLock, int loginCount, int index)
        {
            ResponseBase result = new ResponseBase();
            T_User user = repUser.GetMany(x => x.UserName.Trim().ToUpper().Equals(userName) && (!x.IsDeleted || x.IsOwner==true) && !x.IsLock).FirstOrDefault();
            if (user == null)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Login Fail", Message = "Tên Đăng Nhập không tồn tại. Vui lòng kiểm tra lại." });
            }
            else if (user.IsLock)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Login Fail", Message = "Tài Khoản của bạn đang đã bị khóa bởi Quản Trị Viên. \nVui lòng liên hệ với Quản Trị Viên để biết thông tin." });
            }
            else
            {
                if (user.LockedTime != null && user.LockedTime > DateTime.Now.AddHours(14))
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "TimeLock", Message = "Tài Khoản hiện tại đang bị Khóa tới " + string.Format("{0:d/M/yyyy - HH giờ : mm phút }", user.LockedTime.ToString()) + ".\n Lưu ý : Bạn có thể liên hệ với Quản Trị Viên để yêu cầu mở khóa." });
                }
                else
                {

                    if (!user.PassWord.Equals(GlobalFunction.EncryptMD5(password)))
                    {
                        string mesage = string.Empty;
                        if (listModelCountLoginFail != null && listModelCountLoginFail.Count > 0)
                        {


                            if (index != -1)
                            {
                                if (listModelCountLoginFail[index].Count > 1)
                                {
                                    listModelCountLoginFail[index].Count--;
                                    if (loginCount - listModelCountLoginFail[index].Count == 2)
                                    {
                                        listModelCountLoginFail[index].isCaptcha = true;
                                    }
                                    mesage = "Mật Khẩu không đúng. Vui lòng kiểm tra lại.\n Chú Ý : Bạn còn được phép Đăng Nhập sai " + listModelCountLoginFail[index].Count + " lần. \nTài khoản sẽ bị khóa nếu vượt quá số lần cho phép.";

                                }
                                else
                                {
                                    listModelCountLoginFail[index].Count = loginCount;
                                    listModelCountLoginFail[index].isCaptcha = false;
                                    user.LockedTime = DateTime.Now.AddHours(14).AddMinutes(timeLock);
                                    repUser.Update(user);
                                    SaveChange();
                                    mesage = "Tài Khoản của bạn đã bị khóa tới " + string.Format("{0:d/M/yyyy - HH giờ : mm phút }", user.LockedTime.ToString()) + "  .\n Chú Ý : Bạn có thể liên hệ Quản Trị Hệ Thống để mở khóa nếu không muốn đợi.";
                                }
                            }
                            else
                            {
                                var modelCount = new ModelCountLoginFail();
                                modelCount.UserName = userName;
                                modelCount.Count = loginCount;
                                modelCount.isCaptcha = false;
                                listModelCountLoginFail.Add(modelCount);
                                mesage = "Mật Khẩu không đúng. Vui lòng kiểm tra lại.\n Chú Ý : Bạn còn được phép Đăng Nhập sai " + modelCount.Count + " lần. \nTài khoản sẽ bị khóa nếu vượt quá số lần cho phép.";
                            }
                        }
                        else
                        {
                            listModelCountLoginFail = new List<ModelCountLoginFail>();
                            var modelCount = new ModelCountLoginFail();
                            modelCount.UserName = userName;
                            modelCount.Count = loginCount;
                            listModelCountLoginFail.Add(modelCount);
                            mesage = "Mật Khẩu không đúng. Vui lòng kiểm tra lại.\n Chú Ý : Bạn còn được phép Đăng Nhập sai " + modelCount.Count + " lần. \nTài khoản sẽ bị khóa nếu vượt quá số lần cho phép.";
                        }
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Error", Message = mesage });
                        result.Data = listModelCountLoginFail;
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Data = user.Id;
                    }
                }
            }
            return result;
        }

        public ResponseBase ForgotPasswordRequest(string userName, string mailOrNote, int actionRequest)
        {
            ResponseBase result = null;
            T_User user = CheckName(userName, null);
            try
            {
                result = new ResponseBase();
                if (user != null)
                {
                    if (actionRequest == eForgotPasswordActionType.ByMail)
                    {
                        if (user.Email.Trim().ToUpper().Equals(mailOrNote.Trim().ToUpper()))
                        {
                            string newPassword = GlobalFunction.RandomPass(8);
                            user.PassWord = GlobalFunction.EncryptMD5(newPassword);
                            user.IsRequireChangePW = true;
                            repUser.Update(user);
                            SaveChange();
                            result.IsSuccess = true;
                            result.Errors.Add(new Error() { MemberName = "Request Success", Message = "Đã gửi yêu cầu thành công." });
                            result.Data = newPassword;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Errors.Add(new Error() { MemberName = "Request Fail", Message = "Email này chưa được đăng ký trong hệ Thống. Vui lòng kiểm tra lại." });

                        }
                    }
                    else
                    {
                        user.IsForgotPassword = true;
                        user.NoteForgotPassword = mailOrNote;
                        repUser.Update(user);
                        SaveChange();
                        result.IsSuccess = true;
                        result.Errors.Add(new Error() { MemberName = "Request Success", Message = "Đã gửi yêu cầu thành công." });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Request Fail", Message = "Tên Đăng Nhập không tồn tại trong hệ Thống. Vui lòng kiểm tra lại." });
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }
        public int GetOrgByUser(int userId)
        {
            var orgId = repUser.GetById(userId).OrganizationId??1;
            return orgId;
        }
        public UserService GetUserService(int userId)
        {
            UserService userService = null;
            try
            {
                userService = repUser.GetMany(c => c.Id == userId && (!c.IsDeleted|| c.IsOwner==true) ).Select(c => new UserService()
                {
                    IsOwner = c.IsOwner??false,
                    employeeId = 1,
                    UserID = c.Id,
                    ImagePath = c.ImagePath,
                    Email = c.Email,
                    EmployeeName = c.FisrtName
                }).FirstOrDefault();
                if (userService != null)
                {
                    List<int> listUserRole = bllUserRole.GetUserRolesIdByUserId(userId);
                    List<string> listPermissionUrl = (List<string>)bllRolePermission.GetListSystemNameAndUrlOfPermissionByListRoleId(listUserRole);
                    if (listPermissionUrl != null)
                        userService.Permissions = listPermissionUrl.ToArray();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return userService;
        }

        private void CreateUserRollByListRoleId(int ActionUserId, int userId, List<int> ListRollId)
        {
            foreach (var roleId in ListRollId)
            {
                var userRole = new T_UserRole();
                userRole.RoleId = roleId;
                userRole.UserId = userId;
                userRole.Decription = null;
                userRole.CreatedUser = ActionUserId;
                userRole.CreatedDate = DateTime.Now.AddHours(14);
                repUserRole.Add(userRole);
            }
        }

        public T_User GetUserInfoByUserIdAndCompanyId(int companyId, int userId)
        {
            try
            {
                var user = repUser.GetMany(x => !x.IsDeleted && x.Id == userId).FirstOrDefault();
                if (user != null)
                    return user;
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteUserbyEmail(string email, int companyId, int acctionUserId)
        {
            try
            {
                var user = repUser.GetMany(x => !x.IsDeleted && x.Email.Trim().ToUpper().Equals(email.Trim().ToUpper())).FirstOrDefault();
                if (user != null)
                {
                    user.IsDeleted = true;
                    user.DeletedUser = acctionUserId;
                    user.DeletedDate = DateTime.Now.AddHours(14);
                    repUser.Update(user);
                    SaveChange();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckExistsEmail(string email, int companyId)
        {
            try
            {
                var user = repUser.GetMany(x => !x.IsDeleted && x.Email.Trim().ToUpper().Equals(email.Trim().ToUpper())).FirstOrDefault();
                if (user != null)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AddUser(T_User user, int acctionUserId)
        {
            try
            {
                string password = CreatePasswordRandom();
                user.PassWord = GlobalFunction.EncryptMD5(password);
                user.CreatedUser = acctionUserId;
                user.DeletedDate = DateTime.Now.AddHours(14);
                repUser.Add(user);
                SaveChange();
                return password;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string CreatePasswordRandom()
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyz0123456789";
            char[] chars = new char[8];
            Random rd = new Random();

            for (int i = 0; i < 8; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public List<T_User> GetListUserByCompanyId(int companyId)
        {
            try
            {
                var listUser = repUser.GetMany(x => !x.IsDeleted).ToList();
                if (listUser != null)
                    return listUser;
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedList<T_User> GetListUserWithoutExistsInList(List<int> existT_UserIds, string keyWord, int searchBy, int startIndexRecord, int pageSize, string sorting, int companyId)
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                List<T_User> users  = GetUsers(existT_UserIds, keyWord, searchBy, companyId, sorting);

                var pageNumber = (startIndexRecord / pageSize) + 1;
                return new PagedList<T_User>(users, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<T_User> GetUsers(List<int> existT_UserIds, string keyWord, int searchBy, int companyId, string sorting)
        {
            try
            {
                IQueryable<T_User> users = null;
                switch (searchBy)
                {
                    case 0: //all
                        users = repUser.GetMany(x => !x.IsDeleted && !existT_UserIds.Contains(x.Id));
                        break;
                    case 1: //username
                        keyWord = keyWord.Trim().ToUpper();
                        users = repUser.GetMany(x => !x.IsDeleted  && !existT_UserIds.Contains(x.Id) && x.UserName.Trim().ToUpper().Contains(keyWord));
                        break;
                    case 2: // name
                        keyWord = keyWord.Trim().ToUpper();
                        users = repUser.GetMany(x => !x.IsDeleted  && !existT_UserIds.Contains(x.Id) && (x.FisrtName.Trim().ToUpper().Contains(keyWord) || x.LastName.Trim().ToUpper().Contains(keyWord)));
                        break;
                }
                if (users != null)
                    return users.OrderBy(sorting).ToList();
                return new List<T_User>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
