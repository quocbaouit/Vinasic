using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using Dynamic.Framework.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Helpers;
using VINASIC.Infrastructure.ActionExtention;
using VINASIC.Models;

namespace VINASIC.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IBllOrder _bllOrder;
        private readonly IBllProductType _bllProductType;
        private readonly IBllProduct _bllProduct;
        private readonly IBllEmployee _bllEmployee;
        private readonly IBllCustomer _bllCustomer;
        private readonly IBllSiteSetting _bllSiteSetting;
        private readonly IBllContent _bllContent;
        private readonly IBllOrderStatus _bllOrderStatus;
        private readonly IBllOrderDetailStatus _bllOrderDetailStatus;

        public OrderController(IBllOrder bllOrder, IBllContent bllContent, IBllSiteSetting bllSiteSetting, IBllEmployee bllEmployee, IBllCustomer bllCustomer, IBllProductType bllProductType, IBllProduct bllProduct, IBllOrderStatus bllOrderStatus, IBllOrderDetailStatus bllOrderDetailStatus)
        {
            _bllOrder = bllOrder;
            _bllSiteSetting = bllSiteSetting;
            _bllEmployee = bllEmployee;
            _bllCustomer = bllCustomer;
            _bllProductType = bllProductType;
            _bllProduct = bllProduct;
            _bllContent = bllContent;
            _bllOrderStatus = bllOrderStatus;
            _bllOrderDetailStatus = bllOrderDetailStatus;
        }
        public ActionResult Index()
        {
            var employee = _bllEmployee.GetUserById(UserContext.UserID);
            var showDim = _bllSiteSetting.ChecConfig("configDimension");
            var CompanyInfo = _bllSiteSetting.GetListProduct();
            ViewBag.Employee = employee;
            ViewBag.ShowDim = showDim;
            if (showDim)
            {
                ViewBag.CssShowDim = "inline";
            }
            else
            {
                ViewBag.CssShowDim = "none";
            }
            //company
            ViewBag.cmpShortName = CompanyInfo.Where(x => x.Code == "cmpShortName").FirstOrDefault()?.Value;
            ViewBag.cpnWebsite = CompanyInfo.Where(x => x.Code == "cpnWebsite").FirstOrDefault()?.Value;
            ViewBag.cpnLogo = CompanyInfo.Where(x => x.Code == "cpnLogo").FirstOrDefault()?.Value;
            ViewBag.cpnMobile = CompanyInfo.Where(x => x.Code == "cpnMobile").FirstOrDefault()?.Value;
            ViewBag.cpnAddress = CompanyInfo.Where(x => x.Code == "cpnAddress").FirstOrDefault()?.Value;
            ViewBag.cpnName = CompanyInfo.Where(x => x.Code == "cpnName").FirstOrDefault()?.Value;
            //company

            var orderStatus = _bllOrderStatus.GetListOrderStatus();
            var orderDetailStatus = _bllOrderDetailStatus.GetListOrderDetailStatus();
            ViewBag.OrderStatus = orderStatus;
            ViewBag.OrderDetailStatus = orderDetailStatus;

            return View();
        }

        public ActionResult OrderReport()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult GetOrders(int jtStartIndex = 0, int jtPageSize = 10, string jtSorting = "", string keyword = "", int employee = 0, string fromDate = "", string toDate = "", float orderStatus = -1)
        {
            try
            {
                if (fromDate == string.Empty || toDate == string.Empty)
                {
                    JsonDataResult.Result = "OK";
                    return Json(JsonDataResult);
                }
                if (employee == 0 && !UserContext.Permissions.Contains("IsViewAll"))
                {
                    employee = UserContext.UserID;
                }
                var listOrder = _bllOrder.GetList(UserContext.UserID, jtStartIndex, jtPageSize, jtSorting, fromDate, toDate, employee, keyword, orderStatus);

                JsonDataResult.Records = listOrder;
                dynamic Sum = new System.Dynamic.ExpandoObject();
                var sumHaspay = listOrder.Sum(x => x.HaspayTransfer);
                var sumHaspayTransfer = listOrder.Sum(x => x.HasPay);
                var sumSubTotal = listOrder.Sum(x => x.SubTotal);
                var sumRemaining = sumSubTotal - (sumHaspay + sumHaspayTransfer);
                Sum.sumHaspay = sumHaspay;
                Sum.sumHaspayTransfer = sumHaspayTransfer;
                Sum.sumRemaining = sumRemaining;
                JsonDataResult.Result = "OK";

                JsonDataResult.Data = Sum;
                JsonDataResult.TotalRecordCount = listOrder.TotalItemCount;
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult GetListViewDetail(int jtStartIndex = 0, int jtPageSize = 10, string jtSorting = "", string keyword = "", int orderId = 0)
        {
            try
            {
                var listOrder = _bllOrder.GetListViewDetail(keyword, jtStartIndex, jtPageSize, jtSorting, orderId);
                JsonDataResult.Records = listOrder;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listOrder.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult DeleteOrder(int id)
        {
            var IsAdmin = UserContext.Permissions.Contains("isAdmin");
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrder.DeleteById(id, UserContext.UserID, IsAdmin);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Delete ", Message = "Tài Khoản của bạn không có quyền này." });
                }
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Delete BankBranch", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult ListOrderDetail(int orderId)
        {
            try
            {
                Thread.Sleep(200);
                var orderDetail = _bllOrder.GetListOrderDetailByOrderId(orderId);
                return Json(new { Result = "OK", Records = orderDetail });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        public JsonResult UpdateHasTax(int id, int hasTax)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrder.UpdateHasTax(id, hasTax, UserId);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult UpdatePayment(int orderId, string payment, int paymentType, string transferDescription)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var pay = float.Parse(payment);
                    var responseResult = _bllOrder.UpdatePayment(orderId, pay, paymentType, UserId, transferDescription);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult UpdateApproval(int orderId, bool approval)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrder.UpdateApproval(orderId, approval, UserId);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult UpdateDelivery(int orderId, int delivery)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrder.UpdateDelivery(orderId, delivery, UserId);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult SaveOrder(int orderId, int employeeId, int customerId, string customerName, string customerPhone, string customerMail, string customerAddress, string customerTaxCode, string dateDelivery, float orderTotal, List<ModelDetail> listDetail, bool tax, float orderTotalTax, float deposit)
        {
            try
            {
                var IsAdmin = UserContext.Permissions.Contains("isAdmin");
                if (IsAuthenticate)
                {
                    var saveOrder = new ModelSaveOrder
                    {
                        OrderId = orderId,
                        EmployeeId = employeeId,
                        OrderTotal = orderTotalTax,
                        CustomerId = customerId,
                        CustomerName = customerName,
                        CustomerPhone = customerPhone,
                        CustomerMail = customerMail,
                        CustomerAddress = customerAddress,
                        CustomerTaxCode = customerTaxCode,
                        Tax = tax,
                        Deposit = deposit,
                        OrderTotalExcludeTax = orderTotal,
 
                        DateDelivery = DateTime.Parse(dateDelivery ?? DateTime.Now.AddHours(14).ToString(CultureInfo.InvariantCulture)),
                        Detail = listDetail
                    };
                    var responseResult = saveOrder.OrderId == 0 ? _bllOrder.CreateOrder(saveOrder, UserContext.UserID) : _bllOrder.UpdatedOrder(saveOrder, UserContext.UserID, IsAdmin);
                    if (!responseResult.IsSuccess)
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                    else
                    {
                        JsonDataResult.Result = "OK";
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }


            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult GetAllCustomer()
        {
            try
            {
                Thread.Sleep(200);
                var customers = _bllCustomer.GetAllCustomerName();
                return Json(new { Result = "OK", Records = customers });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        public JsonResult GetListProductType()
        {
            try
            {
                List<SelectListItem> listValues = new List<SelectListItem>();
                var listProductType = _bllProductType.GetListProductType();
                if (listProductType != null)
                {
                    listValues = listProductType.Select(c => new SelectListItem()
                    {
                        Value = c.Value.ToString(),
                        Text = c.Name
                    }).ToList();
                }
                return Json(listValues, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult GetListProduct(int productType)
        {
            try
            {
                List<SelectListItemExtent> listValues = new List<SelectListItemExtent>();
                var listProductType = _bllProduct.GetListProduct(productType);
                if (listProductType != null)
                {
                    listValues = listProductType.Select(c => new SelectListItemExtent()
                    {
                        Value = c.Value.ToString(),
                        Text = c.Name,
                        Type = c.Type,
                        Code = c.Code,
                    }).ToList();
                }
                return Json(listValues, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult GetCustomerById(int customerId)
        {
            try
            {
                Thread.Sleep(200);
                var customer = _bllCustomer.GetCustomerById(customerId);
                return Json(new { Result = "OK", Records = customer });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        public JsonResult GetCustomerByName(string customerName)
        {
            try
            {
                Thread.Sleep(200);
                var customer = _bllCustomer.GetCustomerByName(customerName);
                return Json(new { Result = "OK", Records = customer });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        public JsonResult GetProductPrice(int type)
        {
            try
            {
                var result = _bllContent.GetContentByType(type);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult GetCustomerByPhone(string phoneNumber)
        {
            try
            {
                Thread.Sleep(200);
                var customer = _bllCustomer.GetCustomerByPhone(phoneNumber);
                return Json(new { Result = "OK", Records = customer });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        public JsonResult GetTest(string phoneNumber)
        {
            try
            {
                Thread.Sleep(200);
                var customer = _bllCustomer.GetCustomerByPhone(phoneNumber);
                return Json(new { Result = "OK", Records = customer });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        public JsonResult GetCustomerByOrganization(string shortName)
        {
            try
            {
                List<SelectListItem> listValues = new List<SelectListItem>();
                var IsViewAll = UserContext.Permissions.Contains("IsViewAll");
                var listProductType = _bllEmployee.GetCustomerByOrganization(shortName, IsViewAll, UserContext.UserID);
                if (listProductType != null)
                {
                    listValues = listProductType.Select(c => new SelectListItem()
                    {
                        Value = c.Value.ToString(),
                        Text = c.Name
                    }).ToList();
                }
                return Json(listValues, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult UpdateDesignUser(int detailId, int employeeId, string description)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrder.UpdateDesignUser(detailId, employeeId, description);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult UpdateHaspay(int orderId, string haspay)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrder.UpdateHaspay(orderId, haspay);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult UpdateOrderStatus(int orderId, int status, bool sendSMS = false, bool sendEmail = false)
        {
            try
            {
                var IsAdmin = true;
                //if (IsAuthenticate)
                //{
                var responseResult = _bllOrder.UpdateOrderStatus(orderId, status, UserContext.UserID, IsAdmin, sendSMS, sendEmail);
                if (responseResult.IsSuccess)
                    JsonDataResult.Result = "OK";
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
                //}
                //else
                //{
                //    JsonDataResult.Result = "ERROR";
                //    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                //}
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult UpdateHaspayCustom(int orderId, string haspay, int paymentType)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrder.UpdateHaspayCustom(orderId, haspay, paymentType);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult UpdateCost([System.Web.Http.FromBody]List<CostObj> CostObj, int orderId, float cost)
        {
            try
            {
                //if (IsAuthenticate)
                //{
                var responseResult = _bllOrder.UpdateCost(CostObj, orderId, cost);
                if (responseResult.IsSuccess)
                    JsonDataResult.Result = "OK";
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
                //}
                //else
                //{
                //    JsonDataResult.Result = "ERROR";
                //    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                //}
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult UpdatePrintUser(int detailId, int employeeId, string description)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrder.UpdatePrintUser(detailId, employeeId, description);
                    if (responseResult.IsSuccess)
                        JsonDataResult.Result = "OK";
                    else
                    {
                        JsonDataResult.Result = "ERROR";
                        JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                    }
                }
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult UpdateDetailStatus(int detailId, int status, int employeeId, string content, [FromBody] string content1)
        {
            try
            {
                //if (IsAuthenticate)
                //{
                var responseResult = _bllOrder.UpdateDetailStatus(detailId, status, employeeId, content1);
                if (responseResult.IsSuccess)
                    JsonDataResult.Result = "OK";
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
                //}
                //else
                //{
                //    JsonDataResult.Result = "ERROR";
                //    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                //}
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult GetJobDescriptionForEmployee(int detailId, int status, int employeeId, string content)
        {
            try
            {
                //if (IsAuthenticate)
                //{
                var responseResult = _bllOrder.GetJobDescriptionForEmployee(detailId, status, employeeId, content);
                if (responseResult.IsSuccess)
                {
                    JsonDataResult.Result = "OK";
                    JsonDataResult.Data = responseResult.Data;
                }

                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
                //}
                //else
                //{
                //    JsonDataResult.Result = "ERROR";
                //    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                //}
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult EmployeeUpdateDetailStatus(int detailId, int status, int employeeId,int updateType)
        {
            try
            {
                //if (IsAuthenticate)
                //{
                //if (employeeId == 0)
                    employeeId = UserContext.UserID;
                var responseResult = _bllOrder.EmployeeUpdateDetailStatus(detailId, status, employeeId, updateType);
                if (responseResult.IsSuccess)
                    JsonDataResult.Result = "OK";
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
                //}
                //else
                //{
                //    JsonDataResult.Result = "ERROR";
                //    JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update ", Message = "Tài Khoản của bạn không có quyền này." });
                //}
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        //public JsonResult ImportFromExcel(int detailId, int employeeId, string description)
        //{         
        //    try
        //    {
        //        //if (isAuthenticate)
        //        //{
        //        using (ExcelPackage pck = new ExcelPackage())
        //        {
        //            using (var stream = File.OpenRead(openFileDialog1.FileName))
        //            {
        //                pck.Load(stream);
        //            }

        //            ExcelWorksheet ws = pck.Workbook.Worksheets.First();
        //            DataGridView1.DataSource = WorksheetToDataTable(ws, chkHasHeader.Checked);
        //        }
        //        var responseResult = _bllOrder.UpdatePrintUser(detailId, employeeId, description);
        //        if (responseResult.IsSuccess)
        //            JsonDataResult.Result = "OK";
        //        else
        //        {
        //            JsonDataResult.Result = "ERROR";
        //            JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
        //        }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        //add error
        //        JsonDataResult.Result = "ERROR";
        //        JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
        //    }
        //    return Json(JsonDataResult);
        //}
        private DataTable WorksheetToDataTable(ExcelWorksheet ws, bool hasHeader = true)
        {
            DataTable dt = new DataTable(ws.Name);
            int totalCols = ws.Dimension.End.Column;
            int totalRows = ws.Dimension.End.Row;
            int startRow = hasHeader ? 2 : 1;
            ExcelRange wsRow;
            DataRow dr;
            foreach (var firstRowCell in ws.Cells[1, 1, 1, totalCols])
            {
                dt.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
            }

            for (int rowNum = startRow; rowNum <= totalRows; rowNum++)
            {
                wsRow = ws.Cells[rowNum, 1, rowNum, totalCols];
                dr = dt.NewRow();
                foreach (var cell in wsRow)
                {
                    dr[cell.Start.Column - 1] = cell.Text;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }
        public ActionResult ExportReport([FromUri] DateTime fromDate, [FromUri]DateTime toDate, [FromUri]int employee, [FromUri]string keySearch, int delivery = 0, int paymentStatus = 0, int type = 0,string orderIds=null)
        {
            var orderids = JsonConvert.DeserializeObject<List<int>>(orderIds);
            var pck = new ExcelPackage();
            pck = ExportSum(pck, fromDate, toDate, employee, keySearch, delivery, paymentStatus, type, orderids);
            return new ExcelDownload(pck, string.Format("{0}_.xlsx", DateTime.Now.AddHours(14).ToString("d")));
        }
        public ActionResult ExportExcelQuotation(int orderId, string orderName)
        {
            var pck = new ExcelPackage();
            pck = Export(pck, orderId);
            return new ExcelDownload(pck, string.Format("{0}_{1}.xlsx", orderName, DateTime.Now.AddHours(14).ToString("d")));
        }
        //public 
        public ExcelPackage ExportSum(ExcelPackage package, DateTime fromDate, DateTime toDate, int employee, string keySearch, int delivery, int paymentStatus, int type = 0,List<int> orderIds=null)
        {
            var CompanyInfo = _bllSiteSetting.GetListProduct();
            var cmpShortName = CompanyInfo.Where(x => x.Code == "cmpShortName").FirstOrDefault()?.Value;
            var cpnWebsite = CompanyInfo.Where(x => x.Code == "cpnWebsite").FirstOrDefault()?.Value;
            var cpnLogo = CompanyInfo.Where(x => x.Code == "cpnLogo").FirstOrDefault()?.Value;
            var cpnMobile = CompanyInfo.Where(x => x.Code == "cpnMobile").FirstOrDefault()?.Value;
            var cpnAddress = CompanyInfo.Where(x => x.Code == "cpnAddress").FirstOrDefault()?.Value;
            var cpnName = CompanyInfo.Where(x => x.Code == "cpnName").FirstOrDefault()?.Value;

            var result = _bllOrder.ExportReport(fromDate, toDate, employee, keySearch, delivery, paymentStatus, type, orderIds);
            var customerName = result[0].CustomerName;
            var customerAddress = result[0].CustomerAddress;
            var CustomerPhone = result[0].CustomerPhone;
            var siteSettings = _bllSiteSetting.GetListProduct();
            var configCustomer = siteSettings.Where(x => x.Code == "configCustomer").FirstOrDefault().Value;
            var configUnit = siteSettings.Where(x => x.Code == "configUnit").FirstOrDefault().Value;
            var configDimension = siteSettings.Where(x => x.Code == "configDimension").FirstOrDefault().Value;
            if (result.Count == 0)
            {
                return package;
            }
            var ws = package.Workbook.Worksheets.Add("Thống Kê");

            ws.Cells.Style.Font.Size = 14;
            ws.Cells.Style.Font.Name = "Times New Roman";
            var columnNumber = 1;
            //ws.Column(columnNumber).Width = 7;
            //columnNumber++;
            //ws.Column(columnNumber).Width = 7;
            //columnNumber++;
            //ws.Column(columnNumber).Width = 15;
            //columnNumber++;
            //if (configCustomer == "true")//customerName
            //{
            //    ws.Column(columnNumber).Width = 30;
            //    columnNumber++;
            //}

           
            ws.Column(columnNumber).Width = 30;
            columnNumber++;
            if (configUnit == "true")
            {
                ws.Column(columnNumber).Width = 8;
                columnNumber++;
            }
            if (configDimension == "true")
            {
                ws.Column(columnNumber).Width = 8;
                columnNumber++;
            }
            if (configDimension == "true")
            {
                ws.Column(columnNumber).Width = 8;
                columnNumber++;
            }

            ws.Column(columnNumber).Width = 30;
            columnNumber++;
            ws.Column(columnNumber).Width = 30;
            columnNumber++;
            ws.Column(columnNumber).Width = 30;
            columnNumber++;
            ws.Column(columnNumber).Width = 30;
            columnNumber++;
            ws.Column(columnNumber).Width = 30;
            columnNumber++;
            //ws.Column(columnNumber).Width = 14;
            //columnNumber++;
            //ws.Column(columnNumber).Width = 14;
            //columnNumber++;
            //ws.Column(columnNumber).Width = 14;
            string path = cpnLogo;
            var logo = Image.FromFile(Server.MapPath(path));
            ws.Row(0 * 5).Height = 39.00D;
            var picture = ws.Drawings.AddPicture(0.ToString(), logo);
            picture.From.Column = 0;
            picture.From.Row = 0;
            picture.To.Column = 0;
            picture.To.Row = 0;
            picture.SetSize(200, 104);

            ws.Cells["D1"].Value = cpnName;
            ws.Cells["D1"].Style.Font.Bold = true;
            ws.Cells["D1"].Style.Font.Size = 16;
            ws.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //ws.Cells["G2"].Value = cpnWebsite;
            //ws.Cells["G2"].Style.Font.Bold = true;
            //ws.Cells["G2"].Style.Font.Size = 14;
            //ws.Cells["G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //ws.Cells["G2"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["D2"].Value = "Địa chỉ văn phòng: " + cpnAddress;
            ws.Cells["D2"].Style.Font.Bold = true;
            ws.Cells["D2"].Style.Font.Size = 14;
            ws.Cells["D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["D3"].Value = "Di động: " + cpnMobile;
            ws.Cells["D3"].Style.Font.Bold = true;
            ws.Cells["D3"].Style.Font.Size = 14;
            ws.Cells["D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["D5"].Value = "PHIẾU THU";
            ws.Cells["D5"].Style.Font.Bold = true;
            ws.Cells["D5"].Style.Font.Size = 18;
            ws.Cells["D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["A7"].Value = "Họ và tên người nôp tiền: "+ customerName;
            //ws.Cells["A7"].Style.Font.Bold = true;
            ws.Cells["A7"].Style.Font.Size = 14;
            ws.Cells["A7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells["A8"].Value = "Địa chỉ: "+customerAddress;
            //ws.Cells["A8"].Style.Font.Bold = true;
            ws.Cells["A8"].Style.Font.Size = 14;
            ws.Cells["A8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells["A9"].Value = "Số đt: "+CustomerPhone;
            //ws.Cells["A9"].Style.Font.Bold = true;
            ws.Cells["A9"].Style.Font.Size = 14;
            ws.Cells["A9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


            ws.Cells["A14"].Value = "Giám đốc(Ký, họ tên)";
            ws.Cells["A14"].Style.Font.Size = 10;
            ws.Cells["A14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            ws.Cells["B14"].Value = "Kế toán trưởng(Ký, họ tên)";
            ws.Cells["B14"].Style.Font.Size = 10;
            ws.Cells["B14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            ws.Cells["C14"].Value = "Người nộp tiền(Ký, họ tên)";
            ws.Cells["C14"].Style.Font.Size = 10;
            ws.Cells["C14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            ws.Cells["D14"].Value = "Người lập phiếu(Ký, họ tên)";
            ws.Cells["D14"].Style.Font.Size = 10;
            ws.Cells["D14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            ws.Cells["E14"].Value = "Thủ quỹ(Ký, họ tên)";
            ws.Cells["E14"].Style.Font.Size = 10;
            ws.Cells["E14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            char letter = 'a';

            //ws.Cells[letter + "11"].Value = "STT";
            //ws.Cells[letter + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            //letter = (char)(((int)letter) + 1);

            //ws.Cells[(char)(((int)letter)) + "11"].Value = "Mã ĐH";
            //ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            //letter = (char)(((int)letter) + 1);

            //ws.Cells[(char)(((int)letter)) + "11"].Value = "Ngày Tạo";
            //ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            //letter = (char)(((int)letter) + 1);
            //if (configCustomer == "true")
            //{
            //    ws.Cells[(char)(((int)letter)) + "11"].Value = "Tên Khách Hàng";
            //    ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            //    letter = (char)(((int)letter) + 1);
            //}


            

            ws.Cells[(char)(((int)letter)) + "11"].Value = "Dịch Vụ";
            ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            letter = (char)(((int)letter) + 1);
            if (configUnit == "true")
            {
                ws.Cells[(char)(((int)letter)) + "11"].Value = "Đơn vị";
                ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
                letter = (char)(((int)letter) + 1);
            }
            if (configDimension == "true")
            {
                char mergerLetter = (char)(((int)letter));
                ws.Cells[(char)(((int)mergerLetter)) + "11:" + (char)(((int)mergerLetter) + 1) + "11"].Merge = true;
                ws.Cells[(char)(((int)letter)) + "11"].Value = "Kích thước (m)";
                ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
                letter = (char)(((int)letter) + 2);
            }
            ws.Cells[(char)(((int)letter)) + "11"].Value = "Số lượng";
            ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            letter = (char)(((int)letter) + 1);
            if (configDimension == "true")
            {
                ws.Cells[(char)(((int)letter)) + "11"].Value = "Diện tích";
                ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
                letter = (char)(((int)letter) + 1);
            }
            ws.Cells[(char)(((int)letter)) + "11"].Value = "Đơn giá (vnd)";
            ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            letter = (char)(((int)letter) + 1);
            ws.Cells[(char)(((int)letter)) + "11"].Value = "Phí Vận Chuyển (vnd)";
            ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            letter = (char)(((int)letter) + 1);

            ws.Cells[(char)(((int)letter)) + "11"].Value = "Thành Tiền (vnd)";
            ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            char mergerTotalLetter = (char)(((int)letter) - 1);
            letter = (char)(((int)letter) + 1);

            ws.Cells[(char)(((int)letter)) + "11"].Value = "Ghi Chú";
            ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            letter = (char)(((int)letter) + 1);

            //ws.Cells[(char)(((int)letter)) + "11"].Value = "Chi Phí";
            //ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            //letter = (char)(((int)letter) + 1);

            //ws.Cells[(char)(((int)letter)) + "11"].Value = "Tiền Lãi";
            //ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            //letter = (char)(((int)letter) + 1);

            //ws.Cells[(char)(((int)letter)) + "11"].Value = "ThanhToán Tiền Mặt";
            //ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            //letter = (char)(((int)letter) + 1);

            //ws.Cells[(char)(((int)letter)) + "11"].Value = "Chuyển Khoản(vnd)";
            //ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);
            //letter = (char)(((int)letter) + 1);

            //ws.Cells[(char)(((int)letter)) + "11"].Value = "Còn Lại (vnd)";
            //ws.Cells[(char)(((int)letter)) + "11"].Style.Font.Color.SetColor(Color.RoyalBlue);

            foreach (var c in ws.Cells["A11:" + letter + "11"])
            {
                c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                c.Style.WrapText = true;
                c.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
            ws.Cells["A12:" + mergerTotalLetter + "12"].Merge = true;
            ws.Cells["A12:" + mergerTotalLetter + "12"].Value = "Tổng cộng";
            ws.Cells["A12:" + mergerTotalLetter + "12"].Style.Font.Bold = true;
            ws.Cells["A12:" + mergerTotalLetter + "12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            char totalLetter = (char)(((int)mergerTotalLetter) + 1);
            ws.Cells[totalLetter + "12"].Style.Numberformat.Format = "#,##0";
            ws.Cells[totalLetter + "12"].Value = result[0].Total;
            totalLetter++;

            ws.Cells[totalLetter + "12"].Style.Numberformat.Format = "#,##0";
            ws.Cells[totalLetter + "12"].Value = "";
            totalLetter++;



            //ws.Cells[totalLetter + "12"].Style.Numberformat.Format = "#,##0";
            //ws.Cells[totalLetter + "12"].Value = "";
            //totalLetter++;

            //ws.Cells[totalLetter + "12"].Style.Numberformat.Format = "#,##0";
            //ws.Cells[totalLetter + "12"].Value = "";
            //totalLetter++;

            //ws.Cells[totalLetter + "12"].Style.Numberformat.Format = "#,##0";
            //ws.Cells[totalLetter + "12"].Value = "";
            //totalLetter++;
            //ws.Cells[totalLetter + "12"].Style.Numberformat.Format = "#,##0";
            //ws.Cells[totalLetter + "12"].Value = "";
            foreach (var c in ws.Cells["A12:" + totalLetter + "12"])
            {
                c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                c.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                c.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            }
            var startRow = int.Parse(ws.Cells["A11"].Address.Substring(1)) + 1;
            var endRow = startRow;
            var numRows = result.Count;
            for (var i = 0; i < numRows; i++)
            {
                ws.InsertRow(endRow, 1);
                var column = 1;
                //ws.Cells[endRow, column].Value = i + 1;
                //column++;
                //ws.Cells[endRow, column].Value = result[i].OrderId;
                //column++;
                //ws.Cells[endRow, column].Value = result[i].CreatedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //column++;
                //if (configCustomer == "true")
                //{
                //    ws.Cells[endRow, column].Value = result[i].CustomerName;
                //    column++;
                //}

               
                ws.Cells[endRow, column].Value = result[i].CommodityName;
                column++;
                if (configUnit == "true")
                {
                    ws.Cells[endRow, column].Value = result[i].Unit;
                    column++;
                }
                if (configDimension == "true")
                {
                    ws.Cells[endRow, column].Value = result[i].Width == 0 ? null : result[i].Width;
                    column++;
                    ws.Cells[endRow, column].Value = result[i].Height == 0 ? null : result[i].Height;
                    column++;
                }

                ws.Cells[endRow, column].Value = result[i].Quantity;
                column++;
                if (configDimension == "true")
                {
                    ws.Cells[endRow, column].Value = result[i].Square == 0 ? null : result[i].SumSquare;
                    column++;
                }

                ws.Cells[endRow, column].Style.Numberformat.Format = "#,##0";
                ws.Cells[endRow, column].Value = result[i].Price;
                column++;
                ws.Cells[endRow, column].Style.Numberformat.Format = "#,##0";
                ws.Cells[endRow, column].Value = result[i].TransportFee;
                column++;
                ws.Cells[endRow, column].Style.Numberformat.Format = "#,##0";
                ws.Cells[endRow, column].Value = result[i].SubTotal;
                column++;
                ws.Cells[endRow, column].Value = result[i].FileName;
                column++;

                try
                {
                    if ((result[i].OrderId != result[i - 1].OrderId))
                    {
                        var tryColumn = column;

                        //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                        //ws.Cells[endRow, tryColumn].Value = result[i].Cost;
                        //tryColumn++;
                        //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                        //ws.Cells[endRow, tryColumn].Value = result[i].Total1 - result[i].Cost;
                        //tryColumn++;

                        //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                        //ws.Cells[endRow, tryColumn].Value = result[i].HasPay;
                        //tryColumn++;
                        //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                        //ws.Cells[endRow, tryColumn].Value = result[i].HasPayTransfer;
                        //tryColumn++;
                        //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                        //ws.Cells[endRow, tryColumn].Value = result[i].Total1 - (result[i].HasPay + result[i].HasPayTransfer);
                        //tryColumn++;
                    }
                    else
                    {
                        var tryColumn = column;

                        //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                        //ws.Cells[endRow, tryColumn].Value = "";
                        //tryColumn++;
                        //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                        //ws.Cells[endRow, tryColumn].Value = "";
                        //tryColumn++;

                        //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                        //ws.Cells[endRow, tryColumn].Value = "";
                        //tryColumn++;
                        //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                        //ws.Cells[endRow, tryColumn].Value = "";
                        //tryColumn++;
                        //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                        //ws.Cells[endRow, tryColumn].Value = "";
                        //tryColumn++;
                    }
                }
                catch (Exception)
                {
                    var tryColumn = column;

                    //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                    //ws.Cells[endRow, tryColumn].Value = result[i].Cost;
                    //tryColumn++;
                    //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                    //ws.Cells[endRow, tryColumn].Value = result[i].Total1 - result[i].Cost;
                    //tryColumn++;

                    //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                    //ws.Cells[endRow, tryColumn].Value = result[i].HasPay;
                    //tryColumn++;
                    //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                    //ws.Cells[endRow, tryColumn].Value = result[i].HasPayTransfer;
                    //tryColumn++;
                    //ws.Cells[endRow, tryColumn].Style.Numberformat.Format = "#,##0";
                    //ws.Cells[endRow, tryColumn].Value = result[i].Total1 - (result[i].HasPay + result[i].HasPayTransfer);
                    //tryColumn++;
                }
                if (i == numRows - 1)
                    continue;
                endRow++;
            }
            if (numRows != 0)
            {
                foreach (var c in ws.Cells["A" + startRow + ":" + letter + endRow])
                {
                    c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    c.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    c.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    c.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                    c.Style.WrapText = true;
                }
            }

            return package;
        }
        //public 
        public ExcelPackage Export(ExcelPackage package, int orderId)
        {
            var result = _bllOrder.GetOrderComplex(orderId);
            if (result == null)
            {
                return null;
            }
            var ws = package.Workbook.Worksheets.Add(result[0].CustomerName);

            ws.Cells.Style.Font.Size = 14;
            ws.Cells.Style.Font.Name = "Times New Roman";
            ws.Column(1).Width = 10;
            ws.Column(2).Width = 35;
            ws.Column(3).Width = 35;
            ws.Column(4).Width = 30;
            ws.Column(5).Width = 30;
            ws.Column(6).Width = 30;
            ws.Column(7).Width = 30;
            ws.Column(8).Width = 30;
            ws.Column(9).Width = 30;
            ws.Column(10).Width = 30;
            ws.Column(11).Width = 30;

            const string path = "~/Files/logo.png";
            var logo = Image.FromFile(Server.MapPath(path));
            ws.Row(0 * 5).Height = 39.00D;
            var picture = ws.Drawings.AddPicture(0.ToString(), logo);
            picture.From.Column = 0;
            picture.From.Row = 0;
            picture.To.Column = 0;
            picture.To.Row = 0;
            picture.SetSize(280, 104);

            ws.Cells["G1"].Value = "CÔNG TY TNHH PHÁT TRIỂN TRUYỀN THÔNG ADVISER";
            ws.Cells["G1"].Style.Font.Bold = true;
            ws.Cells["G1"].Style.Font.Size = 16;
            ws.Cells["G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["G2"].Value = "Inlichgo.com";
            ws.Cells["G2"].Style.Font.Bold = true;
            ws.Cells["G2"].Style.Font.Size = 14;
            ws.Cells["G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["G2"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["G3"].Value = "Địa chỉ văn phòng: 748/70/10 Thống Nhất, Phường 15, Quận Gò Vấp,TP Hồ Chí Minh";
            ws.Cells["G3"].Style.Font.Bold = true;
            ws.Cells["G3"].Style.Font.Size = 14;
            ws.Cells["G3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["G4"].Value = "Di động: 0901.333.151 - 0963.763.079 - 0168.565.5505";
            ws.Cells["G4"].Style.Font.Bold = true;
            ws.Cells["G4"].Style.Font.Size = 14;
            ws.Cells["G4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["G6"].Value = "BẢNG BÁO GIÁ";
            ws.Cells["G6"].Style.Font.Bold = true;
            ws.Cells["G6"].Style.Font.Size = 18;
            ws.Cells["G6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["A7"].Value = string.Format("Kính gửi:{0}", result[0].CustomerName);
            ws.Cells["A8"].Value = string.Format("Số điện thoại liên hệ:{0}", result[0].CustomerPhone);
            ws.Cells["A9"].Value = string.Format("Địa chỉ:{0}", result[0].CustomerAddress);
            ws.Cells["A10"].Value = "Bảng báo giá theo yêu cầu của quý công ty:";

            ws.Cells["A11"].Value = "STT";
            ws.Cells["A11"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["B11"].Value = "Hạng mục";
            ws.Cells["B11"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["C11"].Value = "Diễn giải";
            ws.Cells["C11"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["D11"].Value = "Đơn vị";
            ws.Cells["D11"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["E11:F11"].Merge = true;
            ws.Cells["E11"].Value = "Kích thước (m)";
            ws.Cells["E11"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["G11"].Value = "Số lượng";
            ws.Cells["G11"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["H11"].Value = "Diện tích";
            ws.Cells["H11"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["I11"].Value = "Đơn giá (vnd)";
            ws.Cells["I11"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["J11"].Value = "Thành Tiền (vnd)";
            ws.Cells["J11"].Style.Font.Color.SetColor(Color.RoyalBlue);

            foreach (var c in ws.Cells["A11:J11"])
            {
                c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                c.Style.WrapText = true;
                c.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
            ws.Cells["A12:I12"].Merge = true;
            ws.Cells["A12:I12"].Value = "Tổng cộng";
            ws.Cells["A12:I12"].Style.Font.Bold = true;
            ws.Cells["A12:I12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["D12"].Value = "";
            ws.Cells["E12"].Value = "";
            ws.Cells["F12"].Value = "";
            ws.Cells["G12"].Value = "";
            ws.Cells["H12"].Value = "";
            ws.Cells["I12"].Value = "";
            ws.Cells["J12"].Style.Numberformat.Format = "#,##0";
            ws.Cells["J12"].Value = result[0].Total;
            foreach (var c in ws.Cells["A12:J12"])
            {
                c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                c.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                c.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            }

            ws.Cells["A14"].Value = "Ghi chú: Khách hàng vui lòng đặt cọc 50% đơn hàng trước khi in.";
            ws.Cells["A14"].Style.Font.Bold = true;

            ws.Cells["A15"].Value = string.Format("* Giá trên chưa bao gồm thuế VAT 10%, mọi chi tiết xin liên hệ: {0} Ms {1}", result[0].CreateForUserMobile.Trim(), result[0].CreateForUserName.Trim());
            ws.Cells["A15"].Style.Font.Bold = true;

            ws.Cells["A16"].Value = "* Hy vọng sẽ đáp ứng yêu cầu của quý khách. Rất vui khi nhận được phản hồi quý khách.";

            ws.Cells["A17"].Value = "* Rất mong được hợp tác lâu dài cùng quý khách hàng .!.";
            ws.Cells["D19"].Value = string.Format("Ngày {0} tháng {1} năm {2}", DateTime.Now.AddHours(14).Day, DateTime.Now.AddHours(14).Month, DateTime.Now.AddHours(14).Year);


            var startRow = int.Parse(ws.Cells["A11"].Address.Substring(1)) + 1;
            var endRow = startRow;
            var numRows = result.Count;
            for (var i = 0; i < numRows; i++)
            {
                ws.InsertRow(endRow, 1);

                ws.Cells[endRow, 1].Value = i + 1;
                ws.Cells[endRow, 2].Value = result[i].FileName;
                ws.Cells[endRow, 3].Value = result[i].CommodityName;
                ws.Cells[endRow, 4].Value = result[i].Unit;
                ws.Cells[endRow, 5].Value = result[i].Width == 0 ? null : result[i].Width;
                ws.Cells[endRow, 6].Value = result[i].Height == 0 ? null : result[i].Height;
                ws.Cells[endRow, 7].Value = result[i].Quantity;
                ws.Cells[endRow, 8].Value = result[i].Square == 0 ? null : result[i].SumSquare;
                ws.Cells[endRow, 9].Style.Numberformat.Format = "#,##0";
                ws.Cells[endRow, 9].Value = result[i].Price;
                ws.Cells[endRow, 10].Style.Numberformat.Format = "#,##0";
                ws.Cells[endRow, 10].Value = result[i].SubTotal;
                if (i == numRows - 1)
                    continue;
                endRow++;
            }
            if (numRows != 0)
            {
                foreach (var c in ws.Cells["A" + startRow + ":J" + endRow])
                {
                    c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    c.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    c.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    c.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                    c.Style.WrapText = true;
                }

                //foreach (var c in ws.Cells["B" + startRow + ":B" + endRow])
                //{
                //    c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //}
                //foreach (var c in ws.Cells["G" + startRow + ":G" + endRow])
                //{
                //    c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //}
            }

            return package;
        }

        public JsonResult GetPriceForCustomerAndProduct(int customerId, int productId)
        {
            try
            {
                Thread.Sleep(200);

                var price = _bllOrder.GetPriceForCustomerAndProduct(customerId, productId);
                return Json(new { Result = "OK", Records = price });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        public JsonResult DesignUpdateOrderDetail(int Id, string FileName, string DesignDescription)
        {
            try
            {
                var responseResult = _bllOrder.DesignUpdateOrderDetail(Id, FileName, DesignDescription);
                if (responseResult.IsSuccess)
                    JsonDataResult.Result = "OK";
                else
                {
                    JsonDataResult.Result = "ERROR";
                    JsonDataResult.ErrorMessages.AddRange(responseResult.Errors);
                }
            }
            catch (Exception ex)
            {
                //add error
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Update", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }

    }
}
