using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_ErrorLogRepository : RepositoryBase<VINASICEntities,T_ErrorLog> , IT_ErrorLogRepository
    {
        public T_ErrorLogRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_ErrorLogRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_ErrorLogRepository : IRepository<T_ErrorLog>
    {
    }
    
}
