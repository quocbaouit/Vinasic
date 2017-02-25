using Dynamic.Framework.Infrastructure.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework;
using Dynamic.Framework.Mvc;
using GPRO.Ultilities;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
using VINASIC.Data.Repositories;
using VINASIC.Object;

namespace SystemAccount.Bussiness
{
    public class BLLRole : IBLLRole
    {
        private readonly IT_RoLeRepository repRole;
        private readonly IT_FeatureRepository repFeature;
        private readonly IT_RolePermissionRepository repRolePermission;
        private readonly IBLLUserRole bllUserRole;
        private readonly IBLLRolePermission bllRolePermission;

        private readonly IUnitOfWork<VINASICEntities> unitOfWork;
        public BLLRole(IUnitOfWork<VINASICEntities> _unitOfWork, IT_RoLeRepository _repRole, IBLLUserRole _bllUserRole, IBLLRolePermission _bllRolePermission, IT_FeatureRepository _repFeature, IT_RolePermissionRepository _repRolePermission)
        {
            this.unitOfWork = _unitOfWork;
            this.repRole = _repRole;
            this.bllUserRole = _bllUserRole;
            this.bllRolePermission = _bllRolePermission;
            this.repFeature = _repFeature;
            this.repRolePermission = _repRolePermission;
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
        public ResponseBase Create(ModelRole obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    var role = new T_RoLe();
                    Parse.CopyObject(obj, ref role);
                    role.CreatedDate = DateTime.Now.AddHours(14);
                    repRole.Add(role);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Role", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create Role", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelRole obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            T_RoLe role = repRole.Get(x => x.Id == obj.Id && !x.IsDeleted);
            if (role != null)
            {
                role.IsSystem = obj.IsSystem;
                role.RoleName = obj.RoleName;
                role.Decription = obj.Decription;
                role.UpdatedDate = DateTime.Now.AddHours(14);
                role.UpdatedUser = obj.UpdatedUser;
                repRole.Update(role);
                SaveChange();
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateRole", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
            }

            return result;
        }

        public ResponseBase DeleteById(int id, int userId)
        {
            ResponseBase result = null;
            T_RoLe role = null;
            try
            {
                result = new ResponseBase();
                role = repRole.GetMany(x => x.Id == id && !x.IsDeleted).FirstOrDefault();
                if (role != null)
                {
                    if (!role.IsSystem)
                    {
                        role.IsDeleted = true;
                        role.DeletedUser = userId;
                        role.DeletedDate = DateTime.Now.AddHours(14);
                        repRole.Update(role);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Delete Role", Message = "Không thể xóa quyền hệ thống" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Delete Role", Message = "Phân Quyền đang thao tác không tồn tại. Vui lòng kiểm tra lại!" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<ModelSelectItem> GetRolesNotSystem(int companyId)
        {
            List<ModelSelectItem> roles = null;
            try
            {
                roles = repRole.GetMany(x => !x.IsDeleted && !x.IsSystem).Select(x => new ModelSelectItem()
                {
                    Name = x.RoleName,
                    Value = x.Id
                }).ToList();
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase DeleteByListId(List<int> listId, int userId)
        {
            throw new NotImplementedException();
        }

        public PagedList<T_RoLe> GetListRole(string keyWord, int startIndexRecord, int pageSize, string sorting, int userId, int companyId, bool IsOwner)
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }                
               var  roles = repRole.GetMany(x => !x.IsDeleted).OrderBy(sorting).ToList();                                           
                var pageNumber = (startIndexRecord / pageSize) + 1;
                return new PagedList<T_RoLe>(roles, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedList<ModelRole> GetListRoleForUser(string keyWord, int startIndexRecord, int pageSize, string sorting, int userId)
        {

            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                var roles = repRole.GetMany(c => !c.IsDeleted).Select(c => new ModelRole()
                {
                    Id = c.Id,
                    RoleName = c.RoleName,
                    Decription = c.Decription,
                    IsSystem = c.IsSystem,
                    CreatedDate = c.CreatedDate,
                    Selected = false
                }).OrderBy(sorting).ToList();

                var listRole = GetListRoleByUser(userId).Select(x=>x.Id).ToList();
                if (listRole.Count > 0)
                {
                    foreach (var role in roles)
                    {
                        if (listRole.Contains(role.Id))
                        {
                            role.Selected = true;
                        }
                    }
                }
                
                var pageNumber = (startIndexRecord / pageSize) + 1;
                return new PagedList<ModelRole>(roles, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ModelFeature> GetListFeatureAndPermissionByModuleId(int moduleId, int userId)
        {
            List<int> listUserRolesIdByUserId = null;
            List<ModelFeature> ListFeatureByListRoleIdAndModuleId = null;
            try
            {
                listUserRolesIdByUserId = bllUserRole.GetUserRolesIdByUserId(userId);
                if (listUserRolesIdByUserId != null && listUserRolesIdByUserId.Count > 0)
                {
                    ListFeatureByListRoleIdAndModuleId = bllRolePermission.GetListFeatureByModuleIdOfRoles(listUserRolesIdByUserId, moduleId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ListFeatureByListRoleIdAndModuleId;
        }

        #region Get list Module - Feature - Permission

        public List<ModelFeature> GetListFeatureByUserId(int userId)
        {
            List<int> listUserRolesIdByUserId = null;
            List<ModelFeature> ListFeatureByListRoleId = null;
            try
            {
                listUserRolesIdByUserId = bllUserRole.GetUserRolesIdByUserId(userId);
                if (listUserRolesIdByUserId != null && listUserRolesIdByUserId.Count > 0)
                {
                    ListFeatureByListRoleId = bllRolePermission.GetListFeatureByListRoleId(listUserRolesIdByUserId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ListFeatureByListRoleId;
        }

        public List<ModelPermission> GetListPermissionByUserId(int userId)
        {
            List<int> listUserRolesIdByUserId = null;
            List<ModelPermission> ListPermissionByListRoleId = null;
            try
            {
                listUserRolesIdByUserId = bllUserRole.GetUserRolesIdByUserId(userId);
                if (listUserRolesIdByUserId != null && listUserRolesIdByUserId.Count > 0)
                {
                    ListPermissionByListRoleId = bllRolePermission.GetListPermissionByListRoleId(listUserRolesIdByUserId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ListPermissionByListRoleId;
        }
        #endregion


        public ModelRole GetRoleDetailByRoleId(int roleId)
        {
            ModelRole role;
            try
            {
                role = repRole.GetMany(x => x.Id == roleId && !x.IsDeleted).Select(x => new ModelRole()
                {
                    Id = x.Id,

                    RoleName = x.RoleName,
                    Decription = x.Decription
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return role;
        }
        public List<ModelRole> GetListRoleByIsSystem()
        {
            var listRole = repRole.GetMany(c => !c.IsDeleted && c.IsSystem).Select(c => new ModelRole()
            {
                Id = c.Id,
                RoleName = c.RoleName,
            }).ToList();
            return listRole;
        }
        public ModelRole GetRoleByCompanyId(int companyId)
        {
            var listRole = repRole.GetMany(c => !c.IsDeleted && c.IsSystem).Select(c => new ModelRole()
            {
                Id = c.Id,
                RoleName = c.RoleName,
            }).FirstOrDefault();
            return listRole;

        }
        public List<ModelRolePermission> GetListRolePermissionByRoleId(int roleId)
        {
            List<ModelRolePermission> listRolePermission;
            try
            {
                listRolePermission = repRolePermission.GetMany(x => x.RoleId == roleId && !x.IsDeleted && !x.T_RoLe.IsDeleted && !x.T_Permission.IsDeleted).Select(x => new ModelRolePermission()
                {
                    Id = x.Id,
                    RoleId = x.RoleId,
                    FeatureId = x.FeatureId,
                    PermissionId = x.PermissionId
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listRolePermission;
        }


        public List<int> GetListRoleIdByCompanyId(int companyId)
        {
            try
            {
                return repRole.GetMany(x => x.IsSystem && !x.IsDeleted).Select(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<T_RoLe> GetListRoleByCompanyId(int companyId)
        {
            try
            {
                return repRole.GetMany(x => !x.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<T_RoLe> GetListRoleByUser(int userId)
        {
            try
            {
                var roleIds = bllUserRole.GetUserRolesIdByUserId(userId);
                var listRoleByUser = repRole.GetMany(x => roleIds.Contains(x.Id)).ToList();
                return listRoleByUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
