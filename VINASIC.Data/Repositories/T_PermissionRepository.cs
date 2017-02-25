using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_PermissionRepository : RepositoryBase<VINASICEntities,T_Permission> , IT_PermissionRepository
    {
        public T_PermissionRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_PermissionRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_PermissionRepository : IRepository<T_Permission>
    {
    }
    
}
