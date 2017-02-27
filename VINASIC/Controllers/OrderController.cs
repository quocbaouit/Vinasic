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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Infrastructure.ActionExtention;

namespace VINASIC.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IBllOrder _bllOrder;
        private readonly IBllProductType _bllProductType;
        private readonly IBllProduct _bllProduct;
        private readonly IBllEmployee _bllEmployee;
        private readonly IBllCustomer _bllCustomer;

        public OrderController(IBllOrder bllOrder, IBllEmployee bllEmployee, IBllCustomer bllCustomer, IBllProductType bllProductType, IBllProduct bllProduct)
        {
            _bllOrder = bllOrder;
            _bllEmployee = bllEmployee;
            _bllCustomer = bllCustomer;
            _bllProductType = bllProductType;
            _bllProduct = bllProduct;
        }
        public ActionResult Index()
        {
            var employee = _bllEmployee.GetUserById(UserContext.UserID);
            ViewBag.Employee = employee;
            return View();
        }

        public ActionResult OrderReport()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult GetOrders(int jtStartIndex = 0, int jtPageSize = 10, string jtSorting = "", string keyword = "", int employee = 0, string fromDate = "", string toDate = "", int delivery = 0, int paymentStatus = 0)
        {
            try
            {
                if (employee == 0 && !UserContext.Permissions.Contains("/Order/GetCustomerByOrganization"))
                {
                    employee = UserContext.UserID;
                }
                var listOrder = _bllOrder.GetList(UserContext.UserID, jtStartIndex, jtPageSize, jtSorting, fromDate, toDate, employee, keyword, delivery, paymentStatus);

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
        public JsonResult GetListViewDetail(int jtStartIndex = 0, int jtPageSize = 10, string jtSorting = "", string keyword = "", int employee = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                if (employee == 0 && !UserContext.Permissions.Contains("/Order/GetCustomerByOrganization"))
                {
                    employee = UserContext.UserID;
                }
                var listOrder = _bllOrder.GetListViewDetail(keyword, jtStartIndex, jtPageSize, jtSorting, fromDate, toDate, employee);
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
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllOrder.DeleteById(id, UserContext.UserID);
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
        public JsonResult UpdatePayment(int orderId, string payment, int paymentType)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var pay = float.Parse(payment);
                    var responseResult = _bllOrder.UpdatePayment(orderId, pay, paymentType, UserId);
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
        public JsonResult SaveOrder(int orderId, int employeeId, int customerId, string customerName, string customerPhone, string customerMail, string customerAddress, string customerTaxCode, string dateDelivery, float orderTotal, List<ModelDetail> listDetail)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var saveOrder = new ModelSaveOrder
                    {
                        OrderId = orderId,
                        EmployeeId = employeeId,
                        OrderTotal = orderTotal,
                        CustomerId = customerId,
                        CustomerName = customerName,
                        CustomerPhone = customerPhone,
                        CustomerMail = customerMail,
                        CustomerAddress = customerAddress,
                        CustomerTaxCode = customerTaxCode,
                        DateDelivery = DateTime.Parse(dateDelivery ?? DateTime.Now.AddHours(14).ToString(CultureInfo.InvariantCulture)),
                        Detail = listDetail
                    };
                    var responseResult = saveOrder.OrderId == 0 ? _bllOrder.CreateOrder(saveOrder, UserContext.UserID) : _bllOrder.UpdatedOrder(saveOrder, UserContext.UserID);
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
                List<SelectListItem> listValues = new List<SelectListItem>();
                var listProductType = _bllProduct.GetListProduct(productType);
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
                var listProductType = _bllEmployee.GetCustomerByOrganization(shortName,IsAuthenticate,UserContext.UserID);
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


        public ActionResult ExportReport([FromUri] DateTime fromDate, [FromUri]DateTime toDate, [FromUri]int employee, [FromUri]string keySearch, int delivery = 0, int paymentStatus = 0)
        {
            var pck = new ExcelPackage();
            pck = ExportSum(pck, fromDate, toDate, employee, keySearch, delivery, paymentStatus);
            return new ExcelDownload(pck, string.Format("{0}_.xlsx", DateTime.Now.AddHours(14).ToString("d")));
        }
        public ActionResult ExportExcelQuotation(int orderId, string orderName)
        {
            var pck = new ExcelPackage();
            pck = Export(pck, orderId);
            return new ExcelDownload(pck, string.Format("{0}_{1}.xlsx", orderName, DateTime.Now.AddHours(14).ToString("d")));
        }
        //public 
        public ExcelPackage ExportSum(ExcelPackage package, DateTime fromDate, DateTime toDate, int employee, string keySearch, int delivery, int paymentStatus)
        {
            var result = _bllOrder.ExportReport(fromDate, toDate, employee, keySearch, delivery, paymentStatus);
            if (result == null)
            {
                return null;
            }
            var ws = package.Workbook.Worksheets.Add("Thống Kê");

            ws.Cells.Style.Font.Size = 14;
            ws.Cells.Style.Font.Name = "Times New Roman";
            ws.Column(1).Width = 7;
            ws.Column(2).Width = 7;
            ws.Column(3).Width = 15;
            ws.Column(4).Width = 30;
            ws.Column(5).Width = 30;
            ws.Column(6).Width = 30;
            ws.Column(7).Width = 8;
            ws.Column(8).Width = 8;
            ws.Column(9).Width = 8;
            ws.Column(10).Width = 14;
            ws.Column(11).Width = 14;
            ws.Column(12).Width = 14;
            ws.Column(13).Width = 14;
            ws.Column(14).Width = 14;
            ws.Column(15).Width = 14;
            ws.Column(16).Width = 14;
            const string path = "~/Files/logovinasic.png";
            var logo = Image.FromFile(Server.MapPath(path));
            ws.Row(0 * 5).Height = 39.00D;
            var picture = ws.Drawings.AddPicture(0.ToString(), logo);
            picture.From.Column = 0;
            picture.From.Row = 0;
            picture.To.Column = 0;
            picture.To.Row = 0;
            picture.SetSize(280, 104);

            ws.Cells["G1"].Value = "CÔNG TY TNHH DỊCH VỤ VĂN HÓA THÔNG TIN VIỆT NAM";
            ws.Cells["G1"].Style.Font.Bold = true;
            ws.Cells["G1"].Style.Font.Size = 16;
            ws.Cells["G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["G2"].Value = "Vinasic.com - Thicong24h.com - Inppgiare.info";
            ws.Cells["G2"].Style.Font.Bold = true;
            ws.Cells["G2"].Style.Font.Size = 14;
            ws.Cells["G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["G2"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["G3"].Value = "Địa chỉ văn phòng: 118/15 Bàu Cát 2, Phường 12, Quận Tân Bình, Tp.HCM";
            ws.Cells["G3"].Style.Font.Bold = true;
            ws.Cells["G3"].Style.Font.Size = 14;
            ws.Cells["G3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["G4"].Value = "Di động: 091.346.1539 - 090.269.7586 - 090.932.1586";
            ws.Cells["G4"].Style.Font.Bold = true;
            ws.Cells["G4"].Style.Font.Size = 14;
            ws.Cells["G4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["G6"].Value = "THỐNG KÊ HÀNG IN";
            ws.Cells["G6"].Style.Font.Bold = true;
            ws.Cells["G6"].Style.Font.Size = 18;
            ws.Cells["G6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            ws.Cells["A8"].Value = "STT";
            ws.Cells["A8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["B8"].Value = "Mã ĐH";
            ws.Cells["B8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["C8"].Value = "Ngày Tạo";
            ws.Cells["C8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["D8"].Value = "Tên Khách Hàng";
            ws.Cells["D8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["E8"].Value = "Hạng mục";
            ws.Cells["E8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["F8"].Value = "Diễn giải";
            ws.Cells["F8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["G8"].Value = "Đơn vị";
            ws.Cells["G8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["H8:I8"].Merge = true;
            ws.Cells["H8"].Value = "Kích thước (m)";
            ws.Cells["H8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["J8"].Value = "Số lượng";
            ws.Cells["J8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["K8"].Value = "Diện tích";
            ws.Cells["K8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["L8"].Value = "Đơn giá (vnd)";
            ws.Cells["L8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["M8"].Value = "Thành Tiền (vnd)";
            ws.Cells["M8"].Style.Font.Color.SetColor(Color.RoyalBlue);


            ws.Cells["N8"].Value = "Tổng Tiền Hàng (vnd)";
            ws.Cells["N8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["O8"].Value = "ThanhToán(vnd)";
            ws.Cells["O8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["P8"].Value = "Còn Lại (vnd)";
            ws.Cells["P8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            foreach (var c in ws.Cells["A8:P8"])
            {
                c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                c.Style.WrapText = true;
                c.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
            ws.Cells["A9:L9"].Merge = true;
            ws.Cells["A9:L9"].Value = "Tổng cộng";
            ws.Cells["A9:L9"].Style.Font.Bold = true;
            ws.Cells["A9:L9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["D9"].Value = "";
            ws.Cells["E9"].Value = "";
            ws.Cells["F9"].Value = "";
            ws.Cells["G9"].Value = "";
            ws.Cells["H9"].Value = "";
            ws.Cells["I9"].Value = "";
            ws.Cells["J9"].Value = "";
            ws.Cells["K9"].Value = "";
            ws.Cells["L9"].Value = "";
            ws.Cells["M9"].Style.Numberformat.Format = "#,##0";
            ws.Cells["M9"].Value = result[0].Total;
            ws.Cells["N9"].Style.Numberformat.Format = "#,##0";
            ws.Cells["N9"].Value = "";//result.Sum(x => x.Total1);
            ws.Cells["O9"].Style.Numberformat.Format = "#,##0";
            ws.Cells["O9"].Value = "";//result.Sum(x=>x.HasPay);
            ws.Cells["P9"].Style.Numberformat.Format = "#,##0";
            ws.Cells["P9"].Value = "";//result.Sum(x=>x.HasExist);
            foreach (var c in ws.Cells["A9:P9"])
            {
                c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                c.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                c.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            }
            var startRow = int.Parse(ws.Cells["A8"].Address.Substring(1)) + 1;
            var endRow = startRow;
            var numRows = result.Count;
            for (var i = 0; i < numRows; i++)
            {
                ws.InsertRow(endRow, 1);

                ws.Cells[endRow, 1].Value = i + 1;
                ws.Cells[endRow, 2].Value = result[i].OrderId;
                ws.Cells[endRow, 3].Value = result[i].CreatedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                ws.Cells[endRow, 4].Value = result[i].CustomerName;
                ws.Cells[endRow, 5].Value = result[i].FileName;
                ws.Cells[endRow, 6].Value = result[i].CommodityName;
                ws.Cells[endRow, 7].Value = result[i].Unit;
                ws.Cells[endRow, 8].Value = result[i].Width == 0 ? null : result[i].Width;
                ws.Cells[endRow, 9].Value = result[i].Height == 0 ? null : result[i].Height;
                ws.Cells[endRow, 10].Value = result[i].Quantity;
                ws.Cells[endRow, 11].Value = result[i].Square == 0 ? null : result[i].SumSquare;
                ws.Cells[endRow, 12].Style.Numberformat.Format = "#,##0";
                ws.Cells[endRow, 12].Value = result[i].Price;
                ws.Cells[endRow, 13].Style.Numberformat.Format = "#,##0";
                ws.Cells[endRow, 13].Value = result[i].SubTotal;

                try
                {
                    if ((result[i].OrderId != result[i - 1].OrderId))
                    {
                        ws.Cells[endRow, 13].Style.Numberformat.Format = "#,##0";
                        ws.Cells[endRow, 13].Value = result[i].Total1;
                        ws.Cells[endRow, 15].Style.Numberformat.Format = "#,##0";
                        ws.Cells[endRow, 15].Value = result[i].HasPay;
                        ws.Cells[endRow, 16].Style.Numberformat.Format = "#,##0";
                        ws.Cells[endRow, 16].Value = result[i].HasExist;
                    }
                    else
                    {
                        ws.Cells[endRow, 14].Style.Numberformat.Format = "#,##0";
                        ws.Cells[endRow, 14].Value = "";
                        ws.Cells[endRow, 15].Style.Numberformat.Format = "#,##0";
                        ws.Cells[endRow, 15].Value = "";
                        ws.Cells[endRow, 16].Style.Numberformat.Format = "#,##0";
                        ws.Cells[endRow, 16].Value = "";
                    }
                }
                catch (Exception)
                {
                    ws.Cells[endRow, 14].Style.Numberformat.Format = "#,##0";
                    ws.Cells[endRow, 14].Value = result[i].Total1;
                    ws.Cells[endRow, 15].Style.Numberformat.Format = "#,##0";
                    ws.Cells[endRow, 15].Value = result[i].HasPay;
                    ws.Cells[endRow, 16].Style.Numberformat.Format = "#,##0";
                    ws.Cells[endRow, 16].Value = result[i].HasExist;
                }
                if (i == numRows - 1)
                    continue;
                endRow++;
            }
            if (numRows != 0)
            {
                foreach (var c in ws.Cells["A" + startRow + ":P" + endRow])
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
            ws.Column(4).Width = 10;
            ws.Column(5).Width = 10;
            ws.Column(6).Width = 10;
            ws.Column(7).Width = 20;
            ws.Column(8).Width = 20;
            ws.Column(9).Width = 30;
            ws.Column(10).Width = 30;
            ws.Column(11).Width = 30;

            const string path = "~/Files/logovinasic.png";
            var logo = Image.FromFile(Server.MapPath(path));
            ws.Row(0 * 5).Height = 39.00D;
            var picture = ws.Drawings.AddPicture(0.ToString(), logo);
            picture.From.Column = 0;
            picture.From.Row = 0;
            picture.To.Column = 0;
            picture.To.Row = 0;
            picture.SetSize(280, 104);

            ws.Cells["G1"].Value = "CÔNG TY TNHH DỊCH VỤ VĂN HÓA THÔNG TIN VIỆT NAM";
            ws.Cells["G1"].Style.Font.Bold = true;
            ws.Cells["G1"].Style.Font.Size = 16;
            ws.Cells["G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["G2"].Value = "Vinasic.com - Thicong24h.com - Inppgiare.info";
            ws.Cells["G2"].Style.Font.Bold = true;
            ws.Cells["G2"].Style.Font.Size = 14;
            ws.Cells["G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["G2"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["G3"].Value = "Địa chỉ văn phòng: 118/15 Bàu Cát 2, Phường 12, Quận Tân Bình, Tp.HCM";
            ws.Cells["G3"].Style.Font.Bold = true;
            ws.Cells["G3"].Style.Font.Size = 14;
            ws.Cells["G3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["G4"].Value = "Di động: 091.346.1539 - 090.269.7586 - 090.932.1586";
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

    }
}
