using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VINASIC.Object;
using VINASIC.Data.Repositories;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Data;
using System.Diagnostics;
using GPRO.Ultilities;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;


namespace SystemAccount.Bussiness
{
   public class BLLErrorLog : IBLLErrorLog
    {
       private readonly IT_ErrorLogRepository repErrorLog;
       private readonly IUnitOfWork<VINASICEntities> unitOfWork;

       public BLLErrorLog(IT_ErrorLogRepository _repErrorLog, IUnitOfWork<VINASICEntities> _unitOfWork)
       {
           this.unitOfWork = _unitOfWork;
           this.repErrorLog = _repErrorLog;
       }
       private void SaveChange()
       {
           try
           {
               this.unitOfWork.Commit();
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

        public ResponseBase AddErrorLog(ModelErrorLog errorLogModel)
        { 
            var result = new ResponseBase();
            try
            {                
                var errorLog = new T_ErrorLog();
                Parse.CopyObject(errorLogModel, ref errorLog);
                repErrorLog.Add(errorLog);
                SaveChange();
                result.IsSuccess = true;
                result.Data = errorLog.Id;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Data = ex;
                return result; 
            }
        }

        public List<T_ErrorLog> GetListErrorLogNotFix()
        {
            throw new NotImplementedException();
        }

        public bool UpdateErrorLogState(ModelErrorLog errorLog)
        {
            throw new NotImplementedException();
        }
    }
}
