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
    public class StockInController : BaseController
    {
        private readonly IBllStockIn _bllStockIn;
        private readonly IBllMaterial _material;
        private readonly IBllMaterialType _materialType;
        private readonly IBllPartner _bllPartner;

        public StockInController(IBllStockIn bllStockIn, IBllPartner bllPartnerr, IBllMaterialType materialType, IBllMaterial material)
        {
            _bllStockIn = bllStockIn;
            _bllPartner = bllPartnerr;
            _materialType = materialType;
            _material = material;
        }
        public ActionResult Index()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult GetStockIns(int jtStartIndex = 0, int jtPageSize = 10, string jtSorting = "")
        {
            try
            {

                var listStockIn = _bllStockIn.GetList(UserContext.IsOwner ? UserContext.UserID : 1, jtStartIndex, jtPageSize, jtSorting);
                JsonDataResult.Records = listStockIn;
                JsonDataResult.Result = "OK";
                JsonDataResult.TotalRecordCount = listStockIn.TotalItemCount;

            }
            catch (Exception ex)
            {
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Get List ObjectType", Message = "Lỗi: " + ex.Message });

            }
            return Json(JsonDataResult);
        }
        public JsonResult ListStockInDetail(int stockInId)
        {
            try
            {
                Thread.Sleep(200);
                var stockInDetail = _bllStockIn.GetListStockInDetailByStockInId(stockInId);
                return Json(new { Result = "OK", Records = stockInDetail });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult SaveStockIn(int stockInId, string description, int customerId, string customerName, string dateDelivery, float orderTotal, List<ModelStockDetail> listDetail)
        {
            try
            {
                var saveStockIn = new ModelSaveStockIn
                {
                    Description = description,
                    StockInId = stockInId,
                    OrderTotal = orderTotal,
                    PartnerId = customerId,
                    CustomerName = customerName,
                    DateDelivery = DateTime.Parse(dateDelivery ?? DateTime.Now.AddHours(14).ToString(CultureInfo.InvariantCulture)),
                    Detail = listDetail
                };
                if (IsAuthenticate)
                {
                    var responseResult = saveStockIn.StockInId == 0 ? _bllStockIn.CreateStockIn(saveStockIn, UserContext.UserID) : _bllStockIn.UpdatedStockIn(saveStockIn, UserContext.UserID);
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
                var customers = _bllPartner.GetListPartner();
                return Json(new { Result = "OK", Records = customers });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetListPartner()
        {
            try
            {
                List<SelectListItem> listValues = new List<SelectListItem>();
                var listProductType = _bllPartner.GetListPartner();
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

        [System.Web.Mvc.HttpPost]
        public JsonResult DeleteStockIn(int id)
        {
            try
            {
                if (IsAuthenticate)
                {
                    var responseResult = _bllStockIn.DeleteById(id, UserContext.UserID);
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
                JsonDataResult.Result = "ERROR";
                JsonDataResult.ErrorMessages.Add(new Error() { MemberName = "Delete BankBranch", Message = "Lỗi: " + ex.Message });
            }
            return Json(JsonDataResult);
        }
        public JsonResult GetListProductType()
        {
            try
            {
                List<SelectListItem> listValues = new List<SelectListItem>();
                var listProductType = _materialType.GetListMaterialType();
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
                var listProductType = _material.GetListMaterial(productType);
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
        public JsonResult GetPartnerById(int partId)
        {
            try
            {
                Thread.Sleep(200);
                var customer = _bllPartner.GetPartnerById(partId);
                return Json(new { Result = "OK", Records = customer });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult ExportReport([FromUri] DateTime fromDate, [FromUri]DateTime toDate, [FromUri]string keySearch)
        {
            var pck = new ExcelPackage();
            pck = ExportSum(pck, fromDate, toDate, keySearch);
            return new ExcelDownload(pck, string.Format("{0}_.xlsx", DateTime.Now.AddHours(14).ToString("d")));
        }
        public ExcelPackage ExportSum(ExcelPackage package, DateTime fromDate, DateTime toDate, string keySearch)
        {
            var result = _bllStockIn.ExportReport(fromDate, toDate, keySearch);
            if (result == null)
            {
                return null;
            }
            var ws = package.Workbook.Worksheets.Add("Thống Kê");

            ws.Cells.Style.Font.Size = 14;
            ws.Cells.Style.Font.Name = "Times New Roman";
            ws.Column(1).Width = 10;
            ws.Column(2).Width = 20;
            ws.Column(3).Width = 60;
            ws.Column(4).Width = 25;
            ws.Column(5).Width = 25;
            ws.Column(6).Width = 25;
            ws.Column(7).Width = 25;
            ws.Column(8).Width = 25;

            const string path = "~/Files/logovinasic.png";
            var logo = Image.FromFile(Server.MapPath(path));
            ws.Row(0 * 5).Height = 39.00D;
            var picture = ws.Drawings.AddPicture(0.ToString(), logo);
            picture.From.Column = 0;
            picture.From.Row = 0;
            picture.To.Column = 0;
            picture.To.Row = 0;
            picture.SetSize(280, 104);

            ws.Cells["D1"].Value = "CÔNG TY TNHH DỊCH VỤ VĂN HÓA THÔNG TIN VIỆT NAM";
            ws.Cells["D1"].Style.Font.Bold = true;
            ws.Cells["D1"].Style.Font.Size = 16;
            ws.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["D2"].Value = "Vinasic.com - Thicong24h.com - Inppgiare.info";
            ws.Cells["D2"].Style.Font.Bold = true;
            ws.Cells["D2"].Style.Font.Size = 14;
            ws.Cells["D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["D2"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["D3"].Value = "Địa chỉ văn phòng: 118/15 Bàu Cát 2, Phường 12, Quận Tân Bình, Tp.HCM";
            ws.Cells["D3"].Style.Font.Bold = true;
            ws.Cells["D3"].Style.Font.Size = 14;
            ws.Cells["D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["D4"].Value = "Di động: 091.346.1539 - 090.269.7586 - 090.932.1586";
            ws.Cells["D4"].Style.Font.Bold = true;
            ws.Cells["D4"].Style.Font.Size = 14;
            ws.Cells["D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["D6"].Value = "THỐNG KÊ NHẬP HÀNG";
            ws.Cells["D6"].Style.Font.Bold = true;
            ws.Cells["D6"].Style.Font.Size = 18;
            ws.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            ws.Cells["A8"].Value = "STT";
            ws.Cells["A8"].Style.Font.Color.SetColor(Color.RoyalBlue);


            ws.Cells["B8"].Value = "Ngày Tạo";
            ws.Cells["B8"].Style.Font.Color.SetColor(Color.RoyalBlue);
            ws.Cells["C8"].Value = "Đối Tác";
            ws.Cells["C8"].Style.Font.Color.SetColor(Color.RoyalBlue);
            ws.Cells["D8"].Value = "Vật Tư";
            ws.Cells["D8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["E8"].Value = "Mô Tả";
            ws.Cells["E8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["F8"].Value = "Số lượng";
            ws.Cells["F8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["G8"].Value = "Đơn giá (vnd)";
            ws.Cells["G8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            ws.Cells["H8"].Value = "Thành Tiền (vnd)";
            ws.Cells["H8"].Style.Font.Color.SetColor(Color.RoyalBlue);

            foreach (var c in ws.Cells["A8:O8"])
            {
                c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                c.Style.WrapText = true;
                c.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
            ws.Cells["A9:G9"].Merge = true;
            ws.Cells["A9:G9"].Value = "Tổng cộng";
            ws.Cells["A9:G9"].Style.Font.Bold = true;
            ws.Cells["A9:G9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["D9"].Value = "";
            ws.Cells["E9"].Value = "";
            ws.Cells["F9"].Value = "";
            ws.Cells["G9"].Value = "";
            ws.Cells["H9"].Style.Numberformat.Format = "#,##0";
            ws.Cells["H9"].Value = result.Sum(x => x.SubTotal);
            foreach (var c in ws.Cells["A9:H9"])
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
                ws.Cells[endRow, 2].Value = result[i].CreatedDate.ToString("d");
                ws.Cells[endRow, 3].Value = result[i].Name;
                ws.Cells[endRow, 4].Value = result[i].MateriaName;
                ws.Cells[endRow, 5].Value = result[i].Description;
                ws.Cells[endRow, 6].Value = result[i].Quantity;
                ws.Cells[endRow, 7].Style.Numberformat.Format = "#,##0";
                ws.Cells[endRow, 7].Value = result[i].Price;
                ws.Cells[endRow, 8].Style.Numberformat.Format = "#,##0";
                ws.Cells[endRow, 8].Value = result[i].SubTotal;
                if (i == numRows - 1)
                    continue;
                endRow++;
            }
            if (numRows != 0)
            {
                foreach (var c in ws.Cells["A" + startRow + ":H" + endRow])
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
