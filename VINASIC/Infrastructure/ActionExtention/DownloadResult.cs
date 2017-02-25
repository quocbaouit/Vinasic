using System;
using System.IO;
using System.Web.Mvc;

namespace VINASIC.Infrastructure.ActionExtention
{
    public class DownloadResult : ActionResult
    {
        public DownloadResult()
        {
        }

        public DownloadResult(string filePath)
        {
            this.FilePath = filePath;
        }

        public string FilePath
        {
            get;
            set;
        }

        public string FileDownloadName
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (!String.IsNullOrEmpty(FileDownloadName))
            {
                context.HttpContext.Response.AddHeader("content-disposition",
                  "attachment; filename=" + this.FileDownloadName);
            }
            if (!this.FilePath.Contains(":"))
                this.FilePath = context.HttpContext.Server.MapPath(this.FilePath);

            string sContentType = string.Empty;
            switch (Path.GetExtension(this.FilePath).ToLower())
            {
                case ".dwf":
                    sContentType = "Application/x-dwf";
                    break;
                case ".pdf":
                    sContentType = "Application/pdf";
                    break;
                case ".docx":
                    sContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".ppt":
                case ".pps":
                    sContentType = "Application/vnd.ms-powerpoint";
                    break;
                case ".xls":
                    sContentType = "Application/vnd.ms-excel";
                    break;
                default:
                    sContentType = "Application/octet-stream";
                    break;
            }
            context.HttpContext.Response.ContentType = sContentType;
            context.HttpContext.Response.TransmitFile(this.FilePath);
        }
    }
}