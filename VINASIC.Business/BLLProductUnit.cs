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
    public class BllProductUnit : IBllProductUnit
    {
        private readonly IT_UnitRepository _repProductUnit;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllProductUnit(IUnitOfWork<VINASICEntities> unitOfWork, IT_UnitRepository repProductUnit)
        {
            _unitOfWork = unitOfWork;
            _repProductUnit = repProductUnit;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckProductUnitName(string productUnitName, int id)
        {
            var checkResult = false;
            var checkName = _repProductUnit.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(productUnitName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelProductUnit> GetListProduct()
        {
            var productUnit = _repProductUnit.GetMany(c => !c.IsDeleted).Select(c => new ModelProductUnit()
            {
                Id = c.Id,
                //Code = c.Code,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return productUnit;
        }

        public ResponseBase Create(ModelProductUnit obj)
        {
            ResponseBase result = new ResponseBase {IsSuccess = false};
            try
            {
                if (obj != null)
                {
                    if (CheckProductUnitName(obj.Name, obj.Id))
                    {

                        var productUnit = new T_Unit();
                        Parse.CopyObject(obj, ref productUnit);
                        productUnit.CreatedDate = DateTime.Now.AddHours(14);
                        _repProductUnit.Add(productUnit);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create ProductUnit", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create ProductUnit", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create ProductUnit", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelProductUnit obj)
        {

            ResponseBase result = new ResponseBase {IsSuccess = false};
            if (!CheckProductUnitName(obj.Name, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateProductUnit", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_Unit productUnit = _repProductUnit.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (productUnit != null)
                {
                    //productUnit.Code = obj.Code;
                    productUnit.Name = obj.Name;
                    productUnit.Description = obj.Description;
                    productUnit.UpdatedDate = DateTime.Now.AddHours(14);
                    productUnit.UpdatedUser = obj.UpdatedUser;
                    _repProductUnit.Update(productUnit);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateProductUnit", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var productUnit = _repProductUnit.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (productUnit != null)
            {
                productUnit.IsDeleted = true;
                productUnit.DeletedUser = userId;
                productUnit.DeletedDate = DateTime.Now.AddHours(14);
                _repProductUnit.Update(productUnit);
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
        public List<ModelSelectItem> GetListProductUnit()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            listModelSelect.AddRange(_repProductUnit.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            return listModelSelect;
        }
        public PagedList<ModelProductUnit> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var productUnits = _repProductUnit.GetMany(c => !c.IsDeleted).Select(c => new ModelProductUnit()
            {
                Id = c.Id,
                //Code = c.Code,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelProductUnit>(productUnits, pageNumber, pageSize);
        }
    }
}

