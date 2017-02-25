using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Business.Interface
{
    public interface IBllStockIn
    {
        PagedList<ModelStockIn> GetList(int listEmployee, int startIndexRecord, int pageSize, string sorting);
        List<ModelStockInDetail> GetListStockInDetailByStockInId(int orderId);
        ResponseBase UpdatedStockIn(ModelSaveStockIn obj, int userId);
        ResponseBase CreateStockIn(ModelSaveStockIn obj, int userId);
        List<ModelViewStockDetail> ExportReport(DateTime fromDate, DateTime toDate, string keyWord);
        ResponseBase DeleteById(int id, int userId);
    }
}
