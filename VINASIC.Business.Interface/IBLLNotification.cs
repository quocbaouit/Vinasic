using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using VINASIC.Business.Interface.Model;
using VINASIC.Object;

namespace VINASIC.Business.Interface
{
    public interface IBllNotification
    {
        ResponseBase Create(T_Notification obj);
        ResponseBase Update(T_Notification obj);
        ResponseBase DeleteById(int id, int userId);
        ResponseBase DeleteByListId(List<int> listId, int userId);
        PagedList<T_Notification> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelNotification> GetListNotification();
         void UpdateNotification(string userId);
    }
}
