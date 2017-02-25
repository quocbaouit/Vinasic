using GPRO.Ultilities;
using Dynamic.Framework.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
using VINASIC.Data.Repositories;

namespace VINASIC.Business
{
    public class BLLMenuCategory : IBLLMenuCategory
    {
        private readonly IT_MenuCategoryRepository repMenuCategory;
        private readonly IT_MenuRepository repMenu;
        private readonly IBLLUserRole bllUserRole;
        private readonly IBLLRolePermission bllRolePermission;
        private readonly IBLLMenu bllMenu;
        private readonly IUnitOfWork<VINASICEntities> unitOfWork;
        public BLLMenuCategory(IUnitOfWork<VINASICEntities> _unitOfWork, IT_MenuRepository _repMenu, IT_MenuCategoryRepository _repMenuCategory, IBLLUserRole _bllUserRole, IBLLRolePermission _bllRolePermission, IBLLMenu _bllMenu)
        {
            this.unitOfWork = _unitOfWork;
            this.repMenuCategory = _repMenuCategory;
            this.repMenu = _repMenu;
            this.bllUserRole = _bllUserRole;
            this.bllRolePermission = _bllRolePermission;
            this.bllMenu=_bllMenu;
        }
        private IQueryable<ModelMenuCategory> GetCategorysByPosition( string position)
        {
            IQueryable<ModelMenuCategory> listMenuCategory = null;
            try
            {
                listMenuCategory = repMenuCategory.GetMany(x => !x.IsDeleted  && x.Position.Equals(position)).Select(x => new ModelMenuCategory()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Icon = x.Icon,
                    IsViewIcon = x.IsViewIcon,
                    Link = x.Link,
                    OrderIndex = x.OrderIndex,
                    Description = x.Description,
                });
            }
            catch (Exception)
            {
                throw;
            }
            return listMenuCategory;
        }
        public List<ModelMenuCategory> GetMenusAndMenuCategoriesByUserId(int userId, string position)
        {
            List<ModelMenuCategory> listMenuCategory = null;
            try
            {
                List<string> listPermissionUrl = null;
                List<int> listUserRole = bllUserRole.GetUserRolesIdByUserId(userId);
                listPermissionUrl = bllRolePermission.GetListSystemNameAndUrlOfPermissionByListRoleId(listUserRole);

                var menuCategorys = GetCategorysByPosition(position);
                if (menuCategorys != null && menuCategorys.Count() > 0)
                {
                    listMenuCategory = new List<ModelMenuCategory>();
                    var menus = bllMenu.GetListMenuByCheckPermissionType( position, listPermissionUrl);
                    foreach (var menuCategory in menuCategorys)
                    {
                        bool isAdd = false;
                        if (menus != null && menus.Count() > 0)
                        {
                            var listMenu = menus.Where(x => x.MenuCategoryId == menuCategory.Id).ToList();
                            if (listMenu.Count > 0)
                            {
                                var modelMenuCategory = new ModelMenuCategory();
                                Parse.CopyObject(menuCategory, ref modelMenuCategory);
                                modelMenuCategory.listMenu = listMenu;
                                listMenuCategory.Add(modelMenuCategory);
                                isAdd = true;
                            }
                        }
                        if (!isAdd && !string.IsNullOrEmpty(menuCategory.Link))
                        {                              
                                        if (listPermissionUrl.Contains(menuCategory.Link.Trim()))
                                        {
                                            var modelMenuCategory = new ModelMenuCategory();
                                            Parse.CopyObject(menuCategory, ref modelMenuCategory);
                                            listMenuCategory.Add(modelMenuCategory);
                                        }
                                     
                            }
                        }
                    }
                }
            catch (Exception ex)
            {
                throw ex;
            }
            return listMenuCategory;
        }
    }
}
