using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
using VINASIC.Object;

namespace VINASIC.Business.Interface
{
    public interface IBllCustomer
    {
        ResponseBase Create(ModelCustomer obj);
        ResponseBase Update(ModelCustomer obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelCustomer> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListCustomer();
        List<ModelCustomer> GetListProduct();
        List<string> GetAllCustomerName();
        List<string> GetSimpleCustomer();
        T_Customer GetCustomerById(int id);
        T_Customer GetCustomerByName(string name);
        T_Customer GetCustomerByPhone(string phone);
    }
}
