using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamic.Framework;
using Dynamic.Framework.Infrastructure.Data;
using PagedList;
using VINASIC.Data;
using VINASIC.Data.Repositories;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Object;
using Dynamic.Framework.Mvc;
using GPRO.Ultilities;

namespace SystemAccount.Bussiness
{
    public class BLLPermission : IBLLPermission
    {
        private readonly IT_PermissionRepository repPermission;
        private readonly IT_FeatureRepository repFeature;
        private readonly IUnitOfWork<VINASICEntities> unitOfWork;
        private readonly IBLLUserRole bllUserRole;
        private readonly IBLLRolePermission bllRolePermission;
        public BLLPermission(IUnitOfWork<VINASICEntities> _unitOfWork, IT_PermissionRepository _repPermission, IBLLUserRole _bllUserRole, IBLLRolePermission _bllRolePermission, IT_FeatureRepository _repFeature)
        {
            this.repFeature = _repFeature;
            this.unitOfWork = _unitOfWork;
            this.repPermission = _repPermission;
            this.bllUserRole = _bllUserRole;
            this.bllRolePermission = _bllRolePermission;
        }
        private void SaveChange()
        {
            unitOfWork.Commit();
        }
        private bool CheckPermissionName(string permissionName, int id)
        {
            var checkResult = false;
            var checkName = repPermission.GetMany(c => !c.IsDeleted && c.Id != id && c.PermissionName.Trim().ToUpper().Equals(permissionName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public ResponseBase Create(ModelPermission obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckPermissionName(obj.PermissionName, obj.Id))
                    {

                        var permission = new T_Permission();
                        Parse.CopyObject(obj, ref permission);
                        permission.CreatedDate = DateTime.Now.AddHours(14);
                        permission.PermissionTypeId = 1;
                        permission.SystemName = "VSystem";
                        repPermission.Add(permission);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create Permission", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Permission", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create Permission", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelPermission obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            if (!CheckPermissionName(obj.PermissionName, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdatePermission", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_Permission permission = repPermission.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (permission != null)
                {
                    permission.FeatureId = obj.FeatureId;
                    permission.PermissionName = obj.PermissionName;
                    permission.Url = obj.Url;
                    permission.Description = obj.Description;
                    permission.UpdatedDate = DateTime.Now.AddHours(14);
                    permission.UpdatedUser = obj.UpdatedUser;
                    repPermission.Update(permission);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdatePermission", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var permission = repPermission.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (permission != null)
            {
                permission.IsDeleted = true;
                permission.DeletedUser = userId;
                permission.DeletedDate = DateTime.Now.AddHours(14);
                repPermission.Update(permission);
                SaveChange();
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Delete", Message = "Đối Tượng Đã Bị Xóa,Vui Lòng Kiểm Tra Lại" });
            }
            return responResult;
        }

        public ResponseBase DeleteByListId(List<int> listId, int userId)
        {
            throw new NotImplementedException();
        }

        public PagedList<T_Permission> GetList( int startIndexRecord, int pageSize, string sorting)
        {
            throw new NotImplementedException();
        }

        public ResponseBase GetListPermissionByListRoleId(List<int> listRoleId)
        {
            ResponseBase responResult = null;
            try
            {
                //var listPermission = repPermission.GetMany(x=>x.)
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return responResult;
        }
        public List<T_Permission> GetListPermissionByFeatureID(int FeatureId)
        {
            List<T_Permission> listPermisson = null;
            try
            {
                 listPermisson = repPermission.GetMany(x => !x.IsDeleted && x.FeatureId == FeatureId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listPermisson;
        }


        public List<ModelPermission> GetAllPermision()
        {
            var allFeature = repFeature.GetMany(x => !x.IsDeleted &&!x.IsSystem).ToList();
            var allPermission = repPermission.GetMany(x => !x.IsDeleted).ToList();
            var listPermision = new List<ModelPermission>();
            foreach (var feature in allFeature)
            {
                var permission = new ModelPermission();
                permission.Id = 0;
                permission.FeatureId = feature.Id;
                permission.FeatureName = feature.FeatureName.ToUpper();
                permission.AlterFeatureName = feature.FeatureName.ToUpper();
                listPermision.Add(permission);
                var listpermisiondepent = allPermission.Where(x => !x.IsDeleted && x.FeatureId == feature.Id).Select(c => new ModelPermission()
                {
                    FeatureName = "",
                    AlterFeatureName = feature.FeatureName.ToUpper(),
                    Id = c.Id,
                    FeatureId = c.FeatureId,
                    PermissionName = c.PermissionName ?? "",
                    Description = c.Description ?? "",
                    Url = c.Url ?? "",
                    Selected = false,
                    CreatedDate = c.CreatedDate,
                }).ToList();
                if (listpermisiondepent != null)
                { listPermision.AddRange(listpermisiondepent); }

            }
            return listPermision;
        }
        public PagedList<ModelPermission> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var listPermision = GetAllPermision();
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelPermission>(listPermision, pageNumber, pageSize);
        }
        public PagedList<ModelPermission> GetListPermissionForRole(int roleId,string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var listPermision = GetAllPermision();
            var listPermissionByRoleId =bllRolePermission.GetListPermissionByRoleId(roleId);
            foreach (var permission in listPermision)
            {
                if (listPermissionByRoleId.Contains(permission.Id)) 
                {
                    permission.Selected = true;
                }
            }
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelPermission>(listPermision, pageNumber, pageSize);
        }

        public List<int> GetListPermissinIdbyRoleId(int roleId)
        {
            return bllRolePermission.GetListPermissionByRoleId(roleId); 
        }
    }
}
