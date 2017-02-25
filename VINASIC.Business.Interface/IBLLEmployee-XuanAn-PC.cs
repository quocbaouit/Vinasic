using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
using VINASIC.Object;

namespace VINASIC.Business.Interface
{
    public interface IBllEmployee
    {
        ResponseBase Create(ModelUser obj);
        ResponseBase Update(ModelUser obj);
        ResponseBase DeleteById(int id, int userId);
        ResponseBase DeleteByListId(List<int> listId, int userId);
        PagedList<ModelUser> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListEmployee();
        List<ModelSelectItem> GetCustomerByOrganization(string shortName, bool isAuthor, int userId);
        PagedList<ModelForDesign> GetListForDesign(string keyWord, int startIndexRecord, int pageSize, string sorting,int userid, string fromDate, string toDate);
        PagedList<ModelForPrint> GetListForPrint(string keyWord, int startIndexRecord, int pageSize, string sorting, int userid, string fromDate, string toDate);
        ResponseBase DesignUpdateOrderDeatail(int id, int stautus, int userId);
        ResponseBase PrintUpdateOrderDeatail(int id, int stautus, int userId);
        ResponseBase BusinessUpdateOrderDeatail(int id, int employeeId, string description, int type, int stautus,int userId);
        T_User GetUserById(int id);

        ResponseBase ResetPass(int empId);
    }
}
