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
    public class BLLDashBoard : IBLLDashBoard
    {
        private readonly IT_OrderRepository _repOrder;
        private readonly IT_PaymentVoucherRepository _repPaymentVoucher;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        private readonly TimeZoneInfo curentZone = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["WEBSITE_TIME_ZONE"]);
        public BLLDashBoard(IUnitOfWork<VINASICEntities> unitOfWork, IT_PaymentVoucherRepository repPaymentVoucher, IT_OrderRepository repOrder)
        {
            _unitOfWork = unitOfWork;
            _repOrder = repOrder;
            _repPaymentVoucher = repPaymentVoucher;
        }

        public ModelDashBoard GetData(string fromDate, string todate)
        {
            var result = new ModelDashBoard();
            var realfromDate = DateTime.Parse(fromDate);
            var realtoDate = DateTime.Parse(todate);
            var frDate = new DateTime(realfromDate.Year, realfromDate.Month, realfromDate.Day, 0, 0, 0, 0);
            frDate = TimeZoneInfo.ConvertTimeToUtc(frDate, curentZone);
            var tDate = new DateTime(realtoDate.Year, realtoDate.Month, realtoDate.Day, 23, 59, 59, 999);
            tDate = TimeZoneInfo.ConvertTimeToUtc(tDate, curentZone);
            var orders = _repOrder.GetMany(c => !c.IsDeleted && c.CreatedDate >= frDate && c.CreatedDate <= tDate);
            var payments= _repPaymentVoucher.GetMany(c => !c.IsDeleted && c.CreatedDate >= frDate && c.CreatedDate <= tDate);
            var dashBoardOrder = new ModelDashBoardOrder();
            var sum = orders.Sum(x => x.SubTotal);
            dashBoardOrder.Value1 = orders.Count()>0? orders?.Sum(x => x.HasPay??0):0;
            dashBoardOrder.Value2 = orders.Count() > 0 ? orders?.Sum(x => x.HaspayTransfer??0):0;
            dashBoardOrder.Value3 = sum- (dashBoardOrder.Value1 + dashBoardOrder.Value2);
            result.ModelDashBoardOrder = dashBoardOrder;

            var dashBoardPayment = new ModelDashBoardPayment();
            var sum1 = payments.Count()>0? payments?.Sum(x => x.Money):0;
            dashBoardPayment.Value1 = payments.Count() > 0 ? payments?.Sum(x => x.HasPay ?? 0):0;
            dashBoardPayment.Value2 = sum1 - dashBoardPayment.Value1;
            result.ModelDashBoardPayment = dashBoardPayment;

            var dashBoardSum = new ModelDashBoardSum();
            dashBoardSum.Value1 = sum;
            dashBoardSum.Value2 = sum1;
            dashBoardSum.Value3= dashBoardOrder.Value1 + dashBoardOrder.Value2;            
            dashBoardSum.Value4 = dashBoardPayment.Value1;
            result.ModelDashBoardSum = dashBoardSum;
            return result;
        }

        private void SaveChange()
        {
            _unitOfWork.Commit();
        }
    }
}

