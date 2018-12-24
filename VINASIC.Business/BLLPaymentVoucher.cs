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
using System.Configuration;

namespace VINASIC.Business
{
    public class BllPaymentVoucher : IBllPaymentVoucher
    {
        private readonly IT_PaymentVoucherRepository _repPaymentVoucher;
        private readonly IT_PaymentVoucherDetailRepository _repPaymentDetailVoucher;
        private readonly IT_PartnerRepository _repPartner;
        private readonly TimeZoneInfo curentZone = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["WEBSITE_TIME_ZONE"]);
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllPaymentVoucher(IUnitOfWork<VINASICEntities> unitOfWork, IT_PartnerRepository repPartner, IT_PaymentVoucherRepository repPaymentVoucher, IT_PaymentVoucherDetailRepository repPaymentDetailVoucher)
        {
            _unitOfWork = unitOfWork;
            _repPaymentVoucher = repPaymentVoucher;
            _repPaymentDetailVoucher = repPaymentDetailVoucher;
            _repPartner = repPartner;

        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckPaymentVoucherName(string paymentVoucherName, int id)
        {
            var checkResult = false;
            var checkName = _repPaymentVoucher.GetMany(c => !c.IsDeleted && c.Id != id && c.Content.Trim().ToUpper().Equals(paymentVoucherName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelPaymentVoucher> GetListProduct()
        {
            var paymentVoucher = _repPaymentVoucher.GetMany(c => !c.IsDeleted).Select(c => new ModelPaymentVoucher()
            {
                Id = c.Id,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return paymentVoucher;
        }

        public ResponseBase CreateOrder(ModelSavePaymentVoucher obj, int userId)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            if (obj != null)
            {
                int baseCustomerId;
                if (obj.CustomerId != 0)
                {
                    var cus = _repPartner.Get(x => x.Id == obj.CustomerId);
                    cus.Name = obj.CustomerName;
                    cus.Address = obj.CustomerAddress;
                    cus.Mobile = obj.CustomerPhone;
                    cus.Email = obj.CustomerMail;
                    cus.TaxCode = obj.CustomerTaxCode;
                    cus.UpdatedDate = DateTime.UtcNow;
                    cus.UpdatedUser = userId;
                    baseCustomerId = obj.CustomerId;
                    _repPartner.Update(cus);
                    SaveChange();
                }
                else
                {
                    var cus = new T_Partner
                    {
                        Name = obj.CustomerName,
                        Address = obj.CustomerAddress,
                        Mobile = obj.CustomerPhone,
                        Email = obj.CustomerMail,
                        TaxCode = obj.CustomerTaxCode,
                        CreatedDate = DateTime.UtcNow,
                        CreatedUser = userId
                    };
                    _repPartner.Add(cus);
                    SaveChange();
                    baseCustomerId = cus.Id;
                }

                var order = new T_PaymentVoucher
                {
                    Content=obj.Content,
                    Money=obj.OrderTotal != 0?obj.OrderTotal : obj.totalInclude,
                    PaymentDate=DateTime.UtcNow,
                    ReceiptName=obj.CustomerName,
                    ReceiptAddress=obj.CustomerAddress,
                    ReceiptPhone=obj.CustomerPhone,
                    IsDeleted = false,
                    CreatedUser = userId,
                    CreatedDate = DateTime.UtcNow
                };
                _repPaymentVoucher.Add(order);
                SaveChange();
                var id = 1;
                if (obj.Detail != null)
                {
                    foreach (var detail in obj.Detail)
                    {
                        var orderDetail = new T_PaymentVoucherDetail
                        {
                            Index = detail.Index,
                            PaymentVoucherId = order.Id,
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
                        _repPaymentDetailVoucher.Add(orderDetail);
                        SaveChange();
                        id++;
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
        public List<ModelOrderDetail> GetListOrderDetailByOrderId(int orderId)
        {

            var ordeDetails = _repPaymentDetailVoucher.GetMany(o => !o.IsDeleted && o.PaymentVoucherId == orderId).Select(o => new
                ModelOrderDetail()
            {
                Id = o.Id,
                OrderId = o.PaymentVoucherId,
                CommodityId = o.CommodityId,
                CommodityName = o.CommodityName,
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
                order.strSubTotal = $"{order.SubTotal:0,0}";
                order.strPrice = $"{order.Price:0,0}";
            }
            return ordeDetails;
        }
        public ResponseBase UpdatedOrder(ModelSavePaymentVoucher obj, int userId, bool isAdmin)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            if (obj != null)
            {
                var cus = _repPartner.Get(x => x.Id == obj.CustomerId);
                cus.Name = obj.CustomerName;
                cus.Address = obj.CustomerAddress;
                cus.Mobile = obj.CustomerPhone;
                cus.Email = obj.CustomerMail;
                cus.TaxCode = obj.CustomerTaxCode;
                cus.UpdatedDate = DateTime.UtcNow;
                cus.UpdatedUser = userId;
                _repPartner.Update(cus);
                SaveChange();


                var order = _repPaymentVoucher.Get(x => x.Id == obj.OrderId);
                order.Content = obj.Content;
                order.Money = obj.OrderTotal != 0 ? obj.OrderTotal : obj.totalInclude;
                order.PaymentDate = DateTime.UtcNow;
                order.ReceiptName = obj.CustomerName;
                order.ReceiptAddress = obj.CustomerAddress;
                order.ReceiptPhone = obj.CustomerPhone;
                order.UpdatedUser = userId;
                order.UpatedDate = DateTime.UtcNow;
                _repPaymentVoucher.Update(order);
                SaveChange();
                var baseOrderDetail = _repPaymentDetailVoucher.GetMany(x => x.PaymentVoucherId == order.Id).ToList();
                var deleteDetail = baseOrderDetail.Where(x => !obj.Detail.Select(y => y.Id).Contains(x.Id)).ToList();
                foreach (var detail in deleteDetail)
                {
                    _repPaymentDetailVoucher.Delete(detail);
                    SaveChange();
                }
                if (obj.Detail != null)
                {
                    foreach (var detail in obj.Detail)
                    {
                        var detailUpdate = baseOrderDetail.Where(x => x.Id == detail.Id).FirstOrDefault();
                        if (detailUpdate != null && detail.Id != 0 && detailUpdate.CommodityId == int.Parse(detail.CommodityId))
                        {
                            detailUpdate.PaymentVoucherId = order.Id;
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
                            _repPaymentDetailVoucher.Update(detailUpdate);
                            SaveChange();
                        }
                        else
                        {
                            if (detailUpdate != null)
                            {
                                _repPaymentDetailVoucher.Delete(detailUpdate);
                                SaveChange();
                            }
                            var orderDetail = new T_PaymentVoucherDetail
                            {
                                PaymentVoucherId = order.Id,
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
                            _repPaymentDetailVoucher.Add(orderDetail);
                            SaveChange();
                        }
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
        //public ResponseBase Create(ModelPaymentVoucher obj)
        //{
        //    ResponseBase result = new ResponseBase { IsSuccess = false };
        //    try
        //    {
        //        if (obj != null)
        //        {
        //                var paymentVoucher = new T_PaymentVoucher();
        //                Parse.CopyObject(obj, ref paymentVoucher);
        //                paymentVoucher.CreatedDate = DateTime.Now.AddHours(14);
        //                _repPaymentVoucher.Add(paymentVoucher);
        //                SaveChange();
        //                result.IsSuccess = true;
        //        }
        //        else
        //        {
        //            result.IsSuccess = false;
        //            result.Errors.Add(new Error() { MemberName = "Create PaymentVoucher", Message = "Đối Tượng Không tồn tại" });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        result.IsSuccess = false;
        //        result.Errors.Add(new Error() { MemberName = "Create PaymentVoucher", Message = "Đã có lỗi xảy ra" });
        //    }
        //    return result;
        //}

        //public ResponseBase Update(ModelPaymentVoucher obj)
        //{

        //    ResponseBase result = new ResponseBase { IsSuccess = false };
        //        T_PaymentVoucher paymentVoucher = _repPaymentVoucher.Get(x => x.Id == obj.Id && !x.IsDeleted);
        //        if (paymentVoucher != null)
        //        {
        //            paymentVoucher.ReceiptAddress = obj.ReceiptAddress;
        //            paymentVoucher.ReceiptName = obj.ReceiptName;
        //            paymentVoucher.PaymentDate = obj.PaymentDate;
        //            paymentVoucher.Content = obj.Content;
        //            paymentVoucher.Note = obj.Note;
        //            paymentVoucher.Money = obj.Money;
        //            paymentVoucher.UpdatedUser = obj.UpdatedUser;
        //            _repPaymentVoucher.Update(paymentVoucher);
        //            SaveChange();
        //            result.IsSuccess = true;
        //        }
        //        else
        //        {
        //            result.IsSuccess = false;
        //            result.Errors.Add(new Error() { MemberName = "UpdatePaymentVoucher", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
        //        }

        //    return result;
        //}
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var paymentVoucher = _repPaymentVoucher.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (paymentVoucher != null)
            {
                paymentVoucher.IsDeleted = true;
                paymentVoucher.DeletedUser = userId;
                paymentVoucher.DeletedDate = DateTime.Now.AddHours(14);
                _repPaymentVoucher.Update(paymentVoucher);
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
        public List<ModelSelectItem> GetListPaymentVoucher()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            listModelSelect.AddRange(_repPaymentVoucher.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Content }));
            return listModelSelect;
        }
        public PagedList<ModelPaymentVoucher> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var paymentVouchers = _repPaymentVoucher.GetMany(c => !c.IsDeleted).Select(c => new ModelPaymentVoucher()
            {
                Id = c.Id,
                Content = c.Content,
                Note = c.Note,
                Money = c.Money,
                ReceiptAddress = c.ReceiptAddress??"",
                ReceiptName = c.ReceiptName??"",
                PaymentDate = c.PaymentDate,
                CreatedDate = c.CreatedDate,
                ReceiptPhone=c.ReceiptPhone,
                T_PaymentVoucherDetail=c.T_PaymentVoucherDetail,
            }).OrderBy(sorting).ToList();
            foreach (var order in paymentVouchers)
            {

                order.StrCreatedDate = $"{ TimeZoneInfo.ConvertTimeFromUtc(order.CreatedDate, curentZone):d/M/yyyy HH:mm}";
              
            }
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelPaymentVoucher>(paymentVouchers, pageNumber, pageSize);
        }
    }
}

