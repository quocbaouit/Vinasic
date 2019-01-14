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
    public class BllProduct : IBllProduct
    {
        private readonly IT_ProductRepository _repProduct;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllProduct(IUnitOfWork<VINASICEntities> unitOfWork, IT_ProductRepository repProduct)
        {
            _unitOfWork = unitOfWork;
            _repProduct = repProduct;
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
        private bool CheckProductName(string productName, int Id)
        {
            var checkResult = false;
            try
            {
                var checkName = _repProduct.GetMany(c => !c.IsDeleted && c.Id != Id && c.Name.Trim().ToUpper().Equals(productName.Trim().ToUpper())).FirstOrDefault();
                if (checkName == null)
                    checkResult = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return checkResult;
        }
        public ResponseBase Create(ModelProduct obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckProductName(obj.Name, obj.Id))
                    {

                        var product = new T_Product();
                        Parse.CopyObject(obj, ref product);
                        product.CreatedDate = DateTime.Now.AddHours(14);
                        _repProduct.Add(product);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create Product", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Product", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase Update(ModelProduct obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (!CheckProductName(obj.Name, obj.Id))
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateProduct", Message = "Trùng Tên. Vui lòng chọn lại" });
                }
                else
                {
                    T_Product product = _repProduct.Get(x => x.Id == obj.Id && !x.IsDeleted);
                    if (product != null)
                    {
                        product.Code = obj.Code;
                        product.Name = obj.Name;
                        product.ProductTypeId = obj.ProductTypeId;
                        product.OrderIndex = obj.OrderIndex;
                        product.Description = obj.Description;
                        product.UpdatedDate = DateTime.Now.AddHours(14);
                        product.UpdatedUser = obj.UpdatedUser;
                        _repProduct.Update(product);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "UpdateProduct", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
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
                var product = _repProduct.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
                if (product != null)
                {
                    product.IsDeleted = true;
                    product.DeletedUser = userId;
                    product.DeletedDate = DateTime.Now.AddHours(14);
                    _repProduct.Update(product);
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
        public List<ModelSelectItem> GetListProduct(int productTypeId)
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Dịch Vụ----"}
            };
            try
            {
                if (productTypeId==0)
                {
                    listModelSelect.AddRange(_repProduct.GetMany(x => !x.IsDeleted).OrderBy(c=>c.OrderIndex).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
                }
                else
                {
                    listModelSelect.AddRange(_repProduct.GetMany(x => !x.IsDeleted && x.ProductTypeId == productTypeId).OrderBy(c => c.OrderIndex).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModelSelect;
        }
        public PagedList<ModelProduct> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                var products = _repProduct.GetMany(c => !c.IsDeleted).Select(c => new ModelProduct()
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    OrderIndex=c.OrderIndex,
                    Description = c.Description,
                    ProductTypeName=c.T_ProductType.Name,
                    ProductTypeId = c.ProductTypeId,
                    CreatedDate = c.CreatedDate
                }).OrderBy(sorting);
                var pageNumber = (startIndexRecord / pageSize) + 1;
                return new PagedList<ModelProduct>(products, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

