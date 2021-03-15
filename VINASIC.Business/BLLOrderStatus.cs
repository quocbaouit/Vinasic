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

namespace VINASIC.Business
{
    public class BllOrderStatus : IBllOrderStatus
    {
        private readonly IT_OrderStatusRepository _repOrderStatus;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllOrderStatus(IUnitOfWork<VINASICEntities> unitOfWork, IT_OrderStatusRepository repOrderStatus)
        {
            _unitOfWork = unitOfWork;
            _repOrderStatus = repOrderStatus;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckOrderStatusName(string orderStatusName, int id)
        {
            var checkResult = false;
            var checkName = _repOrderStatus.GetMany(c => !c.IsDeleted && c.Id != id && c.StatusName.Trim().ToUpper().Equals(orderStatusName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelOrderStatus> GetListProduct()
        {
            var orderStatus = _repOrderStatus.GetMany(c => !c.IsDeleted).Select(c => new ModelOrderStatus()
            {
                Id = c.Id,
                //Code = c.Code,
                StatusName = c.StatusName,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return orderStatus;
        }

        public ResponseBase Create(ModelOrderStatus obj)
        {
            ResponseBase result = new ResponseBase {IsSuccess = false};
            try
            {
                if (obj != null)
                {
                    if (CheckOrderStatusName(obj.StatusName, obj.Id))
                    {

                        var orderStatus = new T_OrderStatus();
                        Parse.CopyObject(obj, ref orderStatus);
                        orderStatus.CreatedDate = DateTime.Now.AddHours(14);
                        _repOrderStatus.Add(orderStatus);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create OrderStatus", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create OrderStatus", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create OrderStatus", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelOrderStatus obj)
        {

            ResponseBase result = new ResponseBase {IsSuccess = false};
            if (!CheckOrderStatusName(obj.StatusName, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateOrderStatus", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_OrderStatus orderStatus = _repOrderStatus.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (orderStatus != null)
                {
                    orderStatus.StatusName = obj.StatusName;
                    orderStatus.Description = obj.Description;
                    orderStatus.UpdatedDate = DateTime.Now.AddHours(14);
                    orderStatus.UpdatedUser = obj.UpdatedUser;
                    _repOrderStatus.Update(orderStatus);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateOrderStatus", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var orderStatus = _repOrderStatus.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (orderStatus != null)
            {
                orderStatus.IsDeleted = true;
                orderStatus.DeletedUser = userId;
                orderStatus.DeletedDate = DateTime.Now.AddHours(14);
                _repOrderStatus.Update(orderStatus);
                SaveChange();
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Delete", Message = "Đối Tượng Đã Bị Xóa,Vui Lòng Kiểm Tra Lại" });
            }
            return responResult;
        }
        public List<ModelSelectItem> GetListOrderStatus()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                //new ModelSelectItem() {Value = 0, Name = "-------"}
            };
            listModelSelect.AddRange(_repOrderStatus.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.StatusName }));
            return listModelSelect;
        }
        public PagedList<ModelOrderStatus> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var orderStatuss = _repOrderStatus.GetMany(c => !c.IsDeleted && c.Id!=1).Select(c => new ModelOrderStatus()
            {
                Id = c.Id,
                StatusName = c.StatusName,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelOrderStatus>(orderStatuss, pageNumber, pageSize);
        }
    }
}

