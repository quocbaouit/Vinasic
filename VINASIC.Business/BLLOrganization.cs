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
    public class BllOrganization : IBllOrganization
    {
        private readonly IT_OrganizationRepository _repOrganization;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllOrganization(IUnitOfWork<VINASICEntities> unitOfWork, IT_OrganizationRepository repOrganization)
        {
            _unitOfWork = unitOfWork;
            _repOrganization = repOrganization;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckOrganizationName(string organizationName, int id)
        {
            var checkResult = false;
            var checkName = _repOrganization.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(organizationName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelOrganization> GetListProduct()
        {
            var organization = _repOrganization.GetMany(c => !c.IsDeleted).Select(c => new ModelOrganization()
            {
                Id = c.Id,
                ShortName = c.ShortName,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return organization;
        }

        public ResponseBase Create(ModelOrganization obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckOrganizationName(obj.Name, obj.Id))
                    {

                        var organization = new T_Organization();
                        Parse.CopyObject(obj, ref organization);
                        organization.CreatedDate = DateTime.Now.AddHours(14);
                        _repOrganization.Add(organization);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create Organization", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Organization", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create Organization", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelOrganization obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            if (!CheckOrganizationName(obj.Name, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateOrganization", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_Organization organization = _repOrganization.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (organization != null)
                {
                    //organization.ShortName = obj.ShortName;
                    organization.Name = obj.Name;
                    organization.Description = obj.Description;
                    organization.UpdatedDate = DateTime.Now.AddHours(14);
                    organization.UpdatedUser = obj.UpdatedUser;
                    _repOrganization.Update(organization);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateOrganization", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var organization = _repOrganization.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (organization != null)
            {
                organization.IsDeleted = true;
                organization.DeletedUser = userId;
                organization.DeletedDate = DateTime.Now.AddHours(14);
                _repOrganization.Update(organization);
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

        public List<ModelSelectItem> GetListOrganization()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            listModelSelect.AddRange(_repOrganization.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            return listModelSelect;
        }
        public PagedList<ModelOrganization> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var organizations = _repOrganization.GetMany(c => !c.IsDeleted).Select(c => new ModelOrganization()
            {
                Id = c.Id,
                Name = c.Name,
                ShortName = c.ShortName,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelOrganization>(organizations, pageNumber, pageSize);
        }
    }
}

