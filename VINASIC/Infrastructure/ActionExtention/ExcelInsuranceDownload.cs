using System.Web.Mvc;
using OfficeOpenXml;

namespace VINASIC.Infrastructure.ActionExtention
{
    public class ExcelDownload : ActionResult
    {
        public ExcelDownload()
        {
        }
        public ExcelDownload(ExcelPackage pck, string filename)
        {
            _pck = pck;
            _fileName = filename;
        }
        private ExcelPackage _pck
        {
            get;
            set;
        }
        private string _fileName
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.BinaryWrite(_pck.GetAsByteArray());
            context.HttpContext.Response.AddHeader("content-disposition", "attachment;  filename=" + _fileName);
            context.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }
    }
}