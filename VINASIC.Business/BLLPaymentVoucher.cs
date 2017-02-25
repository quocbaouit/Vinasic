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
    public class BllPaymentVoucher : IBllPaymentVoucher
    {
        private readonly IT_PaymentVoucherRepository _repPaymentVoucher;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllPaymentVoucher(IUnitOfWork<VINASICEntities> unitOfWork, IT_PaymentVoucherRepository repPaymentVoucher)
        {
            _unitOfWork = unitOfWork;
            _repPaymentVoucher = repPaymentVoucher;
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

        public ResponseBase Create(ModelPaymentVoucher obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                        var paymentVoucher = new T_PaymentVoucher();
                        Parse.CopyObject(obj, ref paymentVoucher);
                        paymentVoucher.CreatedDate = DateTime.Now.AddHours(14);
                        _repPaymentVoucher.Add(paymentVoucher);
                        SaveChange();
                        result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create PaymentVoucher", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create PaymentVoucher", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelPaymentVoucher obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
                T_PaymentVoucher paymentVoucher = _repPaymentVoucher.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (paymentVoucher != null)
                {
                    paymentVoucher.ReceiptAddress = obj.ReceiptAddress;
                    paymentVoucher.ReceiptName = obj.ReceiptName;
                    paymentVoucher.PaymentDate = obj.PaymentDate;
                    paymentVoucher.Content = obj.Content;
                    paymentVoucher.Note = obj.Note;
                    paymentVoucher.Money = obj.Money;
                    paymentVoucher.UpdatedUser = obj.UpdatedUser;
                    _repPaymentVoucher.Update(paymentVoucher);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdatePaymentVoucher", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            
            return result;
        }
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
                ReceiptAddress = c.ReceiptAddress,
                ReceiptName = c.ReceiptName,
                PaymentDate = c.PaymentDate,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelPaymentVoucher>(paymentVouchers, pageNumber, pageSize);
        }
    }
}

