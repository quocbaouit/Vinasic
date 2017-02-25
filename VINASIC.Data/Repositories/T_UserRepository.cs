using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_UserRepository : RepositoryBase<VINASICEntities,T_User> , IT_UserRepository
    {
        public T_UserRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_UserRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_UserRepository : IRepository<T_User>
    {
    }
    
}
