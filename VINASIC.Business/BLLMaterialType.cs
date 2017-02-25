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
    public class BllMaterialType : IBllMaterialType
    {
        private readonly IT_MaterialTypeRepository _repMaterialType;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllMaterialType(IUnitOfWork<VINASICEntities> unitOfWork, IT_MaterialTypeRepository repMaterialType)
        {
            _unitOfWork = unitOfWork;
            _repMaterialType = repMaterialType;
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
        private bool CheckMaterialTypeName(string materialTypeName, int Id)
        {
            var checkResult = false;
            try
            {
                var checkName = _repMaterialType.GetMany(c => !c.IsDeleted && c.Id != Id && c.Name.Trim().ToUpper().Equals(materialTypeName.Trim().ToUpper())).FirstOrDefault();
                if (checkName == null)
                    checkResult = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return checkResult;
        }
        public List<ModelMaterialType> GetListProduct()
        {
            List<ModelMaterialType> materialType;
            try
            {
                materialType = _repMaterialType.GetMany(c => !c.IsDeleted).Select(c => new ModelMaterialType()
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedDate = c.CreatedDate,
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return materialType;
        }
        public ResponseBase Create(ModelMaterialType obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckMaterialTypeName(obj.Name, obj.Id))
                    {

                        var materialType = new T_MaterialType();
                        Parse.CopyObject(obj, ref materialType);
                        materialType.CreatedDate = DateTime.Now.AddHours(14);
                        _repMaterialType.Add(materialType);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create MaterialType", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create MaterialType", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase Update(ModelMaterialType obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (!CheckMaterialTypeName(obj.Name, obj.Id))
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateMaterialType", Message = "Trùng Tên. Vui lòng chọn lại" });
                }
                else
                {
                    T_MaterialType materialType = _repMaterialType.Get(x => x.Id == obj.Id && !x.IsDeleted);
                    if (materialType != null)
                    {
                        materialType.Code = obj.Code;
                        materialType.Name = obj.Name;
                        materialType.Description = obj.Description;
                        materialType.UpdatedDate = DateTime.Now.AddHours(14);
                        materialType.UpdatedUser = obj.UpdatedUser;
                        _repMaterialType.Update(materialType);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "UpdateMaterialType", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                    }
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
                var materialType = _repMaterialType.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
                if (materialType != null)
                {
                    materialType.IsDeleted = true;
                    materialType.DeletedUser = userId;
                    materialType.DeletedDate = DateTime.Now.AddHours(14);
                    _repMaterialType.Update(materialType);
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
        public List<ModelSelectItem> GetListMaterialType()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            try
            {
                listModelSelect.AddRange(_repMaterialType.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModelSelect;
        }
        public PagedList<ModelMaterialType> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                var materialTypes = _repMaterialType.GetMany(c => !c.IsDeleted).Select(c => new ModelMaterialType()
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedDate = c.CreatedDate,
                }).OrderBy(sorting);
                var pageNumber = (startIndexRecord / pageSize) + 1;
                return new PagedList<ModelMaterialType>(materialTypes, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

