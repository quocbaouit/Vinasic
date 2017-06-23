using System;
using System.Collections.Generic;
using System.Linq;
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
    public class BllOrder : IBllOrder
    {
        private readonly IT_OrderRepository _repOrder;
        private readonly IT_CustomerRepository _repCus;
        private readonly IT_UserRepository _repUser;
        private readonly IT_OrderDetailRepository _repOrderDetail;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllOrder(IUnitOfWork<VINASICEntities> unitOfWork, IT_OrderRepository repOrder, IT_OrderDetailRepository repOrderDetail, IT_CustomerRepository repCus, IT_UserRepository repUserRepository)
        {
            _unitOfWork = unitOfWork;
            _repOrder = repOrder;
            _repOrderDetail = repOrderDetail;
            _repUser = repUserRepository;
            _repCus = repCus;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }
        public PagedList<ModelOrder> GetList(int employee, int startIndexRecord, int pageSize, string sorting, string fromDate, string toDate, int employee1, string keyWord, int delivery, int paymentStatus)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }

            var realfromDate = DateTime.Parse(fromDate);
            var realtoDate = DateTime.Parse(toDate);
            var frDate = new DateTime(realfromDate.Year, realfromDate.Month, realfromDate.Day, 0, 0, 0, 0);
            var tDate = new DateTime(realtoDate.Year, realtoDate.Month, realtoDate.Day, 23, 59, 59, 999);
            var orders = _repOrder.GetMany(c => !c.IsDeleted).Select(c => new ModelOrder()
            {
                CustomerPhone = c.T_Customer.Mobile,
                CustomerEmail = c.T_Customer.Email,
                CustomerAddress = c.T_Customer.Address,
                CustomerTaxCode = c.T_Customer.TaxCode,
                CreatedForUser = c.CreatedForUser,
                Id = c.Id,
                Name = c.Name,
                Description = c.Description ?? "",
                CustomerId = c.CustomerId,
                DeliveryDate = c.DeliveryDate,
                SubTotal = c.SubTotal,
                IsPayment = c.IsPayment,
                IsApproval = c.IsApproval,
                HasTax = c.HasTax,
                strIspayment = c.IsPayment ? "Rồi" : "Chưa",
                strIsApproval = c.IsApproval ? "Đã Duyệt" : "Chưa Duyệt",
                strHasTax = c.HasTax == null ? "Có" : "Không",
                IsDelivery = c.IsDelivery,
                PaymentMethol = c.PaymentMethol,
                StrHasDelivery = c.IsDelivery == 0 ? "Chưa xác Định" : (c.IsDelivery == 1 ? "Chưa Giao" : "Đã Giao"),
                StrPaymentType = c.PaymentMethol == 0 ? "Chưa xác Định" : (c.PaymentMethol == 1 ? "Tiền Mặt" : "Chuyển Khoản"),
                CreatedUser = c.CreatedUser,
                CreateUserName = c.T_User.Name,
                CreatedDate = c.CreatedDate,
                HasPay = c.HasPay ?? 0,
                T_OrderDetail = c.T_OrderDetail
            }).OrderBy(sorting);


            if (employee1 != 0)
                orders = orders.Where(c => c.CreatedForUser == employee1);
            if (!string.IsNullOrEmpty(keyWord))
            {
                var intkeywork = 0;
                try
                {
                    intkeywork = int.Parse(keyWord);
                }
                catch (Exception)
                {
                    intkeywork = 0;
                }
                orders = orders.Where(c => c.Name.Trim().ToLower().Contains(keyWord.ToLower()) || c.CustomerPhone == keyWord || c.Id == intkeywork);
            }
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                orders = orders.Where(c => c.CreatedDate >= frDate && c.CreatedDate <= tDate);
            }
            if (paymentStatus == 1)
            {
                orders = orders.Where(c => c.HasPay == c.SubTotal);
            }
            if (paymentStatus == 2)
            {
                orders = orders.Where(c => c.HasPay < c.SubTotal);
            }
            if (delivery != 0)
            {
                orders = delivery == 2 ? orders.Where(c => c.IsDelivery == 2) : orders.Where(c => c.IsDelivery == 1 || c.IsDelivery == 0);
            }
            var pageNumber = (startIndexRecord / pageSize) + 1;

            var result = new PagedList<ModelOrder>(orders, pageNumber, pageSize);

            foreach (var order in result)
            {
                order.strHaspay = $"{order.HasPay ?? 0:0,0}";
                order.strSubTotal = $"{order.SubTotal:0,0}";
                order.StrCreatedDate = $"{order.CreatedDate:d/M/yyyy HH:mm}";
            }
            var sum = result.Sum(x => x.SubTotal);
            result.ToList().Add(new ModelOrder() { Name = "Tổng Cộng", SubTotal = sum });
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var partner = _repOrder.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (partner != null)
            {
                partner.IsDeleted = true;
                partner.DeletedUser = userId;
                partner.DeletedDate = DateTime.Now.AddHours(14);
                _repOrder.Update(partner);
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
        public List<ModelOrderDetail> GetListOrderDetailByOrderId(int orderId)
        {
            var allEmployee = _repUser.GetMany(x => !x.IsDeleted);

            var ordeDetails = _repOrderDetail.GetMany(o => !o.IsDeleted && o.OrderId == orderId).Select(o => new
                ModelOrderDetail()
            {
                Id = o.Id,
                OrderId = o.OrderId,
                CommodityId = o.CommodityId,
                CommodityName = o.T_Product.Name,
                FileName = o.FileName,
                PrintDescription = o.PrintDescription,
                DesignDescription = o.DesignDescription,
                Description = o.Description,
                Height = o.Height,
                Width = o.Width,
                Square = o.Square ?? 0,
                Quantity = o.Quantity,
                Price = o.Price,
                SumSquare = o.SumSquare,
                PrintStatus = o.PrintStatus,
                DesignStatus = o.DesignStatus,
                PrintUser = o.PrintUser,
                PrintFrom = o.PrintFrom,
                PrintTo = o.PrintTo,
                DesignUser = o.DesignUser,
                DesignFrom = o.DesignFrom,
                DesignTo = o.DesignTo,
                SubTotal = o.SubTotal,
                IsCompleted = o.IsCompleted,
                strIsComplete = o.IsCompleted ? "Đã Xong" : "Chưa Xong",
                strDesignStatus = o.DesignStatus == null ? "Chưa Làm" : (o.DesignStatus == 1 ? "Đang Làm" : (o.DesignStatus == 2 ? "Đã Xong" : "Chưa Làm")),
                strPrinStatus = o.PrintStatus == null ? "Chưa làm" : (o.PrintStatus == 1 ? "Đang Làm" : (o.PrintStatus == 2 ? "Đã Xong" : "Chưa Làm")),
                CreatedDate = o.CreatedDate,
            }).ToList();
            foreach (var order in ordeDetails)
            {
                var firstOrDefault = allEmployee.FirstOrDefault(x => x.Id == order.DesignUser);
                order.DesignUserName = firstOrDefault != null ? firstOrDefault.Name : "";
                var orDefault = allEmployee.FirstOrDefault(x => x.Id == order.PrintUser);
                order.PrintUserName = orDefault != null ? orDefault.Name : "";
                order.strSubTotal = $"{order.SubTotal:0,0}";
                order.strPrice = $"{order.Price:0,0}";
            }
            return ordeDetails;
        }
        public ResponseBase CreateOrder(ModelSaveOrder obj, int userId)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            if (obj != null)
            {
                int baseCustomerId;
                if (obj.CustomerId != 0)
                {
                    var cus = _repCus.Get(x => x.Id == obj.CustomerId);
                    cus.Name = obj.CustomerName;
                    cus.Address = obj.CustomerAddress;
                    cus.Mobile = obj.CustomerPhone;
                    cus.Email = obj.CustomerMail;
                    cus.TaxCode = obj.CustomerTaxCode;
                    cus.UpdatedDate = DateTime.Now.AddHours(14);
                    cus.UpdatedUser = userId;
                    baseCustomerId = obj.CustomerId;
                    _repCus.Update(cus);
                    SaveChange();
                }
                else
                {
                    var cus = new T_Customer
                    {
                        Name = obj.CustomerName,
                        Address = obj.CustomerAddress,
                        Mobile = obj.CustomerPhone,
                        Email = obj.CustomerMail,
                        TaxCode = obj.CustomerTaxCode,
                        CreatedDate = DateTime.Now.AddHours(14),
                        CreatedUser = userId
                    };
                    _repCus.Add(cus);
                    SaveChange();
                    baseCustomerId = cus.Id;
                }

                var order = new T_Order
                {
                    Name = obj.CustomerName,
                    Description = "",
                    CustomerId = baseCustomerId,
                    DeliveryDate = obj.DateDelivery,
                    SubTotal = obj.OrderTotal,
                    IsPayment = false,
                    IsApproval = false,
                    IsDeleted = false,
                    CreatedForUser = obj.EmployeeId,
                    CreatedUser = userId,
                    IsDelivery = 1,
                    CreatedDate = DateTime.Now.AddHours(14)
                };
                _repOrder.Add(order);
                SaveChange();
                foreach (var detail in obj.Detail)
                {
                    var orderDetail = new T_OrderDetail
                    {
                        Index = detail.Index,
                        OrderId = order.Id,
                        CommodityId = int.Parse(detail.CommodityId),
                        CommodityName = detail.CommodityName,
                        Height = detail.Height,
                        Width = detail.Width,
                        Square = detail.Square,
                        Quantity = detail.Quantity,
                        SumSquare = detail.SumSquare,
                        Price = detail.Price,
                        SubTotal = detail.Subtotal,
                        Description = detail.Description,
                        IsCompleted = false,
                        IsDeleted = false,
                        CreatedUser = userId,
                        CreatedDate = DateTime.Now.AddHours(14),
                        FileName = detail.FileName
                    };
                    _repOrderDetail.Add(orderDetail);
                    SaveChange();
                }

                result.IsSuccess = true;

            }
            else
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create ProductType", Message = "Đối Tượng Không tồn tại" });
            }
            return result;
        }
        public ResponseBase UpdatedOrder(ModelSaveOrder obj, int userId)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            if (obj != null)
            {
                var cus = _repCus.Get(x => x.Id == obj.CustomerId);
                cus.Name = obj.CustomerName;
                cus.Address = obj.CustomerAddress;
                cus.Mobile = obj.CustomerPhone;
                cus.Email = obj.CustomerMail;
                cus.TaxCode = obj.CustomerTaxCode;
                cus.UpdatedDate = DateTime.Now.AddHours(14);
                cus.UpdatedUser = userId;
                _repCus.Update(cus);
                SaveChange();


                var order = _repOrder.Get(x => x.Id == obj.OrderId);
                order.Name = obj.CustomerName;
                order.Description = "";
                order.SubTotal = obj.OrderTotal;
                order.CustomerId = obj.CustomerId;
                order.DeliveryDate = obj.DateDelivery;
                order.IsPayment = false;
                order.IsApproval = false;
                order.IsDeleted = false;
                order.IsDelivery = 1;
                order.CreatedForUser = obj.EmployeeId;
                order.UpdatedUser = userId;
                order.UpatedDate = DateTime.Now.AddHours(14);
                _repOrder.Update(order);
                SaveChange();
                var baseOrderDetail = _repOrderDetail.GetMany(x => x.OrderId == order.Id).ToList();
                foreach (var detail in baseOrderDetail)
                {
                    _repOrderDetail.Delete(detail);
                    SaveChange();
                }
                foreach (var detail in obj.Detail)
                {
                    var orderDetail = new T_OrderDetail
                    {
                        OrderId = order.Id,
                        Index = detail.Index,
                        CommodityId = int.Parse(detail.CommodityId),
                        CommodityName = detail.CommodityName,
                        Height = detail.Height,
                        Width = detail.Width,
                        Square = detail.Square,
                        SumSquare = detail.SumSquare,
                        Quantity = detail.Quantity,
                        Price = detail.Price,
                        SubTotal = detail.Subtotal,
                        Description = detail.Description,
                        IsCompleted = false,
                        IsDeleted = false,
                        CreatedUser = userId,
                        CreatedDate = order.CreatedDate,
                        FileName = detail.FileName
                    };
                    _repOrderDetail.Add(orderDetail);
                    SaveChange();
                }
                result.IsSuccess = true;

            }
            else
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create ProductType", Message = "Đối Tượng Không tồn tại" });
            }
            return result;
        }
        public double PrintfPecent(int orderId)
        {
            const double pecent = 100;
            var listOrderDetail = _repOrderDetail.GetMany(o => !o.IsDeleted && o.OrderId == orderId && o.T_Product.Id == 1).ToList();
            if (listOrderDetail.Count > 0)
            {
                var listComplete = _repOrderDetail.GetMany(o => !o.IsDeleted && o.OrderId == orderId && o.T_Product.Id == 1 && o.IsCompleted).ToList();
                if (listComplete.Count > 0)
                {
                    ////double totalm2 = (from orderDetail in listOrderDetail where orderDetail.Height != 0 && orderDetail.Width != 0 && orderDetail.Quantity != 0 select orderDetail.Height ?? 0 * orderDetail.Width ?? 0 * orderDetail.Quantity ?? 0).Aggregate<double, double>(0, (current, total) => current + total);
                    //double totalm2Complete = (from complete in listComplete where complete.Height != 0 && complete.Width != 0 && complete.Quantity != 0 select complete.Height ?? 0 * complete.Width ?? 0 * complete.Quantity ?? 0).Aggregate<double, double>(0, (current, total) => current + total);
                    //pecent = totalm2Complete / totalm2 * 100;
                }
            }
            return pecent;
        }
        public ResponseBase UpdateApproval(int orderId, bool isApproval, int userId)
        {
            var responResult = new ResponseBase();
            var order = _repOrder.GetMany(c => !c.IsDeleted && c.Id == orderId).FirstOrDefault();
            if (!isApproval)
            {
                if (order != null)
                {
                    order.IsApproval = true;
                    order.UpdatedUser = userId;
                    order.UpatedDate = DateTime.Now.AddHours(14);
                    _repOrder.Update(order);
                    SaveChange();
                    responResult.IsSuccess = true;
                }
                else
                {
                    responResult.IsSuccess = false;
                    responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Lỗi" });
                }
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Đã duyệt rồi không thể bỏ duyệt" });
            }

            return responResult;
        }
        public ResponseBase UpdateDelivery(int orderId, int status, int userId)
        {
            var responResult = new ResponseBase();
            var order = _repOrder.GetMany(c => !c.IsDeleted && c.Id == orderId).FirstOrDefault();
            if (order != null)
            {
                status = status + 1;
                if (status == 3)
                {
                    status = 1;
                }
                order.IsDelivery = status;
                order.UpdatedUser = userId;
                order.UpatedDate = DateTime.Now.AddHours(14);
                _repOrder.Update(order);
                SaveChange();
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Lỗi" });
            }
            return responResult;
        }
        public ResponseBase UpdatePayment(int orderId, float payment, int paymentType, int userId)
        {
            var responResult = new ResponseBase();
            var order = _repOrder.GetMany(c => !c.IsDeleted && c.Id == orderId).FirstOrDefault();
            if (order != null)
            {
                if (order.HasPay == null)
                {
                    order.HasPay = 0;
                }
                var total = order.HasPay + payment;
                if (total > order.SubTotal)
                {
                    order.HasPay = order.SubTotal;
                }
                order.HasPay = total;
                order.UpdatedUser = userId;
                order.PaymentMethol = paymentType;
                order.UpatedDate = DateTime.Now.AddHours(14);
                _repOrder.Update(order);
                SaveChange();
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Lỗi" });
            }
            return responResult;
        }
        public ResponseBase UpdateHasTax(int orderId, int id, int userId)
        {
            var responResult = new ResponseBase();
            var order = _repOrder.GetMany(c => !c.IsDeleted && c.Id == orderId).FirstOrDefault();

            if (order != null)
            {
                var subTotal = order.SubTotal;
                if (id == 1)
                {
                    order.HasTax = false;
                    subTotal = subTotal - (subTotal * 10 / 110);
                }
                else
                {
                    order.HasTax = true;
                    subTotal = (subTotal * 10 / 100) + subTotal;

                }
                order.SubTotal = subTotal;
                order.UpdatedUser = userId;
                order.UpatedDate = DateTime.Now.AddHours(14);
                _repOrder.Update(order);
                SaveChange();
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Lỗi" });
            }
            return responResult;
        }
        public ResponseBase UpdateDesignUser(int detailId, int designId, string description)
        {
            var responResult = new ResponseBase();
            var order = _repOrderDetail.GetMany(c => !c.IsDeleted && c.Id == detailId).FirstOrDefault();
            if (order != null)
            {
                order.DesignDescription = description;
                order.DesignUser = designId;
                order.UpatedDate = DateTime.Now.AddHours(14);
                _repOrderDetail.Update(order);
                SaveChange();
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Lỗi" });
            }
            return responResult;
        }
        public ResponseBase UpdatePrintUser(int detailId, int printId, string description)
        {
            var responResult = new ResponseBase();
            var order = _repOrderDetail.GetMany(c => !c.IsDeleted && c.Id == detailId).FirstOrDefault();
            if (order != null)
            {
                order.PrintDescription = description;
                order.PrintUser = printId;
                order.UpatedDate = DateTime.Now.AddHours(14);
                _repOrderDetail.Update(order);
                SaveChange();
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Lỗi" });
            }
            return responResult;
        }
        public ResponseBase UpdateHaspay(int orderId, string haspay)
        {
            var pay = string.IsNullOrEmpty(haspay) ? 0 : double.Parse(haspay);
            var responResult = new ResponseBase();
            var order = _repOrder.Get(c => !c.IsDeleted && c.Id == orderId);
            if (order != null)
            {
                order.HasPay = pay;
                order.UpatedDate = DateTime.Now.AddHours(14);
                _repOrder.Update(order);
                SaveChange();
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Lỗi" });
            }
            return responResult;
        }
        public ResponseBase UpdateHaspayCustom(int orderId, string haspay, int paymentType)
        {
            var responResult = new ResponseBase();
            var order = _repOrder.GetMany(c => !c.IsDeleted && c.Id == orderId).FirstOrDefault();
            if (order != null)
            {
                double pay = 0;
                if (haspay != null)
                {
                    pay = Double.Parse(haspay);
                }
                if (pay > order.SubTotal)
                {
                    pay = order.SubTotal;
                }
                order.HasPay = pay;
                order.PaymentMethol = paymentType;
                order.UpatedDate = DateTime.Now.AddHours(14);
                _repOrder.Update(order);
                SaveChange();
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Lỗi" });
            }
            return responResult;
        }

        public List<int> GetOrderIdByPaymentStatusAndDelivery(DateTime fromDate, DateTime toDate, int delivery, int paymentStatus)
        {
            var order = _repOrder.GetMany(c => !c.IsDeleted && c.CreatedDate >= fromDate && c.CreatedDate <= toDate);
            if (paymentStatus == 1)
            {
                order = order.Where(c => c.HasPay == c.SubTotal);
            }
            if (paymentStatus == 2)
            {
                order = order.Where(c => c.HasPay < c.SubTotal);
            }
            if (delivery != 0)
            {
                order = delivery != 2 ? order.Where(c => c.IsDelivery != 2) : order.Where(c => c.IsDelivery == 2);
            }
            var listId = order.Select(x => x.Id).ToList();
            return listId;
        }
        public List<ModelViewDetail> ExportReport(DateTime fromDate, DateTime toDate, int employee, string keyWord, int delivery, int paymentStatus)
        {
            var frDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0, 0);
            var tDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59, 999);
            var orders =
                _repOrderDetail.GetMany(c => !c.IsDeleted && !c.T_Order.IsDeleted && c.T_Order.CreatedDate >= frDate && c.T_Order.CreatedDate <= tDate)
                    .Select(c => new ModelViewDetail()
                    {
                        CustomerName = c.T_Order.Name,
                        FileName = c.FileName,
                        CreatedDate = c.CreatedDate,
                        Id = c.Id,
                        CreatedForUser = c.T_Order.CreatedForUser,
                        OrderId = c.OrderId,
                        CommodityId = c.CommodityId,
                        CommodityName = c.CommodityName,
                        CustomerPhone = c.T_Order.T_Customer.Mobile,
                        PrintDescription = c.PrintDescription,
                        DesignDescription = c.DesignDescription,
                        Description = c.Description,
                        Height = c.Height,
                        Width = c.Width,
                        SumSquare = c.SumSquare,
                        Square = c.Square ?? 0,
                        Quantity = c.Quantity,
                        Price = c.Price,
                        Unit = c.T_Product.Description,
                        PrintStatus = c.PrintStatus,
                        DesignStatus = c.DesignStatus,
                        PrintUser = c.PrintUser,
                        PrintFrom = c.PrintFrom,
                        PrintTo = c.PrintTo,
                        DesignUser = c.DesignUser,
                        DesignFrom = c.DesignFrom,
                        DesignTo = c.DesignTo,
                        SubTotal = c.SubTotal,
                        Total1 = c.T_Order.SubTotal,
                        HasPay = c.T_Order.HasPay ?? 0,
                        HasExist = c.T_Order.SubTotal - c.T_Order.HasPay?? c.T_Order.SubTotal,
                        IsCompleted = c.IsCompleted,
                        strIsComplete = c.IsCompleted ? "Đã Xong" : "Chưa Xong",
                        strDesignStatus = c.DesignStatus == null ? "Chưa Làm" : (c.DesignStatus == 1 ? "Đang Làm" : (c.DesignStatus == 2 ? "Đã Xong" : "Chưa Làm")),
                        strPrinStatus = c.PrintStatus == null ? "Chưa Làm" : (c.PrintStatus == 1 ? "Đang Làm" : (c.PrintStatus == 2 ? "Đã Xong" : "Chưa Làm"))
                    }).ToList();
            if (employee != 0)
            {
                orders = orders.Where(c => c.CreatedForUser == employee).ToList();
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                orders = orders.Where(c => c.CustomerName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) || c.CustomerPhone.Contains(keyWord) || c.OrderId.ToString().Contains(keyWord)).ToList();
            }
            if (paymentStatus == 1)
            {
                orders = orders.Where(c => c.HasPay == c.Total1).ToList();
            }
            if (paymentStatus == 2)
            {
                orders = orders.Where(c => c.HasPay < c.Total1).ToList();
            }
            var sum = orders.Sum(x => x.SubTotal);
            if (orders.Count > 0)
            {
                orders[0].Total = sum;
            }
            return orders;

        }
        public List<ModelViewDetail> GetOrderComplex(int orderId)
        {
            var allEmployee = _repUser.GetMany(x => !x.IsDeleted);
            var orders =
                        _repOrderDetail.GetMany(c => !c.IsDeleted && c.T_Order.Id == orderId)
                            .Select(c => new ModelViewDetail()
                            {
                                CustomerName = c.T_Order.Name,
                                FileName = c.FileName,
                                CreatedDate = c.CreatedDate,
                                Id = c.Id,
                                CreatedForUser = c.T_Order.CreatedForUser,
                                OrderId = c.OrderId,
                                CommodityId = c.CommodityId,
                                CommodityName = c.CommodityName,
                                CustomerPhone = c.T_Order.T_Customer.Mobile,
                                CustomerAddress = c.T_Order.T_Customer.Address,
                                PrintDescription = c.PrintDescription,
                                DesignDescription = c.DesignDescription,
                                Description = c.Description,
                                Height = c.Height,
                                Width = c.Width,
                                Square = c.Square ?? 0,
                                SumSquare = c.SumSquare ?? 0,
                                Quantity = c.Quantity,
                                Price = c.Price,
                                PrintStatus = c.PrintStatus,
                                DesignStatus = c.DesignStatus,
                                PrintUser = c.PrintUser,
                                PrintFrom = c.PrintFrom,
                                Unit = c.T_Product.Description,
                                PrintTo = c.PrintTo,
                                Total = c.T_Order.SubTotal,
                                DesignUser = c.DesignUser,
                                DesignFrom = c.DesignFrom,
                                DesignTo = c.DesignTo,
                                SubTotal = c.SubTotal,
                                IsCompleted = c.IsCompleted,
                                strIsComplete = c.IsCompleted ? "Đã Xong" : "Chưa Làm",
                                strDesignStatus = c.DesignStatus == null ? "Chưa Làm" : (c.DesignStatus == 1 ? "Đang Làm" : (c.DesignStatus == 2 ? "Đã Xong" : "Chưa Làm")),
                                strPrinStatus = c.PrintStatus == null ? "Chưa Làm" : (c.PrintStatus == 1 ? "Đang Làm" : (c.PrintStatus == 2 ? "Đã Xong" : "Chưa Làm"))
                            }).ToList();
            foreach (var order in orders)
            {
                var firstOrDefault = allEmployee.FirstOrDefault(x => x.Id == order.DesignUser);
                order.DesignUserName = firstOrDefault != null ? firstOrDefault.Name : "";
                var orDefault = allEmployee.FirstOrDefault(x => x.Id == order.PrintUser);
                order.PrintUserName = orDefault != null ? orDefault.Name : "";

                var orDefault1 = allEmployee.FirstOrDefault(x => x.Id == order.CreatedForUser);
                order.CreateForUserName = orDefault1 != null ? orDefault1.LastName : "";
                order.CreateForUserMobile = orDefault1 != null ? orDefault1.Mobile : "";
                order.strSubTotal = $"{order.SubTotal:0,0}";
                order.strPrice = $"{order.Price:0,0}";
                order.strTotal = $"{order.Total:0,0}";
            }
            return orders;
        }

        public List<ModelOrder> GetOrderOfEmployeeByDate()
        {
            return null;
        }

        public double GetPriceForCustomerAndProduct(int customerId, int productId)
        {
            var detail = _repOrderDetail.GetMany(x => x.T_Order.CustomerId == customerId && x.CommodityId == productId).OrderByDescending(z => z.CreatedDate).FirstOrDefault();
            if (detail == null)
                return 0;            
            return detail.Price??0;
        }
        public PagedList<ModelViewDetail> GetListViewDetail(string keyWord, int startIndexRecord, int pageSize, string sorting, string fromDate, string toDate, int employee)
        {
            var allEmployee = _repUser.GetMany(x => !x.IsDeleted);
            var realfromDate = DateTime.Parse(fromDate);
            var realtoDate = DateTime.Parse(toDate);
            var frDate = new DateTime(realfromDate.Year, realfromDate.Month, realfromDate.Day, 0, 0, 0, 0);
            var tDate = new DateTime(realtoDate.Year, realtoDate.Month, realtoDate.Day, 23, 59, 59, 999);
            sorting = "CreatedDate DESC";
            var orders =
                _repOrderDetail.GetMany(c => !c.IsDeleted && !c.T_Order.IsDeleted)
                    .Select(c => new ModelViewDetail()
                    {
                        CustomerName = c.T_Order.Name,
                        FileName = c.FileName,
                        CreatedDate = c.CreatedDate,
                        Id = c.Id,
                        T_Order = c.T_Order,
                        CreatedForUser = c.T_Order.CreatedForUser,
                        OrderId = c.OrderId,
                        CommodityId = c.CommodityId,
                        CommodityName = c.CommodityName,
                        CustomerPhone = c.T_Order.T_Customer.Mobile,
                        PrintDescription = c.PrintDescription,
                        DesignDescription = c.DesignDescription,
                        Description = c.Description,
                        Height = c.Height,
                        Width = c.Width,
                        SumSquare = c.SumSquare,
                        Square = c.Square ?? 0,
                        Quantity = c.Quantity,
                        Price = c.Price,
                        PrintStatus = c.PrintStatus,
                        DesignStatus = c.DesignStatus,
                        PrintUser = c.PrintUser,
                        PrintFrom = c.PrintFrom,
                        PrintTo = c.PrintTo,
                        DesignUser = c.DesignUser,
                        DesignFrom = c.DesignFrom,
                        DesignTo = c.DesignTo,
                        SubTotal = c.SubTotal,
                        IsCompleted = c.IsCompleted,
                        strIsComplete = c.IsCompleted ? "Đã In" : "Chưa In",
                        strDesignStatus = c.DesignStatus == null ? "Không Xử Lý" : (c.DesignStatus == 1 ? "Đang Xử Lý" : (c.DesignStatus == 2 ? "Đã Xử Lý" : "Không xử lý")),
                        strPrinStatus = c.PrintStatus == null ? "Không Xử Lý" : (c.PrintStatus == 1 ? "Đang Xử Lý" : (c.PrintStatus == 2 ? "Đã Xử Lý" : "Không xử lý"))
                    }).OrderBy(sorting);
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                orders = orders.Where(c => c.T_Order.CreatedDate >= frDate && c.T_Order.CreatedDate <= tDate);
            }
            if (employee != 0)
            {
                orders = orders.Where(c => c.CreatedForUser == employee);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                orders = orders.Where(c => c.CustomerName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) || c.CustomerPhone.Contains(keyWord));
            }
            var pageNumber = (startIndexRecord / pageSize) + 1;
            var result = new PagedList<ModelViewDetail>(orders, pageNumber, pageSize);
            foreach (var order in result)
            {
                var firstOrDefault = allEmployee.FirstOrDefault(x => x.Id == order.DesignUser);
                order.DesignUserName = firstOrDefault != null ? firstOrDefault.Name : "";
                var orDefault = allEmployee.FirstOrDefault(x => x.Id == order.PrintUser);
                order.PrintUserName = orDefault != null ? orDefault.Name : "";
                order.strSubTotal = $"{order.SubTotal:0,0}";
                order.strPrice = $"{order.Price:0,0}";
            }
            return result;

        }
    }
}

