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
        ResponseBase SaveUserSalery(List<SalaryObj> SalaryObj, int employId, int userID);
        ResponseBase DeleteByListId(List<int> listId, int userId);
        PagedList<ModelUser> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListEmployee();
        List<ModelSelectItem> GetCustomerByOrganization(string shortName, bool isAuthor, int userId);
        PagedList<ModelForDesign> GetListForDesign(string keyWord, int startIndexRecord, int pageSize, string sorting,int userid, string fromDate, string toDate,bool auth,int emp);
        PagedList<ModelForPrint> GetListForPrint(string keyWord, int startIndexRecord, int pageSize, string sorting, int userid, string fromDate, string toDate,bool auth,int emp);
        ResponseBase DesignUpdateOrderDeatail(int id, int stautus, int userId, string employeeName);
        ResponseBase PrintUpdateOrderDeatail(int id, int stautus, int userId,string employeeName);
        ResponseBase BusinessUpdateOrderDeatail(int id, int employeeId, string description, int type, int stautus,int userId);
        T_User GetUserById(int id);

        PagedList<UserProduct> ListProductIdByUser(int userId, string keyWord, int startIndexRecord, int pageSize,
            string sorting);
        ResponseBase ResetPass(int empId);
        List<int> GetProductByUserId(int userId);
        ResponseBase UpdateUserProduct(int userId, List<int> products);
        ResponseBase UpdateLock(int userId, bool isLock, int contextUser);
        ModelSimpleEmployee GetSimpleEmployee();
         PagedList<ModelForDesign> GetListDetailForBusiness(string keyWord, int startIndexRecord, int pageSize, string sorting, int userId, string fromDate, string toDate, bool auth, int emp);
    }
}
