using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using GPRO.Ultilities;

namespace VINASIC.Infrastructure.ActionExtention
{
    public class ExcelDownloadResult : ActionResult
    {
        public ExcelDownloadResult()
        {
        }
        public ExcelDownloadResult(DataSet ds, string filename, string filePath)
        {
            this._dataSet = ds;
            this._fileName = filename;
            this._filePath = filePath;
        }
        public DataSet _dataSet
        {
            get;
            set;
        }
        public string _fileName
        {
            get;
            set;
        }
        public string _filePath
        {
            get;
            set;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            DirectoryInfo info = new DirectoryInfo(this._filePath);
            if (!info.Exists)
            {
                info.Create();
            }
            string file = this._filePath + this._fileName;
            // lay thong tin file mau => chua co du lieu => copy ra tao moi du lieu
            string fileTemp = "~/Files/ExcelTemplate/Book1.xls";
            if (File.Exists(fileTemp))
            {
                WriteExcel.CreateFile(fileTemp);
            }
            File.Copy(HttpContext.Current.Server.MapPath(fileTemp), file, true);
            WriteExcel.Export(this._dataSet, file);
            if (file.Contains("Sheet"))
            {
                WriteExcel.DeleteSheetContainsName(file, "Sheet");
            }
            //WriteExcel.DeleteSheetContainsName(file, "Sheet");// Xoa truong hop sheet con ton tai k file downlad ve
            context.HttpContext.Response.AddHeader("content-disposition",
                  "attachment; filename=" + this._fileName);
            context.HttpContext.Response.ContentType = "Application/vnd.ms-excel";
            context.HttpContext.Response.TransmitFile(file);
        }
    }
}