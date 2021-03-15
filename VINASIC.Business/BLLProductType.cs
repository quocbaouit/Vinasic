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
    public class BllProductType : IBllProductType
    {
        private readonly IT_ProductTypeRepository _repProductType;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllProductType(IUnitOfWork<VINASICEntities> unitOfWork, IT_ProductTypeRepository repProductType)
        {
            _unitOfWork = unitOfWork;
            _repProductType = repProductType;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckProductTypeName(string productTypeName, int id)
        {
            var checkResult = false;
            var checkName = _repProductType.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(productTypeName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelProductType> GetListProduct()
        {
            var productType = _repProductType.GetMany(c => !c.IsDeleted).Select(c => new ModelProductType()
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return productType;
        }

        public ResponseBase Create(ModelProductType obj)
        {
            ResponseBase result = new ResponseBase {IsSuccess = false};
            try
            {
                if (obj != null)
                {
                    if (CheckProductTypeName(obj.Name, obj.Id))
                    {

                        var productType = new T_ProductType();
                        Parse.CopyObject(obj, ref productType);
                        productType.CreatedDate = DateTime.Now.AddHours(14);
                        _repProductType.Add(productType);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create ProductType", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create ProductType", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create ProductType", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelProductType obj)
        {

            ResponseBase result = new ResponseBase {IsSuccess = false};
            if (!CheckProductTypeName(obj.Name, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateProductType", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_ProductType productType = _repProductType.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (productType != null)
                {
                    productType.Code = obj.Code;
                    productType.Name = obj.Name;
                    productType.Description = obj.Description;
                    productType.UpdatedDate = DateTime.Now.AddHours(14);
                    productType.UpdatedUser = obj.UpdatedUser;
                    _repProductType.Update(productType);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateProductType", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var productType = _repProductType.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (productType != null)
            {
                productType.IsDeleted = true;
                productType.DeletedUser = userId;
                productType.DeletedDate = DateTime.Now.AddHours(14);
                _repProductType.Update(productType);
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
        public List<ModelSelectItem> GetListProductType()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            listModelSelect.AddRange(_repProductType.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            return listModelSelect;
        }

   
        public PagedList<ModelProductType> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var productTypes = _repProductType.GetMany(c => !c.IsDeleted).Select(c => new ModelProductType()
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelProductType>(productTypes, pageNumber, pageSize);
        }
    }
}

