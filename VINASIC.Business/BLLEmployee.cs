﻿using System;
using System.Collections.Generic;
using System.Linq;
//using GPRO.Core.Mvc;
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
    public class BllEmployee : IBllEmployee
    {
        private readonly IT_UserRepository _repUser;
        private readonly IT_ProductRepository _repProduct;
        private readonly IT_UserProductRepository _repUserProduct;
        private readonly IT_PositionRepository _repPositionRepository;
        private readonly IT_OrderDetailRepository _repOrderDetailRepository;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        private readonly IBLLRole bllRole;
        public BllEmployee(IUnitOfWork<VINASICEntities> unitOfWork, IT_UserRepository repUser, IT_OrderDetailRepository repOrderDetailRepository, IT_PositionRepository repPositionRepository, IT_ProductRepository repProduct, IBLLRole _bllRole, IT_UserProductRepository repUserProduct)
        {
            _unitOfWork = unitOfWork;
            _repUser = repUser;
            _repProduct = repProduct;
            _repUserProduct = repUserProduct;
            _repOrderDetailRepository = repOrderDetailRepository;
            _repPositionRepository = repPositionRepository;
            bllRole = _bllRole;
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
        private bool CheckUserName(string employeeName, int id)
        {
            var checkResult = false;
            try
            {
                var checkName = _repUser.GetMany(c => !c.IsDeleted && c.Id != id && c.UserName.Trim().ToUpper().Equals(employeeName.Trim().ToUpper())).FirstOrDefault();
                if (checkName == null)
                    checkResult = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return checkResult;
        }

        public ResponseBase Create(ModelUser obj)
        {
            var allPos = _repPositionRepository.GetMany(x => !x.IsDeleted).ToList();

            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckUserName(obj.UserName, obj.Id))
                    {
                        obj.OrganizationId = allPos.Where(x => x.Id == obj.PositionId).Select(x => x.OrganizationId).FirstOrDefault();
                        var employee = new T_User();
                        Parse.CopyObject(obj, ref employee);
                        if (string.IsNullOrEmpty(obj.UserName))
                        {
                            employee.UserName = Guid.NewGuid().ToString();
                        }
                        else
                        {
                            employee.UserName = obj.UserName;
                        }
                        employee.PositionId = obj.PositionId;
                        employee.OrganizationId = obj.OrganizationId;
                        employee.PassWord = GlobalFunction.EncryptMD5("123456");
                        employee.FisrtName = obj.Name;
                        employee.IsLock = false;
                        employee.IsRequireChangePW = false;
                        employee.IsForgotPassword = false;
                        employee.CreatedDate = DateTime.Now.AddHours(14);
                        _repUser.Add(employee);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create", Message = "Tên Đăng Nhập Đã Tồn Tại" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseBase Update(ModelUser obj)
        {
            var allPos = _repPositionRepository.GetMany(x => !x.IsDeleted).ToList();
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (!CheckUserName(obj.UserName, obj.Id))
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create", Message = "Tên Đăng Nhập Đã Tồn Tại" });
                }
                else
                {
                    var employee = _repUser.Get(x => x.Id == obj.Id && !x.IsDeleted);
                    if (employee != null)
                    {
                        obj.OrganizationId = allPos.Where(x => x.Id == obj.PositionId).Select(x => x.OrganizationId).FirstOrDefault();
                        employee.Name = obj.Name;
                        employee.Address = obj.Address;
                        employee.Mobile = obj.Mobile;
                        employee.Email = obj.Email;
                        if (!string.IsNullOrEmpty(obj.UserName))
                        {
                            employee.UserName = obj.UserName; 
                        }
                        employee.FisrtName = obj.Name;
                        employee.OrganizationId = obj.OrganizationId;
                        employee.PositionId = obj.PositionId;
                        employee.IsLock = false;
                        employee.IsRequireChangePW = false;
                        employee.IsForgotPassword = false;
                        employee.UpdatedDate = DateTime.Now.AddHours(14);
                        employee.UpdatedUser = obj.UpdatedUser;
                        _repUser.Update(employee);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
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
                var employee = _repUser.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
                if (employee != null)
                {


                    employee.IsDeleted = true;
                    employee.DeletedUser = userId;
                    employee.DeletedDate = DateTime.Now.AddHours(14);
                    _repUser.Update(employee);
                    SaveChange();
                    responResult.IsSuccess = true;
                }
                else
                {
                    responResult.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responResult;
        }
        public ResponseBase DeleteByListId(List<int> listId, int userId)
        {
            ResponseBase responResult = null;
            try
            {
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return responResult;
        }
        public PagedList<ModelUser> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                var employees = _repUser.GetMany(c => !c.IsDeleted).Select(c => new ModelUser()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    stringRoleName="Chưa chọn nhóm quyền",
                    Mobile = c.Mobile,
                    Email = c.Email,
                    UserName = c.UserName,
                    PassWord=c.PassWord,
                    PositionId = c.PositionId,
                    PositionName = c.T_Position.Name,
                    CreatedDate = c.CreatedDate,
                }).OrderBy(sorting).ToList();
                
                foreach (var employ in employees)
                {
                    var listRole = bllRole.GetListRoleByUser(employ.Id);
                    if (listRole != null && listRole.Count > 0)
                    {
                        employ.stringRoleName = "";
                        var listRoleName = listRole.Select(x => x.RoleName).ToList();
                        foreach (var roleName in listRoleName)
                        {
                            if (employ.stringRoleName != "")
                            {
                                employ.stringRoleName = employ.stringRoleName + "</br>" + roleName;
                            }
                            else
                            {
                                employ.stringRoleName = roleName;
                            }
                            
                        }
                    }
                }
                var pageNumber = (startIndexRecord / pageSize) + 1;
                return new PagedList<ModelUser>(employees, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ModelSelectItem> GetListEmployee()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Chọn Hết----"}
            };

            try
            {
                listModelSelect.AddRange(_repUser.GetMany(x => !x.IsDeleted ).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModelSelect;
        }

        public T_User GetUserById(int id)
        {
            return _repUser.Get(x => x.Id == id);
        }

        public List<ModelSelectItem> GetCustomerByOrganization(string shortName,bool isAuthor,int userId)
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem> { };
            if (shortName == "PKD" && isAuthor == false)
            {
                try
                {
                    listModelSelect.AddRange(_repUser.GetMany(x => !x.IsDeleted &&x.Id== userId && x.T_Position.T_Organization.ShortName.Contains(shortName)).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    listModelSelect.Add( new ModelSelectItem() { Value = 0, Name = "---Chọn Hết----" });
                    listModelSelect.AddRange(_repUser.GetMany(x => !x.IsDeleted && x.T_Position.T_Organization.ShortName.Contains(shortName)).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return listModelSelect;
        }

        public PagedList<ModelForDesign> GetListForDesign(string keyWord, int startIndexRecord, int pageSize, string sorting, int userId, string fromDate, string toDate,bool auth,int emp)
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                    var realfromDate = DateTime.Parse(fromDate);
                    var realtoDate = DateTime.Parse(toDate);
                    var frDate = new DateTime(realfromDate.Year, realfromDate.Month, realfromDate.Day, 0, 0, 0, 0);
                    var tDate = new DateTime(realtoDate.Year, realtoDate.Month, realtoDate.Day, 23, 59, 59, 999);
                    var listDesignProcess =
                        _repOrderDetailRepository.GetMany(c => !c.IsDeleted&&c.DesignUser!=null&& c.CreatedDate >= frDate && c.CreatedDate <= tDate)
                            .Select(c => new ModelForDesign()
                            {
                                T_Order=c.T_Order,
                                CustomerName = c.T_Order.Name,
                                Id = c.Id,
                                EmployeeName = c.T_Order.T_User.Name,
                                CommodityName = c.CommodityName,
                                FileName = c.FileName,
                                Height = c.Height,
                                Width = c.Width,
                                DesignFrom = c.DesignFrom,
                                DesignTo = c.DesignTo,
                                DesignStatus = c.DesignStatus ?? 0,
                                DesignDescription = c.DesignDescription,
                                StrdesignStatus =
                                    c.DesignStatus == null
                                        ? "Không Xử Lý"
                                        : (c.DesignStatus == 1
                                            ? "Đang Xử Lý"
                                            : (c.DesignStatus == 2 ? "Đã Xử Lý" : "Không xử lý")),
                                CreatedDate = c.CreatedDate,
                            }).OrderBy(sorting).ToList();
                    if (!auth)
                    {
                        listDesignProcess = listDesignProcess.Where(x => x.DesignUser == userId).ToList();
                    }
                    if (!string.IsNullOrEmpty(keyWord))
                    {
                        listDesignProcess = listDesignProcess.Where(x => x.CustomerName.Contains(keyWord) ).ToList();
                    }
                if (emp!=0)
                {
                    listDesignProcess = listDesignProcess.Where(x => x.T_Order.CreatedForUser== emp).ToList();
                }
                var pageNumber = (startIndexRecord / pageSize) + 1;
                    return new PagedList<ModelForDesign>(listDesignProcess, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedList<ModelForPrint> GetListForPrint(string keyWord, int startIndexRecord, int pageSize, string sorting, int userId, string fromDate, string toDate,bool auth,int emp)
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                var userProduct =
                    _repUserProduct.GetMany(x => !x.IsDeleted && x.UserId == userId).Select(x => x.ProductId).ToList();
                    var realfromDate = DateTime.Parse(fromDate);
                    var realtoDate = DateTime.Parse(toDate);
                    var frDate = new DateTime(realfromDate.Year, realfromDate.Month, realfromDate.Day, 0, 0, 0, 0);
                    var tDate = new DateTime(realtoDate.Year, realtoDate.Month, realtoDate.Day, 23, 59, 59, 999);
                    var listPrintProcess =
                        _repOrderDetailRepository.GetMany(c =>!c.IsDeleted && c.CreatedDate >= frDate && c.CreatedDate <= tDate)
                            .Select(c => new ModelForPrint()
                            {
                                T_Order = c.T_Order,
                                PrintUser =c.PrintUser,
                                CommodityId = c.CommodityId,
                                CustomerName = c.T_Order.Name,
                                Id = c.Id,
                                PrintStatus = c.PrintStatus ?? 0,
                                EmployeeName = c.T_Order.T_User.Name,
                                CommodityName = c.CommodityName,
                                FileName = c.FileName,
                                Height = c.Height,
                                Width = c.Width,
                                PrintDescription = c.PrintDescription,
                                PrintFrom = c.PrintFrom,
                                PrintTo = c.PrintTo,
                                StrPrintStatus =
                                    c.PrintStatus == null
                                        ? "Chưa In"
                                        : (c.PrintStatus == 1
                                            ? "Đang In"
                                            : (c.PrintStatus == 2 ? "Đã Xong" : "Chưa In")),
                                CreatedDate = c.CreatedDate,
                            }).OrderBy(sorting);
                if (!auth)
                {
                    listPrintProcess = listPrintProcess.Where(c=>userProduct.Contains(c.CommodityId));
                }
                if (!string.IsNullOrEmpty(keyWord))
                    {
                        listPrintProcess = listPrintProcess.Where(x => x.CustomerName.Contains(keyWord));
                    }
                if (emp != 0)
                {
                    listPrintProcess = listPrintProcess.Where(x => x.T_Order.CreatedForUser == emp);
                }
                var pageNumber = (startIndexRecord / pageSize) + 1;
                    return new PagedList<ModelForPrint>(listPrintProcess, pageNumber, pageSize);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase DesignUpdateOrderDeatail(int id, int stautus, int userId)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (stautus >= 2)
                {
                    result.IsSuccess = false;
                    result.Data = "Đã Hoàn Thành Không Thể Cập nhật";
                    return result;
                }
                var orderDetail = _repOrderDetailRepository.Get(x => x.Id == id && !x.IsDeleted);
                orderDetail.DesignStatus = stautus + 1;
                orderDetail.UpatedDate = DateTime.Now.AddHours(14);
                orderDetail.UpdatedUser = userId;
                _repOrderDetailRepository.Update(orderDetail);
                SaveChange();
                result.IsSuccess = true;
                result.Data = DateTime.Now.AddHours(14);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase PrintUpdateOrderDeatail(int id, int stautus, int userId)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (stautus >= 2)
                {
                    result.IsSuccess = false;
                    result.Data = "Đã Hoàn Thành Không Thể Cập nhật";
                    return result;
                }
                var orderDetail = _repOrderDetailRepository.Get(x => x.Id == id && !x.IsDeleted);
                orderDetail.PrintStatus = stautus + 1;
                orderDetail.UpatedDate = DateTime.Now.AddHours(14);
                orderDetail.UpdatedUser = userId;
                _repOrderDetailRepository.Update(orderDetail);
                SaveChange();
                result.IsSuccess = true;
                result.Data = DateTime.Now.AddHours(14);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseBase ResetPass(int empId)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                var emp = _repUser.Get(x => x.Id == empId && !x.IsDeleted);
                emp.PassWord = GlobalFunction.EncryptMD5("123456");
                emp.UpdatedDate = DateTime.Now;
                _repUser.Update(emp);
                SaveChange();
                result.IsSuccess = true;
                result.Data = DateTime.Now.AddHours(14);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public ResponseBase BusinessUpdateOrderDeatail(int id, int employeeId, string description, int type, int stautus, int userId)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                var orderDetail = _repOrderDetailRepository.Get(x => x.Id == id && !x.IsDeleted);
                if (type == 1)
                {
                    orderDetail.DesignStatus = stautus;
                    orderDetail.DesignUser = employeeId;
                    orderDetail.DesignDescription = description;
                }
                else
                {
                    orderDetail.PrintStatus = stautus;
                    orderDetail.PrintUser = employeeId;
                    orderDetail.PrintDescription = description;
                }
                orderDetail.UpdatedUser = userId;
                orderDetail.UpatedDate = DateTime.Now.AddHours(14);
                _repOrderDetailRepository.Update(orderDetail);
                SaveChange();
                result.IsSuccess = true;
                result.Data = DateTime.Now.AddHours(14);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public PagedList<UserProduct> ListProductIdByUser(int userId, string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var listProduct = _repProduct.GetMany(x=>!x.IsDeleted).Select(x=>new UserProduct()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedDate = x.CreatedDate,
                Selected = false,              
                ProductTypeName = x.T_ProductType.Name,
            }).ToList();
            var listProductIdByUserId= _repUserProduct.GetMany(x => !x.IsDeleted && x.UserId == userId).Select(x=>x.ProductId).ToList();
            foreach (var product in listProduct)
            {
                if (listProductIdByUserId.Contains(product.Id))
                {
                    product.Selected = true;
                }
            }
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<UserProduct>(listProduct, pageNumber, pageSize);
        }

        public List<int> GetProductByUserId(int userId)
        {
            var listProduct =
                _repUserProduct.GetMany(x => !x.IsDeleted && x.UserId == userId).Select(x => x.ProductId).ToList();
            return listProduct;
        }
        public ResponseBase UpdateUserProduct(int userId, List<int> products)
        {
            if (products == null)
            {
                products = new List<int> { 0 };
            }
            ResponseBase result = new ResponseBase { IsSuccess = false };

            var insertProduct = new List<T_Product>();
            var deleteUserProduct = new List<T_UserProduct>();

            var existUserProduct = _repUserProduct.GetMany(x => x.UserId == userId && !x.IsDeleted);
            var existUserProductId = existUserProduct.Select(x => x.ProductId).ToList();

            var selectProduct = _repProduct.GetMany(x => !x.IsDeleted && products.Contains(x.Id));
            var selectProductId = selectProduct.Select(x => x.Id).ToList();

            insertProduct = selectProduct.Where(x => !x.IsDeleted && !existUserProductId.Contains(x.Id)).ToList();
            deleteUserProduct = existUserProduct.Where(x => !x.IsDeleted && !selectProductId.Contains(x.ProductId)).ToList();
            var numberDelete = deleteUserProduct.Count;
            for (var i = 0; i < numberDelete; i++)
            {
                deleteUserProduct[i].IsDeleted = true;
                deleteUserProduct[i].DeletedDate = DateTime.Now;
                _repUserProduct.Update(deleteUserProduct[i]);
                SaveChange();
            }

            var numberInsert = insertProduct.Count;
            for (var i = 0; i < numberInsert; i++)
            {
                var userRole = new T_UserProduct
                {
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    CreatedUser = 1,
                    UserId = userId,
                    ProductId = insertProduct[i].Id
                };
                var tryRestore = TryRestoreRolePermission(userRole);
                if (!tryRestore)
                {
                    _repUserProduct.Add(userRole);
                    SaveChange();
                }
            }
            result.IsSuccess = true;


            return result;
        }
        public bool TryRestoreRolePermission(T_UserProduct userProduct)
        {
            try
            {
                var exist = _repUserProduct.Get(x => x.UserId == userProduct.UserId && x.ProductId == userProduct.ProductId);
                if (exist != null)
                {
                    exist.IsDeleted = false;
                    _repUserProduct.Update(exist);
                    SaveChange();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

