using System.Collections.Generic;
using PagedList;
using VINASIC.Business.Interface.Model;
using VINASIC.Object;

namespace VINASIC.Business.Interface
{
    public interface IBLLRolePermission 
    {
        ResponseBase Create(ModelRolePermission rolePermission);
        ResponseBase Update(ModelRolePermission rolePermission);
        ResponseBase DeleteById(int id, int userId);
        ResponseBase DeleteByListId(List<int> listId, int userId);
        List<string> GetListSystemNameAndUrlOfPermissionByListRoleId(List<int> listRoleId);
        List<string> GetListSystemNameAndUrlOfPermissionByListRoleId(List<int> listRoleId, int moduleId);
        List<ModelFeature> GetListFeatureByListRoleId(List<int> listRoleId);
        List<ModelPermission> GetListPermissionByListRoleId(List<int> listRoleId);
        List<ModelFeature> GetListFeatureByModuleIdOfRoles(List<int> listRoleId, int moduleId);
        List<ModelPermission> GetAllPermission();
        PagedList<ModelRolePermission> GetList(int counTryId, int startIndexRecord, int pageSize, string sorting);
        List<ModelPermission> GetListPermissionByListRoleId(List<int> listRoleId, int moduleId);
        List<int> GetListModuleIdByListRoleId(List<int> RoleIds);
        List<string> GetSystemNameAndUrlOfPermissionBycompanyId(int companyId);
        ResponseBase UpdateRolePermision(int roleId, List<int> permissionId);
        List<int> GetListPermissionByRoleId(int roleId);

        // service
        List<T_RolePermission> GetRolesByCompanyId(int companyId);
    }
}
