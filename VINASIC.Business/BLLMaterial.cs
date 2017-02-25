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
    public class BllMaterial : IBllMaterial
    {
        private readonly IT_MaterialRepository _repMaterial;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllMaterial(IUnitOfWork<VINASICEntities> unitOfWork, IT_MaterialRepository repMaterial)
        {
            _unitOfWork = unitOfWork;
            _repMaterial = repMaterial;
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
        private bool CheckMaterialName(string materialName, int Id)
        {
            var checkResult = false;
            try
            {
                var checkName = _repMaterial.GetMany(c => !c.IsDeleted && c.Id != Id && c.Name.Trim().ToUpper().Equals(materialName.Trim().ToUpper())).FirstOrDefault();
                if (checkName == null)
                    checkResult = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return checkResult;
        }
        public ResponseBase Create(ModelMaterial obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckMaterialName(obj.Name, obj.Id))
                    {

                        var material = new T_Material();
                        Parse.CopyObject(obj, ref material);
                        material.CreatedDate = DateTime.Now.AddHours(14);
                        _repMaterial.Add(material);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create Material", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Material", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase Update(ModelMaterial obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (!CheckMaterialName(obj.Name, obj.Id))
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateMaterial", Message = "Trùng Tên. Vui lòng chọn lại" });
                }
                else
                {
                    T_Material material = _repMaterial.Get(x => x.Id == obj.Id && !x.IsDeleted);
                    if (material != null)
                    {
                        material.Code = obj.Code;
                        material.Name = obj.Name;
                        material.Description = obj.Description;
                        material.Inventory = obj.Inventory;
                        material.MaterialTypeId = obj.MaterialTypeId;
                        material.UpdatedDate = DateTime.Now.AddHours(14);
                        material.UpdatedUser = obj.UpdatedUser;
                        _repMaterial.Update(material);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "UpdateMaterial", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
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
                var material = _repMaterial.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
                if (material != null)
                {
                    material.IsDeleted = true;
                    material.DeletedUser = userId;
                    material.DeletedDate = DateTime.Now.AddHours(14);
                    _repMaterial.Update(material);
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
        public List<ModelSelectItem> GetListMaterial(int materialTypeId)
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Dịch Vụ----"}
            };
            try
            {
                if (materialTypeId==0)
                {
                    listModelSelect.AddRange(_repMaterial.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
                }
                else
                {
                    listModelSelect.AddRange(_repMaterial.GetMany(x => !x.IsDeleted && x.MaterialTypeId == materialTypeId).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModelSelect;
        }
        public PagedList<ModelMaterial> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                var materials = _repMaterial.GetMany(c => !c.IsDeleted).Select(c => new ModelMaterial()
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Description = c.Description,
                    MaterialName = c.T_MaterialType.Name,
                    MaterialTypeId = c.MaterialTypeId,
                    Inventory = c.Inventory,
                    CreatedDate = c.CreatedDate,
                }).OrderBy(sorting);
                var pageNumber = (startIndexRecord / pageSize) + 1;
                return new PagedList<ModelMaterial>(materials, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

