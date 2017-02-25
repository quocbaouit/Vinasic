using System.Collections.Generic;
using PagedList;
using VINASIC.Business.Interface.Model;
using VINASIC.Object;

namespace VINASIC.Business.Interface
{
    public interface IBLLPermission 
    {
         ResponseBase Create(ModelPermission obj);
        ResponseBase Update(ModelPermission obj);
        ResponseBase DeleteById(int id, int userId);
        ResponseBase DeleteByListId(List<int> listId, int userId);
        ResponseBase GetListPermissionByListRoleId(List<int> listRoleId);
        List<T_Permission> GetListPermissionByFeatureID(int FeatureId);
        List<int> GetListPermissinIdbyRoleId(int roleId);
        PagedList<ModelPermission> GetList(string keyword, int startIndexRecord, int pageSize, string sorting);
        PagedList<ModelPermission> GetListPermissionForRole(int roleId, string keyWord, int startIndexRecord, int pageSize, string sorting);
    }
}
