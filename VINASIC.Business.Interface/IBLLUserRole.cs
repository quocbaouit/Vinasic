using System.Collections.Generic;
using PagedList;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Business.Interface
{
    public interface IBLLUserRole
    {
        ResponseBase Create(ModelUserRole obj);
        ResponseBase Update(ModelUserRole obj);
        ResponseBase DeleteById(int id, int userId);       
        List<ModelSelectItem> GetUserRolesModelByUserId(int userId , bool IsOwner, int companyId);
        PagedList<ModelUserRole> GetList(int counTryId, int startIndexRecord, int pageSize, string sorting);

        ResponseBase UpdateUserRole(int userId, List<int> roleIds);
        // service
        List<int> GetUserRolesIdByUserId(int userId);
        ResponseBase AddListUserRole(List<int> listRoleId, int UserId, int acctionUserId);
        ResponseBase DeleteByListRoleId(List<int> listId, int userId,  int acctionUserId);
        
    }
}
