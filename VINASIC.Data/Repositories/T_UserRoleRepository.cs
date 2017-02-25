using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_UserRoleRepository : RepositoryBase<VINASICEntities,T_UserRole> , IT_UserRoleRepository
    {
        public T_UserRoleRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_UserRoleRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_UserRoleRepository : IRepository<T_UserRole>
    {
    }
    
}
