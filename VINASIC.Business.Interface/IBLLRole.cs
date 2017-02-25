using System.Collections.Generic;
using PagedList;
using VINASIC.Business.Interface.Model;
using VINASIC.Object;

namespace VINASIC.Business.Interface
{
    public interface IBLLRole
    {
        ResponseBase Create(ModelRole obj);
        ResponseBase Update(ModelRole obj);
        ResponseBase DeleteById(int id, int userId);
        List<ModelSelectItem> GetRolesNotSystem(int companyId);
        List<ModelFeature> GetListFeatureByUserId(int userId);
        List<ModelPermission> GetListPermissionByUserId(int userId);
        List<ModelFeature> GetListFeatureAndPermissionByModuleId(int moduleId, int userId);
        ModelRole GetRoleDetailByRoleId(int roleId);
        List<ModelRolePermission> GetListRolePermissionByRoleId(int roleId);
        List<ModelRole> GetListRoleByIsSystem();
        ResponseBase DeleteByListId(List<int> listId, int userId);
        PagedList<T_RoLe> GetListRole(string keyWord, int startIndexRecord, int pageSize, string sorting, int userId, int companyId, bool IsOwner);
        PagedList<ModelRole> GetListRoleForUser(string keyWord, int startIndexRecord, int pageSize, string sorting, int userId);
        List<int> GetListRoleIdByCompanyId(int companyId);
        List<T_RoLe> GetListRoleByCompanyId(int companyId);
        List<T_RoLe> GetListRoleByUser(int userId);
        ModelRole GetRoleByCompanyId(int companyId);
 
    }
}
