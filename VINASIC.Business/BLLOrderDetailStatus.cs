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
    public class BllOrderDetailStatus : IBllOrderDetailStatus
    {
        private readonly IT_OrderDetailStatusRepository _repOrderDetailStatus;
        private readonly IT_OrderDetailStatusPrintRepository _repOrderDetailStatusPrint;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllOrderDetailStatus(IUnitOfWork<VINASICEntities> unitOfWork, IT_OrderDetailStatusRepository repOrderDetailStatus, IT_OrderDetailStatusPrintRepository repOrderDetailStatusPrint)
        {
            _unitOfWork = unitOfWork;
            _repOrderDetailStatus = repOrderDetailStatus;
            _repOrderDetailStatusPrint = repOrderDetailStatusPrint;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckOrderDetailStatusName(string orderDetailStatusName, int id)
        {
            var checkResult = false;
            var checkName = _repOrderDetailStatus.GetMany(c => !c.IsDeleted && c.Id != id && c.StatusName.Trim().ToUpper().Equals(orderDetailStatusName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelOrderDetailStatus> GetListProduct()
        {
            var orderDetailStatus = _repOrderDetailStatus.GetMany(c => !c.IsDeleted).Select(c => new ModelOrderDetailStatus()
            {
                Id = c.Id,
                //Code = c.Code,
                StatusName = c.StatusName,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return orderDetailStatus;
        }

        public ResponseBase Create(ModelOrderDetailStatus obj)
        {
            ResponseBase result = new ResponseBase {IsSuccess = false};
            try
            {
                if (obj != null)
                {
                    if (CheckOrderDetailStatusName(obj.StatusName, obj.Id))
                    {

                        var orderDetailStatus = new T_OrderDetailStatus();
                        Parse.CopyObject(obj, ref orderDetailStatus);
                        orderDetailStatus.CreatedDate = DateTime.Now.AddHours(14);
                        _repOrderDetailStatus.Add(orderDetailStatus);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create OrderDetailStatus", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create OrderDetailStatus", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create OrderDetailStatus", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelOrderDetailStatus obj)
        {

            ResponseBase result = new ResponseBase {IsSuccess = false};
            if (!CheckOrderDetailStatusName(obj.StatusName, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateOrderDetailStatus", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_OrderDetailStatus orderDetailStatus = _repOrderDetailStatus.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (orderDetailStatus != null)
                {
                    orderDetailStatus.StatusName = obj.StatusName;
                    orderDetailStatus.Description = obj.Description;
                    orderDetailStatus.UpdatedDate = DateTime.Now.AddHours(14);
                    orderDetailStatus.UpdatedUser = obj.UpdatedUser;
                    _repOrderDetailStatus.Update(orderDetailStatus);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateOrderDetailStatus", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var orderDetailStatus = _repOrderDetailStatus.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (orderDetailStatus != null)
            {
                orderDetailStatus.IsDeleted = true;
                orderDetailStatus.DeletedUser = userId;
                orderDetailStatus.DeletedDate = DateTime.Now.AddHours(14);
                _repOrderDetailStatus.Update(orderDetailStatus);
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
        public List<ModelSelectItem> GetListOrderDetailStatus()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                //new ModelSelectItem() {Value = 0, Name = "-------"}
            };
            listModelSelect.AddRange(_repOrderDetailStatus.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.StatusName }));
            return listModelSelect;
        }
        public PagedList<ModelOrderDetailStatus> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var orderDetailStatuss = _repOrderDetailStatus.GetMany(c => !c.IsDeleted && c.Id!=1).Select(c => new ModelOrderDetailStatus()
            {
                Id = c.Id,
                StatusName = c.StatusName,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelOrderDetailStatus>(orderDetailStatuss, pageNumber, pageSize);
        }


        private bool CheckOrderDetailStatusNamePrint(string orderDetailStatusName, int id)
        {
            var checkResult = false;
            var checkName = _repOrderDetailStatusPrint.GetMany(c => !c.IsDeleted && c.Id != id && c.StatusName.Trim().ToUpper().Equals(orderDetailStatusName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelOrderDetailStatusPrint> GetListProductPrint()
        {
            var orderDetailStatus = _repOrderDetailStatusPrint.GetMany(c => !c.IsDeleted).Select(c => new ModelOrderDetailStatusPrint()
            {
                Id = c.Id,
                //Code = c.Code,
                StatusName = c.StatusName,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return orderDetailStatus;
        }

        public ResponseBase CreatePrint(ModelOrderDetailStatusPrint obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckOrderDetailStatusNamePrint(obj.StatusName, obj.Id))
                    {

                        var orderDetailStatus = new T_OrderDetailStatusPrint();
                        Parse.CopyObject(obj, ref orderDetailStatus);
                        orderDetailStatus.CreatedDate = DateTime.Now.AddHours(14);
                        _repOrderDetailStatusPrint.Add(orderDetailStatus);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create OrderDetailStatus", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create OrderDetailStatus", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create OrderDetailStatus", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase UpdatePrint(ModelOrderDetailStatusPrint obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            if (!CheckOrderDetailStatusNamePrint(obj.StatusName, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateOrderDetailStatus", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_OrderDetailStatusPrint orderDetailStatus = _repOrderDetailStatusPrint.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (orderDetailStatus != null)
                {
                    orderDetailStatus.StatusName = obj.StatusName;
                    orderDetailStatus.Description = obj.Description;
                    orderDetailStatus.UpdatedDate = DateTime.Now.AddHours(14);
                    orderDetailStatus.UpdatedUser = obj.UpdatedUser;
                    _repOrderDetailStatusPrint.Update(orderDetailStatus);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateOrderDetailStatus", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteByIdPrint(int id, int userId)
        {
            var responResult = new ResponseBase();
            var orderDetailStatus = _repOrderDetailStatusPrint.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (orderDetailStatus != null)
            {
                orderDetailStatus.IsDeleted = true;
                orderDetailStatus.DeletedUser = userId;
                orderDetailStatus.DeletedDate = DateTime.Now.AddHours(14);
                _repOrderDetailStatusPrint.Update(orderDetailStatus);
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
        public List<ModelSelectItem> GetListOrderDetailStatusPrint()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                //new ModelSelectItem() {Value = 0, Name = "-------"}
            };
            listModelSelect.AddRange(_repOrderDetailStatusPrint.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.StatusName }));
            return listModelSelect;
        }
        public PagedList<ModelOrderDetailStatusPrint> GetListPrint(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var orderDetailStatuss = _repOrderDetailStatusPrint.GetMany(c => !c.IsDeleted && c.Id != 1).Select(c => new ModelOrderDetailStatusPrint()
            {
                Id = c.Id,
                StatusName = c.StatusName,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelOrderDetailStatusPrint>(orderDetailStatuss, pageNumber, pageSize);
        }
    }
}

