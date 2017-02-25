using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_RolePermissionRepository : RepositoryBase<VINASICEntities,T_RolePermission> , IT_RolePermissionRepository
    {
        public T_RolePermissionRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_RolePermissionRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_RolePermissionRepository : IRepository<T_RolePermission>
    {
    }
    
}
