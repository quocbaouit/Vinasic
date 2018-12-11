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
using System.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace VINASIC.Business
{
    public class BllOrder : IBllOrder
    {
        private readonly IT_OrderRepository _repOrder;
        private readonly IT_CustomerRepository _repCus;
        private readonly IT_SiteSettingRepository _repSite;
        private readonly IT_UserRepository _repUser;
        private readonly IT_OrderDetailRepository _repOrderDetail;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        private readonly TimeZoneInfo curentZone = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["WEBSITE_TIME_ZONE"]);
        public BllOrder(IUnitOfWork<VINASICEntities> unitOfWork, IT_OrderRepository repOrder, IT_OrderDetailRepository repOrderDetail, IT_CustomerRepository repCus, IT_UserRepository repUserRepository, IT_SiteSettingRepository repSite)
        {
            _unitOfWork = unitOfWork;
            _repOrder = repOrder;
            _repOrderDetail = repOrderDetail;
            _repUser = repUserRepository;
            _repSite = repSite;
            _repCus = repCus;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }
        public PagedList<ModelOrder> GetList(int employee, int startIndexRecord, int pageSize, string sorting, string fromDate, string toDate, int employee1, string keyWord, float orderStatus)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var realfromDate = DateTime.Parse(fromDate);
            var realtoDate = DateTime.Parse(toDate);

            var frDate = new DateTime(realfromDate.Year, realfromDate.Month, realfromDate.Day, 0, 0, 0, 0);
            frDate = TimeZoneInfo.ConvertTimeToUtc(frDate, curentZone);
            var tDate = new DateTime(realtoDate.Year, realtoDate.Month, realtoDate.Day, 23, 59, 59, 999);
            tDate = TimeZoneInfo.ConvertTimeToUtc(tDate, curentZone);
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
                HaspayTransfer = c.HaspayTransfer ?? 0,
                OrderStatus = c.OrderStatus,
                T_OrderDetail = c.T_OrderDetail,
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
            if (orderStatus != -1)
            {
                orders = orders.Where(c => c.OrderStatus == orderStatus);
            }
            var pageNumber = (startIndexRecord / pageSize) + 1;

            var result = new PagedList<ModelOrder>(orders, pageNumber, pageSize);
            foreach (var order in result)
            {
                order.strHaspay = $"{order.HasPay ?? 0:0,0}";
                order.strHaspayTransfer = $"{order.HaspayTransfer ?? 0:0,0}";
                order.strSubTotal = $"{order.SubTotal:0,0}";
                order.StrCreatedDate = $"{ TimeZoneInfo.ConvertTimeFromUtc(order.CreatedDate, curentZone):d/M/yyyy HH:mm}";
                order.strFileName  = string.Join(", ", order.T_OrderDetail.Select(x => x.FileName).ToArray());
            }
            var sum = result.Sum(x => x.SubTotal);
            result.ToList().Add(new ModelOrder() { Name = "Tổng Cộng", SubTotal = sum });
            return result;
        }
        public ResponseBase DeleteById(int id, int userId, bool isAdmin)
        {
            var responResult = new ResponseBase();
            var partner = _repOrder.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (partner.OrderStatus >= 2 && !isAdmin)
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Delete", Message = "Đơn hàng này không thể xóa. Vui lòng liên hệ với quản trị" });
            }
            else if (partner != null)
            {
                partner.IsDeleted = true;
                partner.DeletedUser = userId;
                partner.DeletedDate = DateTime.UtcNow;
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
                CreatedDate = o.CreatedDate,
                DesignView = o.DesignView ?? "",
                PrintView = o.PrintView ?? "",
                AddOnView = o.AddOnView ?? "",
                DetailStatus = o.DetailStatus,

            }).ToList();
            foreach (var order in ordeDetails)
            {
                if (order.DetailStatus == 1 || order.DetailStatus == 2 || order.DetailStatus == 3)
                {
                    if (order.DesignView != null)
                    {
                        order.UserProcess = order.DesignView;
                    }
                }
                if (order.DetailStatus == 3 || order.DetailStatus == 4 || order.DetailStatus == 5)
                {
                    if (order.PrintView != null)
                    {
                        order.UserProcess = order.PrintView;
                    }
                }
                if (order.DetailStatus == 5 || order.DetailStatus == 6 || order.DetailStatus == 7)
                {
                    if (order.AddOnView != null)
                    {
                        order.UserProcess = order.AddOnView;
                    }
                }
                if (string.IsNullOrEmpty(order.UserProcess))
                {
                    order.UserProcess = "Chưa có nhân viên phụ trách";
                }
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
                    cus.UpdatedDate = DateTime.UtcNow;
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
                        CreatedDate = DateTime.UtcNow,
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
                    SubTotalExcludeTax=obj.OrderTotalExcludeTax,
                    HasTax=obj.Tax,
                    CreatedUser = userId,
                    IsDelivery = 1,
                    OrderStatus = 1,
                    CreatedDate = DateTime.UtcNow
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
                        DetailStatus = 0,
                        CreatedUser = userId,
                        CreatedDate = DateTime.UtcNow,
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
        public ResponseBase UpdatedOrder(ModelSaveOrder obj, int userId, bool isAdmin)
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
                cus.UpdatedDate = DateTime.UtcNow;
                cus.UpdatedUser = userId;
                _repCus.Update(cus);
                SaveChange();


                var order = _repOrder.Get(x => x.Id == obj.OrderId);
                if (order.OrderStatus >= 2 && !isAdmin)
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Delete", Message = "Đơn hàng này không thể cập nhật. Vui lòng liên hệ với quản trị" });
                    return result;
                }
                order.Name = obj.CustomerName;
                order.HasTax = obj.Tax;
                order.SubTotalExcludeTax = obj.OrderTotalExcludeTax;
                order.Description = "";
                order.SubTotal = obj.OrderTotal;
                order.CustomerId = obj.CustomerId;
                order.DeliveryDate = obj.DateDelivery;
                order.IsPayment = false;
                order.IsApproval = false;
                order.IsDeleted = false;
                order.IsDelivery = 1;
                order.OrderStatus = 1;
                order.CreatedForUser = obj.EmployeeId;
                order.UpdatedUser = userId;
                order.UpatedDate = DateTime.UtcNow;
                _repOrder.Update(order);
                SaveChange();
                var baseOrderDetail = _repOrderDetail.GetMany(x => x.OrderId == order.Id).ToList();
                var deleteDetail = baseOrderDetail.Where(x =>!obj.Detail.Select(y => y.Id).Contains(x.Id)).ToList();
                foreach (var detail in deleteDetail)
                {
                    _repOrderDetail.Delete(detail);
                    SaveChange();
                }
                foreach (var detail in obj.Detail)
                {
                    var detailUpdate = baseOrderDetail.Where(x => x.Id == detail.Id).FirstOrDefault();
                    if (detailUpdate != null && detail.Id != 0 && detailUpdate.CommodityId == int.Parse(detail.CommodityId))
                    {        
                            detailUpdate.OrderId = order.Id;
                            detailUpdate.Index = detail.Index;
                            detailUpdate.CommodityId = int.Parse(detail.CommodityId);
                            detailUpdate.CommodityName = detail.CommodityName;
                            detailUpdate.Height = detail.Height;
                            detailUpdate.Width = detail.Width;
                            detailUpdate.Square = detail.Square;
                            detailUpdate.SumSquare = detail.SumSquare;
                            detailUpdate.Quantity = detail.Quantity;
                            detailUpdate.Price = detail.Price;
                            detailUpdate.SubTotal = detail.Subtotal;
                            detailUpdate.Description = detail.Description;
                            detailUpdate.IsCompleted = false;
                            detailUpdate.IsDeleted = false;
                            detailUpdate.CreatedUser = userId;
                            detailUpdate.CreatedDate = order.CreatedDate;
                            detailUpdate.FileName = detail.FileName;
                            _repOrderDetail.Update(detailUpdate);
                            SaveChange();
                    }
                    else
                    {
                        if (detailUpdate != null)
                        {
                            _repOrderDetail.Delete(detailUpdate);
                            SaveChange();
                        }                       
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
                            DetailStatus = 0,
                            CreatedDate = order.CreatedDate,
                            FileName = detail.FileName
                        };
                        _repOrderDetail.Add(orderDetail);
                        SaveChange();
                    }                  
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
                    order.UpatedDate = DateTime.UtcNow;
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
        public ResponseBase UpdateOrderStatus(int orderId, float status, int userId, bool isAdmin)
        {
            var responResult = new ResponseBase();
            var customerId = 0;
            var order = _repOrder.GetMany(c => !c.IsDeleted && c.Id == orderId).FirstOrDefault();
            //if (order.OrderStatus >= 3 && !isAdmin && status < order.OrderStatus)
            //{
            //    responResult.IsSuccess = false;
            //    responResult.Errors.Add(new Error() { MemberName = "Tài khoản không có quyền thay đổi đơn hàng sau khi giao hàng", Message = "Lỗi" });
            //    return responResult;

            //}
            if (order != null)
            {
                order.OrderStatus = status;
                order.UpdatedUser = userId;
                order.UpatedDate = DateTime.UtcNow;
                customerId = order.CustomerId;
                _repOrder.Update(order);
                SaveChange();
                responResult.IsSuccess = true;
                if (status == 2)
                {
                    var customer = _repCus.GetById(customerId);
                    if (customer != null)
                    {
                        var phone = customer.Mobile;
                        string mess = _repSite.GetById(1).Value;
                        if (!String.IsNullOrEmpty(phone))
                        {
                            Task.Run(() => Send(phone,mess));
                        }
                        
                    }
                    
                }
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Lỗi" });
            }
            return responResult;
        }
        public void Send(string phone,string mess)
        {
            //SendMail
            SendMailSMTP sendMail = new SendMailSMTP();
            sendMail.SendMail("quocbao.uit@gmail.com", "quocbao.uit@gmail.com","test","this is test mail");
            //SendSMS
            SpeedSMSAPI api = new SpeedSMSAPI("zg0WCSR_yUIjz3z7iWARLvEp3IEXhnKg");
            String[] phones = new String[] { phone };
            //String str = ConfigurationManager.AppSettings["SMS_CONTENT"];
            String response = api.sendSMS(phones, mess, 2, "");
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
                order.UpatedDate = DateTime.UtcNow;
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
        public ResponseBase UpdatePayment(int orderId, float payment, int paymentType, int userId, string transferDescription)
        {
            var responResult = new ResponseBase();
            var order = _repOrder.GetMany(c => !c.IsDeleted && c.Id == orderId).FirstOrDefault();
            if (order != null)
            {
                if (order.HasPay == null)
                {
                    order.HasPay = 0;
                }
                double? total = 0;
                if (paymentType == 1)
                {
                    total = order.HasPay + payment;
                    if (total > order.SubTotal)
                    {
                        order.HasPay = order.SubTotal;
                    }
                   
                    order.HasPay = total;
                    if (payment == 0)
                    {
                        order.HasPay = 0;
                    }
                }
                if (paymentType == 2)
                {
                    if (order.HaspayTransfer == null)
                    {
                        order.HaspayTransfer = 0;
                    }
                    total = order.HaspayTransfer + payment;
                    if (total > order.SubTotal)
                    {
                        order.HaspayTransfer = order.SubTotal;
                    }
                    
                    order.HaspayTransfer = total;
                    if (payment == 0)
                    {
                        order.HaspayTransfer = 0;
                    }
                    order.Description = transferDescription;
                }
                order.UpdatedUser = userId;
                order.PaymentMethol = paymentType;
                order.UpatedDate = DateTime.UtcNow;
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
                order.UpatedDate = DateTime.UtcNow;
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
                order.UpatedDate = DateTime.UtcNow;
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
        
           public ResponseBase GetJobDescriptionForEmployee(int detailId, int status, int employeeId, string content)
        {
            var responResult = new ResponseBase();
            var orderDetail = _repOrderDetail.GetMany(c => !c.IsDeleted && c.Id == detailId).FirstOrDefault();

            if (orderDetail != null)
            {
                orderDetail.DetailStatus = status;
                if (employeeId == 0)
                {
                    orderDetail.PrintUser = null;
                    orderDetail.AddonUser = null;
                }
                else
                {
                    var employe = _repUser.GetById(employeeId);
                    if (status == 1)
                    {
                        responResult.Data = orderDetail.DesignDescription;
                    }
                    if (status == 3)
                    {
                        responResult.Data = orderDetail.PrintDescription;
                    }
                    if (status == 5)
                    {
                            responResult.Data = orderDetail.AddOnView;
                    }
                }
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Lỗi" });
            }
            return responResult;
        }
        public ResponseBase UpdateDetailStatus(int detailId, int status, int employeeId,string content)
        {
            var responResult = new ResponseBase();
            var orderDetail = _repOrderDetail.GetMany(c => !c.IsDeleted && c.Id == detailId).FirstOrDefault();
            if (orderDetail != null)
            {
                orderDetail.DetailStatus = status;
                if (employeeId == 0)
                {
                    orderDetail.PrintUser = null;
                    orderDetail.AddonUser = null;
                }
                else
                {
                    var employe = _repUser.GetById(employeeId);
                    if (status == 1)
                    {
                        orderDetail.DesignUser = employe.Id;
                        orderDetail.DesignView = employe.FisrtName;
                        orderDetail.DesignDescription = content;
                    }
                    if (status == 3)
                    {
                        orderDetail.PrintUser = employe.Id;
                        orderDetail.PrintView = employe.FisrtName;
                        orderDetail.PrintDescription = content;
                    }
                    if (status == 5)
                    {
                        orderDetail.AddonUser = employe.Id;
                        orderDetail.AddOnView = employe.FisrtName;
                    }
                }
                orderDetail.UpatedDate = DateTime.UtcNow;
                _repOrderDetail.Update(orderDetail);
                SaveChange();
                var order = _repOrder.GetById(orderDetail.OrderId);
                string notificationContent ="Khách Hàng:"+ order.Name + ",Dịch Vụ:" + orderDetail.CommodityName+",Số Lượng: "+ orderDetail.Quantity;
                var isComplete = _repOrderDetail.GetMany(x => x.OrderId == order.Id && x.DetailStatus != 0 && x.DetailStatus != 7).ToList();
                if (isComplete.Count == 0)
                {
                    order.OrderStatus = 2;
                }
                else
                {
                    order.OrderStatus = 1;
                }
                _repOrder.Update(order);
                SaveChange();
                List<Notification> listSubcription = new List<Notification>();
                var userGetPush = _repUser.GetById(employeeId);
                if (!string.IsNullOrEmpty(userGetPush.Subscription))
                {
                    listSubcription = JsonConvert.DeserializeObject<List<Notification>>(userGetPush.Subscription);
                    var endPoint = listSubcription.Select(x => new Dynamic.Framework.Subscription()
                    {
                        Id = x.Guid,
                        BrowserName = x.BrowserName,
                        BrowserVersion = x.BrowserVersion,
                        endpoint = x.Endpoint,
                        OsName = x.OsName,
                        OsVersion = x.OsVersion,
                        keys = x.Keys,
                    }).ToList();
                    PushNotificationHelper.SendNotification(endPoint, "Thông Báo Cho: "+ userGetPush.FisrtName, notificationContent, "/", 2);
                }
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Update", Message = "Lỗi" });
            }
            return responResult;
        }
        public ResponseBase UpdateDetailStatus2(int detailId, int status, int employeeId)
        {
            var responResult = new ResponseBase();
            var orderDetail = _repOrderDetail.GetMany(c => !c.IsDeleted && c.Id == detailId).FirstOrDefault();
            if (orderDetail != null)
            {
                orderDetail.DetailStatus = status;
                if (employeeId == 0)
                {
                    orderDetail.PrintUser = null;
                    orderDetail.AddonUser = null;
                }
                else
                {
                    var employe = _repUser.GetById(employeeId);
                    if (status == 1)
                    {
                        orderDetail.DesignUser = employe.Id;
                        orderDetail.DesignView = employe.FisrtName;
                    }
                    if (status == 3)
                    {
                        orderDetail.PrintUser = employe.Id;
                        orderDetail.PrintView = employe.FisrtName;
                    }
                    if (status == 5)
                    {
                        orderDetail.AddonUser = employe.Id;
                        orderDetail.AddOnView = employe.FisrtName;
                    }
                }
                orderDetail.UpatedDate = DateTime.UtcNow;
                _repOrderDetail.Update(orderDetail);
                SaveChange();
                var order = _repOrder.GetById(orderDetail.OrderId);
                var isComplete = _repOrderDetail.GetMany(x => x.OrderId == order.Id && x.DetailStatus != 0 && x.DetailStatus != 7).ToList();
                if (isComplete.Count == 0)
                {
                    order.OrderStatus = 2;
                }
                else
                {
                    order.OrderStatus = 1;
                }
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
        public ResponseBase UpdatePrintUser(int detailId, int printId, string description)
        {
            var responResult = new ResponseBase();
            var order = _repOrderDetail.GetMany(c => !c.IsDeleted && c.Id == detailId).FirstOrDefault();
            if (order != null)
            {
                order.PrintDescription = description;
                order.PrintUser = printId;
                order.UpatedDate = DateTime.UtcNow;
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
                order.UpatedDate = DateTime.UtcNow;
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
                    order.HasPay = pay;
                }
                if (pay > order.SubTotal)
                {
                    pay = order.SubTotal;
                    order.HasPay = pay;
                }
                order.PaymentMethol = paymentType;
                order.UpatedDate = DateTime.UtcNow;
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
            frDate = TimeZoneInfo.ConvertTimeToUtc(frDate, curentZone);
            tDate = TimeZoneInfo.ConvertTimeToUtc(tDate, curentZone);
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
                        HasPayTransfer = c.T_Order.HaspayTransfer ?? 0,
                        IsCompleted = c.IsCompleted,
                        HasPayTotal = 0,
                        HasExistTotal = 0,
                        HasPayTransferTotal = 0,
                        strIsComplete = c.IsCompleted ? "Đã Xong" : "Chưa Xong",
                        strDesignStatus = c.DesignStatus == null ? "Chưa Làm" : (c.DesignStatus == 1 ? "Đang Làm" : (c.DesignStatus == 2 ? "Đã Xong" : "Chưa Làm")),
                        strPrinStatus = c.PrintStatus == null ? "Chưa Làm" : (c.PrintStatus == 1 ? "Đang Làm" : (c.PrintStatus == 2 ? "Đã Xong" : "Chưa Làm"))
                    }).OrderBy("CreatedDate DESC").ToList();
            if (employee != 0)
            {
                orders = orders.Where(c => c.CreatedForUser == employee).ToList();
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                orders = orders.Where(c => c.CustomerName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) || c.CustomerPhone.Contains(keyWord) || c.OrderId.ToString().Contains(keyWord)).ToList();
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
            return detail.Price ?? 0;
        }
        public PagedList<ModelViewDetail> GetListViewDetail(string keyWord, int startIndexRecord, int pageSize, string sorting, int orderId)
        {
            sorting = "CreatedDate DESC";
            var orders =
                _repOrderDetail.GetMany(c => !c.IsDeleted && !c.T_Order.IsDeleted && c.T_Order.Id == orderId)
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
                        DesignView = c.DesignView ?? "",
                        PrintView = c.PrintView ?? "",
                        AddOnView = c.AddOnView ?? "",
                        DetailStatus = c.DetailStatus,

                    }).OrderBy(sorting);
            if (!string.IsNullOrEmpty(keyWord))
            {
                orders = orders.Where(c => c.CustomerName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) || c.CustomerPhone.Contains(keyWord));
            }
            var pageNumber = (startIndexRecord / pageSize) + 1;
            var result = new PagedList<ModelViewDetail>(orders, pageNumber, pageSize);
            foreach (var order in result)
            {
                if (order.DetailStatus == 1 || order.DetailStatus == 2 || order.DetailStatus == 3)
                {
                    order.UserProcess = order.DesignView;
                }
                if (order.DetailStatus == 4 || order.DetailStatus == 5)
                {
                    order.UserProcess = order.PrintView;
                }
                if (order.DetailStatus == 6 || order.DetailStatus == 7)
                {
                    order.UserProcess = order.AddOnView;
                }
                if (string.IsNullOrEmpty(order.UserProcess))
                {
                    order.UserProcess = "Chưa có nhân viên phụ trách";
                }
                order.strSubTotal = $"{order.SubTotal:0,0}";
                order.strPrice = $"{order.Price:0,0}";
            }
            return result;

        }

        public ResponseBase DesignUpdateOrderDetail(int orderId, string fileName, string description)
        {
            var responResult = new ResponseBase();
            var order = _repOrderDetail.GetById(c => c.Id == orderId);
            if (order != null)
            {
                order.FileName = fileName;
                order.DesignDescription = description;
                order.UpatedDate = DateTime.UtcNow;
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
    }
}

