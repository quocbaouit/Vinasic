using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
using VINASIC.Object;

namespace VINASIC.Business.Interface
{
   public interface IBLLErrorLog
    {
       ResponseBase AddErrorLog(ModelErrorLog errorLog );
       List<T_ErrorLog> GetListErrorLogNotFix();
       bool UpdateErrorLogState(ModelErrorLog errorLog);
    }
}
