using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    public class BllCustomer : IBllCustomer
    {
        private readonly IT_CustomerRepository _repCustomer;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllCustomer(IUnitOfWork<VINASICEntities> unitOfWork, IT_CustomerRepository repCustomer)
        {
            _unitOfWork = unitOfWork;
            _repCustomer = repCustomer;
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
        private bool CheckCustomerName(string customerName, int id)
        {
            var checkResult = false;
            try
            {
                var checkName = _repCustomer.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(customerName.Trim().ToUpper())).FirstOrDefault();
                if (checkName == null)
                    checkResult = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return checkResult;
        }
        public List<ModelCustomer> GetListProduct()
        {
            List<ModelCustomer> customer;
            try
            {
                customer = _repCustomer.GetMany(c => !c.IsDeleted).Select(c => new ModelCustomer()
                {
                    Id = c.Id,

                    Name = c.Name,
                    CreatedDate = c.CreatedDate,
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return customer;
        }
        public ResponseBase Create(ModelCustomer obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckCustomerName(obj.Name, obj.Id))
                    {

                        var customer = new T_Customer();
                        Parse.CopyObject(obj, ref customer);
                        customer.CreatedDate = DateTime.Now.AddHours(14);
                        _repCustomer.Add(customer);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create Customer", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Customer", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase Update(ModelCustomer obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {

                    T_Customer customer = _repCustomer.Get(x => x.Id == obj.Id && !x.IsDeleted);
                    if (customer != null)
                    {
                        customer.Name = obj.Name;
                        customer.Address = obj.Address;
                        customer.Email = obj.Email;
                        customer.Mobile = obj.Mobile;
                        customer.TaxCode = obj.TaxCode;
                        customer.UpdatedDate = DateTime.Now.AddHours(14);
                        customer.UpdatedUser = obj.UpdatedUser;
                        _repCustomer.Update(customer);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "UpdateCustomer", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
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
                var customer = _repCustomer.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
                if (customer != null)
                {
                    customer.IsDeleted = true;
                    customer.DeletedUser = userId;
                    customer.DeletedDate = DateTime.Now.AddHours(14);
                    _repCustomer.Update(customer);
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
        public List<ModelSelectItem> GetListCustomer()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Khách Hàng----"}
            };
            try
            {
                listModelSelect.AddRange(_repCustomer.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModelSelect;
        }
        public PagedList<ModelCustomer> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                var customers = _repCustomer.GetMany(c => !c.IsDeleted).Select(c => new ModelCustomer()
                {
                    Id = c.Id,
                    Email = c.Email,
                    Address = c.Address,
                    Name = c.Name,
                    Mobile = c.Mobile,
                    TaxCode = c.TaxCode,
                    CreatedDate = c.CreatedDate
                }).OrderBy(sorting);
                if (!string.IsNullOrEmpty(keyWord))
                {
                    customers = customers.Where(x => x.Name.Contains(keyWord));
                }
                var pageNumber = (startIndexRecord / pageSize) + 1;
                return new PagedList<ModelCustomer>(customers, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetAllCustomerName()
        {
            List<string> strCustomer = _repCustomer.GetMany(x => !x.IsDeleted).Select(x => x.Name).Distinct().ToList();
            return strCustomer;
        }
        public T_Customer GetCustomerById(int id)
        {
            var customer = _repCustomer.Get(x => !x.IsDeleted && x.Id==id);
            return customer;
        }
        public T_Customer GetCustomerByName(string name)
        {
            var customer = _repCustomer.Get(x => !x.IsDeleted && x.Name.Trim() == name.Trim());
            return customer;
        }
        public T_Customer GetCustomerByPhone(string phone)
        {
            var customer = _repCustomer.Get(x => !x.IsDeleted && x.Mobile.Trim() == phone.Trim());
            return customer;
        }
    }
}

