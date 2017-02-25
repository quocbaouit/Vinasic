using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamic.Framework;
using Dynamic.Framework.Infrastructure.Data;
using PagedList;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
using VINASIC.Data.Repositories;
using VINASIC.Object;

namespace SystemAccount.Bussiness
{
    public class BLLUserRole : IBLLUserRole
    {
        private readonly IT_UserRoleRepository repUserRole;
        private readonly IT_RoLeRepository repRole;
        private readonly IUnitOfWork<VINASICEntities> unitOfWork;
        public BLLUserRole(IUnitOfWork<VINASICEntities> _unitOfWork, IT_UserRoleRepository _repUserRole, IT_RoLeRepository _repRole)
        {
            this.unitOfWork = _unitOfWork;
            this.repUserRole = _repUserRole;
            this.repRole = _repRole;
        }
        
        private void SaveChange()
        {
            try
            {
                this.unitOfWork.Commit();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ResponseBase Create(ModelUserRole asset)
        {
            throw new NotImplementedException();
        }

        public ResponseBase Update(ModelUserRole asset)
        {
            throw new NotImplementedException();
        }

        public ResponseBase DeleteById(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public ResponseBase DeleteByListRoleId(List<int> listRoleId, int userId, int acctionUserId)
        {
            var result = new ResponseBase();
            try
            {                
                var listUserRole = repUserRole.GetMany(x => !x.IsDeleted && listRoleId.Contains(x.RoleId) && x.UserId == userId );
                if (listRoleId.Count > 0)
                {
                    foreach (var item in listUserRole)
                    {
                        item.IsDeleted = true;
                        item.DeletedDate = DateTime.Now.AddHours(14);
                        item.DeletedUser = acctionUserId;
                        repUserRole.Update(item);
                    }
                    SaveChange();
                    result.IsSuccess = true;                    
                }
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                throw ex;
            }
        }

        public PagedList<ModelUserRole> GetList(int counTryId, int startIndexRecord, int pageSize, string sorting)
        {
            throw new NotImplementedException();
        }

        public List<int> GetUserRolesIdByUserId(int userId)
        {
            List<int> userRolesId = null;
             try
            {
                userRolesId = repUserRole.GetMany(x => !x.IsDeleted && x.UserId == userId).Select(x => x.RoleId ).ToList();                                 
            }
            catch (Exception ex)
            {
                throw ex;
            }
             return userRolesId;
        }

        

        public List<ModelSelectItem> GetUserRolesModelByUserId(int userId, bool IsOwner,int companyId)
        {
            List<ModelSelectItem> roles = null;
            try
            {
                if (IsOwner)
                {
                    roles = repRole.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem()
                    {
                        Name = x.RoleName,
                        Value = x.Id
                    }).ToList();
                }
                else
                {
                    roles = repUserRole.GetMany(x => !x.IsDeleted && x.UserId == userId).Select(x => new ModelSelectItem()
                    {
                        Name = x.T_RoLe.RoleName,
                        Value = x.Id
                    }).ToList();
                }
                
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase AddListUserRole(List<int> listRoleId, int UserId, int acctionUserId)
        {
            try
            {
                var result = new ResponseBase();

                if (listRoleId.Count > 0)
                {
                    foreach (var item in listRoleId)
                    {
                        var userRole = new T_UserRole();
                        userRole.RoleId = item;
                        userRole.UserId = UserId;
                        userRole.CreatedDate = DateTime.Now.AddHours(14);
                        userRole.CreatedUser = acctionUserId;
                        repUserRole.Add(userRole);
                    }
                    SaveChange();
                    result.IsSuccess = true;
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool TryRestoreRolePermission(T_UserRole useRole)
        {
            try
            {
                var exist = repUserRole.Get(x => x.RoleId == useRole.RoleId && x.UserId == useRole.UserId);
                if (exist != null)
                {
                    exist.IsDeleted = false;
                    repUserRole.Update(exist);
                    SaveChange();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public ResponseBase UpdateUserRole(int userId, List<int> roleIds)
        {
            if (roleIds == null)
            {
                roleIds = new List<int> { 0 };
            }
            ResponseBase result = new ResponseBase { IsSuccess = false };

            var insertRole = new List<T_RoLe>();
            var deleteRole = new List<T_UserRole>();
            var existUserRole= repUserRole.GetMany(x => x.UserId == userId && !x.IsDeleted);
            var existUserRoleId = existUserRole.Select(x => x.RoleId).ToList();

            var selectRole = repRole.GetMany(x => !x.IsDeleted && roleIds.Contains(x.Id));
            var selectRoleId = selectRole.Select(x => x.Id).ToList();

            insertRole = selectRole.Where(x => !x.IsDeleted && !existUserRoleId.Contains(x.Id)).ToList();
            deleteRole = existUserRole.Where(x => !x.IsDeleted && !selectRoleId.Contains(x.RoleId)).ToList();
            var numberDelete = deleteRole.Count;
            for (var i = 0; i < numberDelete; i++)
            {
                deleteRole[i].IsDeleted = true;
                deleteRole[i].DeletedDate = DateTime.Now;
                repUserRole.Update(deleteRole[i]);
                SaveChange();
            }

            var numberInsert = insertRole.Count;
            for (var i = 0; i < numberInsert; i++)
            {
                T_UserRole userRole = new T_UserRole();
                userRole.IsDeleted = false;
                userRole.CreatedDate = DateTime.Now;
                userRole.CreatedUser = 1;
                userRole.UserId = userId;
                userRole.RoleId = insertRole[i].Id;
                var tryRestore = TryRestoreRolePermission(userRole);
                if (!tryRestore)
                {
                    repUserRole.Add(userRole);
                    SaveChange();
                }
            }
            result.IsSuccess = true;


            return result;
        }
    }
}
