using System;
using System.Collections.Generic;
using System.Linq;
using GPRO.Ultilities;
using Dynamic.Framework;
using Dynamic.Framework.Infrastructure.Data;
using Dynamic.Framework.Mvc;
using PagedList;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
using VINASIC.Data.Repositories;
using VINASIC.Object;
using System.Threading.Tasks;

namespace VINASIC.Business
{
    public class BllNotification : IBllNotification
    {
        private readonly IT_NotificationRepository _repNotification;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllNotification(IUnitOfWork<VINASICEntities> unitOfWork, IT_NotificationRepository repNotification)
        {
            _unitOfWork = unitOfWork;
            _repNotification = repNotification;
        }
        private void SaveChange()
        {

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ModelNotification> GetListNotification()
        {
            List<ModelNotification> notification;
            try
            {
                notification = _repNotification.GetMany(c => !c.IsDeleted && !c.IsRead).Select(c => new ModelNotification()
                {
                    Id = c.Id,
                    ListUser = c.UserName,
                    Description = c.Description
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return notification;
        }
        
        public  void UpdateNotification(string userId)
        {
            var listNotifications =
                _repNotification.GetMany(c => !c.IsDeleted && !c.IsRead && !string.IsNullOrEmpty(c.UserName)).Select(c => new ModelNotification()
                {
                    Id = c.Id,
                    ListUser = c.UserName,
                }).ToList();

            if (listNotifications.Count > 0)
            {
                foreach (var listNotification in listNotifications)
                {
                    var listUserId = listNotification.ListUser.Split(',');
                    var strUpdate = String.Join(",", listUserId.Where(x => !x.Contains(userId)));
                    if (strUpdate.Count() == listUserId.Count()) continue;
                    var stNotification = String.Join(",", listUserId.Where(x => !x.Contains(userId)));
                    var notification1 = listNotification;
                    var notification = _repNotification.Get(x => x.Id == notification1.Id);
                    if (notification == null) continue;
                    notification.UserName = stNotification;
                    _repNotification.Update(notification);
                    SaveChange();
                }
            }
        }

        public ResponseBase Create(T_Notification obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    var notification = new T_Notification();
                    Parse.CopyObject(obj, ref notification);
                    notification.CreatedDate = DateTime.Now.AddHours(14);
                    _repNotification.Add(notification);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error()
                    {
                        MemberName = "Create Notification",
                        Message = "Đối Tượng Không tồn tại"
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseBase Update(int id, string notifi)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                T_Notification notification = _repNotification.Get(x => x.Id == id);
                if (notification != null)
                {
                    notification.UserName = notifi;
                    _repNotification.Update(notification);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateNotification", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseBase Update(T_Notification obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                T_Notification notification = _repNotification.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (notification != null)
                {
                    notification.Description = obj.Description;
                    notification.UpdatedUser = obj.UpdatedUser;
                    _repNotification.Update(notification);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateNotification", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            ResponseBase responResult;

            try
            {
                responResult = new ResponseBase();
                var notification = _repNotification.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
                if (notification != null)
                {
                    notification.IsDeleted = true;
                    notification.DeletedUser = userId;
                    notification.DeletedDate = DateTime.Now.AddHours(14);
                    _repNotification.Update(notification);
                    SaveChange();
                    responResult.IsSuccess = true;
                }
                else
                {
                    responResult.IsSuccess = false;
                    responResult.Errors.Add(new Error() { MemberName = "Delete", Message = "Đối Tượng Đã Bị Xóa,Vui Lòng Kiểm Tra Lại" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responResult;
        }
        public ResponseBase DeleteByListId(List<int> listId, int userId)
        {
            ResponseBase responResult = null;
            try
            {
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return responResult;
        }
        public PagedList<T_Notification> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                var notifications = _repNotification.GetMany(c => !c.IsDeleted).Select(c => new T_Notification()
                {
                    Id = c.Id,
                    Description = c.Description,
                    CreatedDate = c.CreatedDate,
                }).OrderBy(sorting);
                var pageNumber = (startIndexRecord / pageSize) + 1;
                return new PagedList<T_Notification>(notifications, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

