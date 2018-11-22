using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_StatusRepository : RepositoryBase<VINASICEntities,T_Status> , IT_StatusRepository
    {
        public T_StatusRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_StatusRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_StatusRepository : IRepository<T_Status>
    {
    }
    
}
