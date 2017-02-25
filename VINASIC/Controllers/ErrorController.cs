using System;
using System.Linq;
using System.Web.Mvc;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Enum;
using VINASIC.Business.Interface.Model;
using VINASIC.Data.Repositories;

namespace VINASIC.Controllers
{
    public class ErrorController : BaseController
    {
        private readonly IT_ErrorLogRepository repErrorLog;
        private readonly IBLLErrorLog bllErrorLog;
        public ErrorController(IT_ErrorLogRepository _repErrorLog, IBLLErrorLog _bllErrorLog)
        {
            this.repErrorLog = _repErrorLog;
            this.bllErrorLog = _bllErrorLog;
        }

        public ActionResult Index(int ErrorType, int? ErrorId)
        {
            try
            {
                var errorLog = new ModelErrorLog();
                switch (ErrorType)
                {
                    case 0:
                        errorLog = repErrorLog.GetMany(x => !x.IsFix && x.Id == ErrorId).Select(
                            x => new ModelErrorLog()
                            {
                                
                                Id = x.Id,
                                CompanyId = x.CompanyId,
                                ModuleName = x.ModuleName,
                                IsFix = x.IsFix,
                                ErrorCaption = x.ErrorCaption,
                                ErrorClass = x.ErrorClass,
                                ErrorMethod = x.ErrorMethod,
                                StrackTrace = x.StrackTrace,
                                IpError = x.IpError,
                                ActionError = x.ActionError,
                                TargetSite = x.TargetSite
                            }).FirstOrDefault();
                        errorLog.IsDeveloper = UserContext.Permissions.Contains(eErrorPermissionType.Dev) ? true : false;
                        break;
                    case 1:
                        errorLog.Id = 0;
                        errorLog.ErrorCaption = eErrorMessage.NoPermission;
                        break;
                    case 2:
                        errorLog.Id = 0;
                        errorLog.ErrorCaption = eErrorMessage.Error404;
                        break;
                }
                return View(errorLog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
