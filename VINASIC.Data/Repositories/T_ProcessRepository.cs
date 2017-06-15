using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_ProcessRepository : RepositoryBase<VINASICEntities,T_Process> , IT_ProcessRepository
    {
        public T_ProcessRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_ProcessRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_ProcessRepository : IRepository<T_Process>
    {
    }
    
}
