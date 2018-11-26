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
    public class BllSysStatus : IBllSysStatus
    {
        private readonly IT_StatusRepository _repSysStatus;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllSysStatus(IUnitOfWork<VINASICEntities> unitOfWork, IT_StatusRepository repSysStatus)
        {
            _unitOfWork = unitOfWork;
            _repSysStatus = repSysStatus;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckSysStatusName(string sysStatusName, int id)
        {
            var checkResult = false;
            var checkName = _repSysStatus.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(sysStatusName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelSysStatus> GetListProduct()
        {
            var sysStatus = _repSysStatus.GetMany(c => !c.IsDeleted).Select(c => new ModelSysStatus()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return sysStatus;
        }

        public ResponseBase Create(ModelSysStatus obj)
        {
            ResponseBase result = new ResponseBase {IsSuccess = false};
            try
            {
                if (obj != null)
                {
                    if (CheckSysStatusName(obj.Name, obj.Id))
                    {

                        var sysStatus = new T_Status();
                        Parse.CopyObject(obj, ref sysStatus);
                        sysStatus.CreatedDate = DateTime.Now.AddHours(14);
                        _repSysStatus.Add(sysStatus);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create SysStatus", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create SysStatus", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create SysStatus", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelSysStatus obj)
        {

            ResponseBase result = new ResponseBase {IsSuccess = false};
            if (!CheckSysStatusName(obj.Name, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateSysStatus", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_Status sysStatus = _repSysStatus.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (sysStatus != null)
                {
                    sysStatus.Name = obj.Name;
                    sysStatus.Description = obj.Description;
                    sysStatus.UpdatedDate = DateTime.Now.AddHours(14);
                    sysStatus.UpdatedUser = obj.UpdatedUser;
                    _repSysStatus.Update(sysStatus);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateSysStatus", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var sysStatus = _repSysStatus.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (sysStatus != null)
            {
                sysStatus.IsDeleted = true;
                sysStatus.DeletedUser = userId;
                sysStatus.DeletedDate = DateTime.Now.AddHours(14);
                _repSysStatus.Update(sysStatus);
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
        public List<ModelSelectItem> GetListSysStatus()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            listModelSelect.AddRange(_repSysStatus.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            return listModelSelect;
        }
        public PagedList<ModelSysStatus> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var sysStatuss = _repSysStatus.GetMany(c => !c.IsDeleted).Select(c => new ModelSysStatus()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelSysStatus>(sysStatuss, pageNumber, pageSize);
        }
    }
}

