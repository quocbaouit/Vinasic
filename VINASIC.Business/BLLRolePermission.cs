using System;
using System.Collections.Generic;
using System.Linq;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
using VINASIC.Data.Repositories;
using VINASIC.Object;
using Dynamic.Framework.Mvc;

namespace VINASIC.Business
{
    public class BLLRolePermission : IBLLRolePermission
    {
        private readonly IT_RolePermissionRepository repRolePermission;
        private readonly IT_FeatureRepository repFeature;
        private readonly IT_PermissionRepository repPermission;
        private readonly IUnitOfWork<VINASICEntities> unitOfWork;
        public BLLRolePermission(IUnitOfWork<VINASICEntities> _unitOfWork, IT_RolePermissionRepository _repRolePermission, IT_FeatureRepository _repFeature, IT_PermissionRepository _repPermission)
        {
            this.unitOfWork = _unitOfWork;
            this.repRolePermission = _repRolePermission;
            this.repFeature = _repFeature;
            this.repPermission = _repPermission;
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

        public ResponseBase Create(ModelRolePermission rolePermission)
        {
            throw new NotImplementedException();
        }

        public ResponseBase Update(ModelRolePermission rolePermission)
        {
            throw new NotImplementedException();
        }

        public ResponseBase DeleteById(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public ResponseBase DeleteByListId(List<int> listId, int userId)
        {
            throw new NotImplementedException();
        }

        public List<string> GetListSystemNameAndUrlOfPermissionByListRoleId(List<int> listRoleId)
        {
            List<string> systemNameUrlPermission = null;
            try
            {
                if (listRoleId != null)
                {
                    var listPermission = new List<ModelPermission>();
                    listPermission = GetListPermissionByListRoleId(listRoleId);
                    if (listPermission != null && listPermission.Any())
                    {
                        systemNameUrlPermission = new List<string>();
                        //systemNameUrlPermission.AddRange(listPermission.Select(x => x.SystemName).ToList());
                        foreach (var item in listPermission)
                        {
                            if (item.Url != null)
                            {
                                systemNameUrlPermission.AddRange(item.Url.Split('|').ToList());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return systemNameUrlPermission;
        }

        public List<string> GetListSystemNameAndUrlOfPermissionByListRoleId(List<int> listRoleId, int moduleId)
        {
            List<string> systemNameUrlPermission = null;
            try
            {
                List<ModelPermission> listPermission;
                if (listRoleId != null && listRoleId.Count > 0)
                {
                    if (listRoleId.Contains(1))
                    {
                        listPermission = GetListPermissionByListRoleId(listRoleId, moduleId);
                    }
                    listPermission = GetListPermissionByListRoleId(listRoleId, moduleId);
                    if (listPermission != null && listPermission.Any())
                    {
                        systemNameUrlPermission = new List<string>();
                        systemNameUrlPermission.AddRange(listPermission.Select(x => x.SystemName).ToList());
                        foreach (var item in listPermission)
                        {
                            if (item.Url != null)
                            {
                                systemNameUrlPermission.AddRange(item.Url.Split('|').ToList());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return systemNameUrlPermission;
        }
        public List<ModelFeature> GetListFeatureByListRoleId(List<int> listRoleId)
        {
            List<ModelFeature> listFeature = null;
            try
            {
                if (listRoleId != null && listRoleId.Count > 0)
                {

                    var featureIds = repRolePermission.GetMany(x => !x.IsDeleted && !x.T_Feature.IsDeleted && listRoleId.Contains(x.RoleId)).Select(c => c.FeatureId).Distinct();
                    if (featureIds != null)
                    {
                        listFeature = repFeature.GetMany(x => featureIds.Contains(x.Id)).Select(c => new ModelFeature()
                        {
                            Id = c.Id,
                            FeatureName = c.FeatureName,
                            IsDefault = c.IsDefault
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listFeature;
        }
        public List<ModelPermission> GetAllPermission()
        {
            List<ModelPermission> listPermission = null;
            try
            {

                var permissionIds = repRolePermission.GetMany(x => !x.IsDeleted).Select(c => c.PermissionId).Distinct();
                if (permissionIds != null)
                {
                    listPermission = repPermission.GetMany(c => permissionIds.Contains(c.Id)).Select(x => new ModelPermission()
                    {
                        Id = x.Id,
                        PermissionName = x.PermissionName,
                        FeatureId = x.FeatureId,
                        SystemName = x.SystemName,
                        Url = x.Url,
                        T_Feature = x.T_Feature,
                        IsDefault = x.IsDefault

                    }).ToList();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listPermission;
        }
        public List<ModelPermission> GetListPermissionByListRoleId(List<int> listRoleId)
        {
            List<ModelPermission> listPermission = null;
            try
            {
                if (listRoleId != null)
                {
                    var permissionIds = repRolePermission.GetMany(x => !x.IsDeleted && !x.T_Permission.IsDeleted && !x.T_Permission.T_Feature.IsDeleted && (listRoleId.Contains(x.RoleId) || (x.T_Feature.IsDefault || x.T_Permission.IsDefault))).Select(c => c.PermissionId).Distinct().ToList();
                    if (permissionIds != null)
                    {
                        listPermission = repPermission.GetMany(c => permissionIds.Contains(c.Id)).Select(x => new ModelPermission()
                        {
                            Id = x.Id,
                            PermissionName = x.PermissionName,
                            FeatureId = x.FeatureId,
                            SystemName = x.SystemName,
                            Url = x.Url,
                            T_Feature = x.T_Feature,
                            IsDefault = x.IsDefault

                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listPermission;
        }

        public List<ModelPermission> GetListPermissionByListRoleId(List<int> listRoleId, int moduleId)
        {
            List<ModelPermission> listPermission = null;
            try
            {
                if (listRoleId != null && listRoleId.Count > 0)
                {
                    var permissionIds = repRolePermission.GetMany(x => !x.IsDeleted && !x.T_Permission.IsDeleted && !x.T_Permission.T_Feature.IsDeleted && (listRoleId.Contains(x.RoleId) || (x.T_Feature.IsDefault || x.T_Permission.IsDefault))).Select(c => c.PermissionId).Distinct();
                    var a = permissionIds.ToList();
                    var per = repRolePermission.GetMany(x => !x.IsDeleted && !x.T_Permission.IsDeleted && !x.T_Permission.T_Feature.IsDeleted && listRoleId.Contains(x.RoleId)).ToList();

                    if (permissionIds != null)
                    {
                        listPermission = repPermission.GetMany(c => permissionIds.Contains(c.Id))
                         .Select(x => new ModelPermission()
                         {
                             Id = x.Id,
                             PermissionName = x.PermissionName,
                             FeatureId = x.FeatureId,
                             SystemName = x.SystemName,
                             Url = x.Url,
                             T_Feature = x.T_Feature,
                             IsDefault = x.IsDefault

                         }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listPermission;
        }

        private List<ModelPermission> GetListPermissionByFeatureIdOfRoles(List<int> listRoleId, int featureId)
        {
            List<ModelPermission> listPermission = null;
            try
            {
                var listPermissionOfAllFeature = GetListPermissionByListRoleId(listRoleId);
                if (listPermissionOfAllFeature != null && listPermissionOfAllFeature.Count > 0)
                {
                    listPermission = listPermissionOfAllFeature.Where(c => c.FeatureId == featureId).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listPermission;
        }

        public List<ModelFeature> GetListFeatureByModuleIdOfRoles(List<int> listRoleId, int moduleId)
        {
            List<ModelFeature> listFeature = null;
            try
            {
                var listFeatureOfAllModule = GetListFeatureByListRoleId(listRoleId);
                if (listFeatureOfAllModule != null && listFeatureOfAllModule.Count() > 0)
                {
                    var listFeatureId = listFeatureOfAllModule.Select(c => c.Id).Distinct();
                    listFeature = listFeatureOfAllModule.Where(c => listFeatureId.Contains(c.Id)).Select(c => new ModelFeature()
                    {
                        Id = c.Id,
                        FeatureName = c.FeatureName,
                        IsDefault = c.IsDefault
                    }).ToList();
                    if (listFeature.Count > 0)
                    {
                        var listPermissionOfAllRoles = GetListPermissionByListRoleId(listRoleId);
                        if (listPermissionOfAllRoles != null && listPermissionOfAllRoles.Count > 0)
                        {
                            foreach (var feature in listFeature)
                            {
                                feature.Permissions = listPermissionOfAllRoles.Where(c => c.FeatureId == feature.Id).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listFeature;
        }

        public PagedList.PagedList<ModelRolePermission> GetList(int counTryId, int startIndexRecord, int pageSize, string sorting)
        {
            throw new NotImplementedException();
        }


        public List<int> GetListModuleIdByListRoleId(List<int> RoleIds)
        {
            try
            {
                if (RoleIds != null || RoleIds.Count > 0)
                {
                    //return repRolePermission.GetMany(x => !x.IsDeleted && RoleIds.Contains(x.RoleId)).Select(x => x.ModuleId).ToList();
                    return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T_RolePermission> GetRolesByCompanyId(int companyId)
        {
            try
            {
                List<T_RolePermission> rolePermissions = null;
                rolePermissions = repRolePermission.GetMany(x => !x.IsDeleted).ToList();
                return rolePermissions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<string> GetSystemNameAndUrlOfPermissionBycompanyId(int companyId)
        {
            try
            {
                List<string> systemNameUrlPermission = null;
                var permissionIds = repRolePermission.GetMany(c => c.T_RoLe.IsSystem && !c.T_RoLe.IsDeleted).Select(c => c.PermissionId).ToList();
                if (permissionIds != null)
                {
                    var permissions = repPermission.GetMany(c => permissionIds.Contains(c.Id)).Select(x => new ModelPermission()
                    {
                        Id = x.Id,
                        PermissionName = x.PermissionName,
                        FeatureId = x.FeatureId,
                        SystemName = x.SystemName,
                        Url = x.Url,
                        T_Feature = x.T_Feature,
                        IsDefault = x.IsDefault
                    }).ToList();
                    if (permissions.Count() > 0)
                    {
                        systemNameUrlPermission = new List<string>();
                        systemNameUrlPermission.AddRange(permissions.Select(x => x.SystemName).ToList());
                        foreach (var item in permissions)
                        {
                            if (item.Url != null)
                            {
                                systemNameUrlPermission.AddRange(item.Url.Split('|').ToList());
                            }
                        }
                    }
                }
                if (systemNameUrlPermission != null)
                    return systemNameUrlPermission;
                return new List<string>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<int> GetListPermissionByRoleId(int roleId)
        {
            var listPermisionId = repRolePermission.GetMany(x => !x.IsDeleted && x.RoleId == roleId).Select(x => x.PermissionId).ToList();
            return listPermisionId;
        }
        public bool TryRestoreRolePermission(T_RolePermission rolePermission)
        {
            try
            {
                var exist = repRolePermission.Get(x => x.RoleId == rolePermission.RoleId && x.PermissionId == rolePermission.PermissionId);
                if (exist != null)
                {
                    exist.IsDeleted = false;
                    repRolePermission.Update(exist);
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
        public ResponseBase UpdateRolePermision(int roleId, List<int> permissionId)
        {
            if (permissionId == null)
            {
                permissionId = new List<int> { 0 };
            }
            ResponseBase result = new ResponseBase { IsSuccess = false };

            var insertPermission = new List<T_Permission>();
            var deletePermission = new List<T_RolePermission>();
            var existRolePermission = repRolePermission.GetMany(x => x.RoleId == roleId && !x.IsDeleted);
            var exsitPermissionId = existRolePermission.Select(x => x.PermissionId).ToList();

            var selectPermission = repPermission.GetMany(x => !x.IsDeleted && permissionId.Contains(x.Id));
            var selectPermissionId = selectPermission.Select(x => x.Id).ToList();

            insertPermission = selectPermission.Where(x => !x.IsDeleted && !exsitPermissionId.Contains(x.Id)).ToList();
            deletePermission = existRolePermission.Where(x => !x.IsDeleted && !selectPermissionId.Contains(x.PermissionId)).ToList();
            var numberDelete = deletePermission.Count;
            for (var i = 0; i < numberDelete; i++)
            {
                deletePermission[i].IsDeleted = true;
                deletePermission[i].DeletedDate = DateTime.Now;
                repRolePermission.Update(deletePermission[i]);
                SaveChange();
            }

            var numberInsert = insertPermission.Count;
            for (var i = 0; i < numberInsert; i++)
            {
                T_RolePermission rolePermission = new T_RolePermission();
                rolePermission.IsDeleted = false;
                rolePermission.CreatedDate = DateTime.Now;
                rolePermission.CreatedUser = 1;
                rolePermission.RoleId = roleId;
                rolePermission.FeatureId = insertPermission[i].FeatureId;
                rolePermission.PermissionId = insertPermission[i].Id;
                var tryRestore = TryRestoreRolePermission(rolePermission);
                if (!tryRestore)
                {
                    repRolePermission.Add(rolePermission);
                    SaveChange();
                }
            }
            result.IsSuccess = true;


            return result;
        }
    }
}
