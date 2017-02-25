using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_UserFormularRepository : RepositoryBase<VINASICEntities,T_UserFormular> , IT_UserFormularRepository
    {
        public T_UserFormularRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_UserFormularRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_UserFormularRepository : IRepository<T_UserFormular>
    {
    }
    
}
