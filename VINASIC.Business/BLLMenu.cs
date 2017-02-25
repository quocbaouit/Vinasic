using Dynamic.Framework.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
using VINASIC.Data.Repositories;

namespace VINASIC.Business
{
    public class BLLMenu : IBLLMenu
    {
        private readonly IT_MenuRepository repMenu;
        private readonly IUnitOfWork<VINASICEntities> unitOfWork;
        public BLLMenu(IUnitOfWork<VINASICEntities> _unitOfWork, IT_MenuRepository _repMenu)
        {
            this.unitOfWork = _unitOfWork;
            this.repMenu = _repMenu;
        }
        public IQueryable<ModelMenu> GetListMenuByCheckPermissionType(string position, List<string> listPermissionUrl)
        {
            IQueryable<ModelMenu> listModelMenu = null;
            try
            {
                            if (listPermissionUrl != null && listPermissionUrl.Count > 0)
                            {
                                listModelMenu = repMenu.GetMany(x =>!x.IsDeleted && !x.T_MenuCategory.IsDeleted && x.T_MenuCategory.Position.Equals(position)).Select(x => new ModelMenu()
                                {
                                    Id = x.Id,
                                    MenuName = x.MenuName,
                                    Icon = x.Icon,
                                    OrderIndex = x.OrderIndex,
                                    Link = x.Link,
                                    IsShow = x.IsShow,
                                    IsViewIcon = x.IsViewIcon,
                                    Description = x.Description,
                                    MenuCategoryId = x.MenuCategoryId
                                });
                            }                                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModelMenu;
        }
    }
}
