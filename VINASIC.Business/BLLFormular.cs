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
    public class BllFormular : IBllFormular
    {
        private readonly IT_FormularRepository _repFormular;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllFormular(IUnitOfWork<VINASICEntities> unitOfWork, IT_FormularRepository repFormular)
        {
            _unitOfWork = unitOfWork;
            _repFormular = repFormular;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckFormularName(string formularName, int id)
        {
            var checkResult = false;
            var checkName = _repFormular.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(formularName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelFormular> GetListProduct()
        {
            var formular = _repFormular.GetMany(c => !c.IsDeleted).Select(c => new ModelFormular()
            {
                Id = c.Id,           
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return formular;
        }

        public ResponseBase Create(ModelFormular obj)
        {
            ResponseBase result = new ResponseBase {IsSuccess = false};
            try
            {
                if (obj != null)
                {
                    if (CheckFormularName(obj.Name, obj.Id))
                    {

                        var formular = new T_Formular();
                        Parse.CopyObject(obj, ref formular);
                        formular.CreatedDate = DateTime.Now.AddHours(14);
                        _repFormular.Add(formular);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create Formular", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Formular", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create Formular", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelFormular obj)
        {

            ResponseBase result = new ResponseBase {IsSuccess = false};
            if (!CheckFormularName(obj.Name, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateFormular", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_Formular formular = _repFormular.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (formular != null)
                {
                    formular.Name = obj.Name;
                    formular.Description = obj.Description;
                    formular.UpdatedDate = DateTime.Now.AddHours(14);
                    formular.UpdatedUser = obj.UpdatedUser;
                    _repFormular.Update(formular);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateFormular", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var formular = _repFormular.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (formular != null)
            {
                formular.IsDeleted = true;
                formular.DeletedUser = userId;
                formular.DeletedDate = DateTime.Now.AddHours(14);
                _repFormular.Update(formular);
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
        public List<ModelSelectItem> GetListFormular()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            listModelSelect.AddRange(_repFormular.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            return listModelSelect;
        }
        public PagedList<ModelFormular> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var formulars = _repFormular.GetMany(c => !c.IsDeleted).Select(c => new ModelFormular()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelFormular>(formulars, pageNumber, pageSize);
        }
    }
}

