using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
using VINASIC.Object;

namespace VINASIC.Business.Interface
{
    public interface IBllPartner
    {
        ResponseBase Create(ModelPartner obj);
        ResponseBase Update(ModelPartner obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelPartner> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListPartner();
        List<ModelPartner> GetListProduct();
        T_Partner GetPartnerById(int id);
    }
}