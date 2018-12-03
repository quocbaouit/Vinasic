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
    public class BllSiteSetting : IBllSiteSetting
    {
        private readonly IT_SiteSettingRepository _repSiteSetting;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllSiteSetting(IUnitOfWork<VINASICEntities> unitOfWork, IT_SiteSettingRepository repSiteSetting)
        {
            _unitOfWork = unitOfWork;
            _repSiteSetting = repSiteSetting;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckSiteSettingName(string siteSettingName, int id)
        {
            var checkResult = false;
            var checkName = _repSiteSetting.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(siteSettingName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelSiteSetting> GetListProduct()
        {
            var siteSetting = _repSiteSetting.GetMany(c => !c.IsDeleted).Select(c => new ModelSiteSetting()
            {
                Id = c.Id,
                Code=c.Code,
                Name = c.Name,
                Description = c.Description,
                Value=c.Value,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return siteSetting;
        }
        public bool ChecConfig(string code)
        {
            var siteSetting = _repSiteSetting.GetMany(c => !c.IsDeleted && c.Code == code).FirstOrDefault()?.Value;
            return siteSetting == "true";
        }

        public ResponseBase Create(ModelSiteSetting obj)
        {
            ResponseBase result = new ResponseBase {IsSuccess = false};
            try
            {
                if (obj != null)
                {
                    if (CheckSiteSettingName(obj.Name, obj.Id))
                    {

                        var siteSetting = new T_SiteSetting();
                        Parse.CopyObject(obj, ref siteSetting);
                        siteSetting.CreatedDate = DateTime.Now.AddHours(14);
                        _repSiteSetting.Add(siteSetting);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create SiteSetting", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create SiteSetting", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create SiteSetting", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelSiteSetting obj)
        {

            ResponseBase result = new ResponseBase {IsSuccess = false};
            if (!CheckSiteSettingName(obj.Name, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateSiteSetting", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_SiteSetting siteSetting = _repSiteSetting.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (siteSetting != null)
                {
                    //siteSetting.Code = obj.Code;
                    siteSetting.Value = obj.Value;
                    siteSetting.Name = obj.Name;
                    siteSetting.Description = obj.Description;
                    siteSetting.UpdatedDate = DateTime.Now.AddHours(14);
                    siteSetting.UpdatedUser = obj.UpdatedUser;
                    _repSiteSetting.Update(siteSetting);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateSiteSetting", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var siteSetting = _repSiteSetting.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (siteSetting != null)
            {
                siteSetting.IsDeleted = true;
                siteSetting.DeletedUser = userId;
                siteSetting.DeletedDate = DateTime.Now.AddHours(14);
                _repSiteSetting.Update(siteSetting);
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
        public List<ModelSelectItem> GetListSiteSetting()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            listModelSelect.AddRange(_repSiteSetting.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            return listModelSelect;
        }
        public PagedList<ModelSiteSetting> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var siteSettings = _repSiteSetting.GetMany(c => !c.IsDeleted).Select(c => new ModelSiteSetting()
            {
                Id = c.Id,
                Code=c.Code,
                Value = c.Value,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelSiteSetting>(siteSettings, pageNumber, pageSize);
        }
    }
}

