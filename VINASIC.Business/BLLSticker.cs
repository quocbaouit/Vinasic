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
    public class BllSticker : IBllSticker
    {
        private readonly IT_StickerRepository _repSticker;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllSticker(IUnitOfWork<VINASICEntities> unitOfWork, IT_StickerRepository repSticker)
        {
            _unitOfWork = unitOfWork;
            _repSticker = repSticker;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckStickerName(string productTypeName, int id)
        {
            var checkResult = false;
            var checkName = _repSticker.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(productTypeName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelSticker> GetListProduct()
        {
            var productType = _repSticker.GetMany(c => !c.IsDeleted).Select(c => new ModelSticker()
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return productType;
        }

        public ResponseBase Create(ModelSticker obj)
        {
            ResponseBase result = new ResponseBase {IsSuccess = false};
            try
            {
                if (obj != null)
                {
                    if (CheckStickerName(obj.Name, obj.Id))
                    {

                        var productType = new T_Sticker();
                        Parse.CopyObject(obj, ref productType);
                        productType.CreatedDate = DateTime.Now.AddHours(14);
                        _repSticker.Add(productType);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create Sticker", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Sticker", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create Sticker", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelSticker obj)
        {

            ResponseBase result = new ResponseBase {IsSuccess = false};
            if (!CheckStickerName(obj.Name, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateSticker", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_Sticker productType = _repSticker.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (productType != null)
                {
                    productType.Code = obj.Code;
                    productType.Name = obj.Name;
                    productType.UpdatedDate = DateTime.Now.AddHours(14);
                    productType.UpdatedUser = obj.UpdatedUser;
                    _repSticker.Update(productType);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateSticker", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var productType = _repSticker.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (productType != null)
            {
                productType.IsDeleted = true;
                productType.DeletedUser = userId;
                productType.DeletedDate = DateTime.Now.AddHours(14);
                _repSticker.Update(productType);
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
        public List<ModelSelectItem> GetListSticker()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            listModelSelect.AddRange(_repSticker.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            return listModelSelect;
        }
        public PagedList<ModelSticker> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var productTypes = _repSticker.GetMany(c => !c.IsDeleted).Select(c => new ModelSticker()
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelSticker>(productTypes, pageNumber, pageSize);
        }

        public List<T_Sticker> GetAllSticker()
        {
            return _repSticker.GetMany(x => !x.IsDeleted).ToList();
        }
    }
}

