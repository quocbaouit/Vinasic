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
    public class BllPosition : IBllPosition
    {
        private readonly IT_PositionRepository _repPosition;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllPosition(IUnitOfWork<VINASICEntities> unitOfWork, IT_PositionRepository repPosition)
        {
            _unitOfWork = unitOfWork;
            _repPosition = repPosition;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckPositionName(string positionName, int id)
        {
            var checkResult = false;
            var checkName = _repPosition.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(positionName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelPosition> GetListProduct()
        {
            var position = _repPosition.GetMany(c => !c.IsDeleted).Select(c => new ModelPosition()
            {
                Id = c.Id,
                Name = c.Name,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return position;
        }

        public ResponseBase Create(ModelPosition obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckPositionName(obj.Name, obj.Id))
                    {

                        var position = new T_Position();
                        Parse.CopyObject(obj, ref position);
                        position.CreatedDate = DateTime.Now.AddHours(14);
                        _repPosition.Add(position);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create Position", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Position", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create Position", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelPosition obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            if (!CheckPositionName(obj.Name, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdatePosition", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_Position position = _repPosition.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (position != null)
                {
                    position.Name = obj.Name;
                    position.OrganizationId = obj.OrganizationId;
                    position.UpdatedDate = DateTime.Now.AddHours(14);
                    position.UpdatedUser = obj.UpdatedUser;
                    _repPosition.Update(position);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdatePosition", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var position = _repPosition.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (position != null)
            {
                position.IsDeleted = true;
                position.DeletedUser = userId;
                position.DeletedDate = DateTime.Now.AddHours(14);
                _repPosition.Update(position);
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
        public List<ModelSelectItem> GetListPosition()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Chọn Chức Vụ----"}
            };
            listModelSelect.AddRange(_repPosition.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            return listModelSelect;
        }
        public PagedList<ModelPosition> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var positions = _repPosition.GetMany(c => !c.IsDeleted).Select(c => new ModelPosition()
            {
                Id = c.Id,
                Name = c.Name,
                OrganizationId = c.OrganizationId,
                OrganizationName = c.T_Organization.Name,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelPosition>(positions, pageNumber, pageSize);
        }
    }
}

