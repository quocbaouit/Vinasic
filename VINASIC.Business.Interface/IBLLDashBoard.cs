using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBLLDashBoard
    { 
        ModelDashBoard GetData(string fromDate, string todate);
    }
}
