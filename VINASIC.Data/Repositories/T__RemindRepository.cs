using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T__RemindRepository : RepositoryBase<VINASICEntities,T__Remind> , IT__RemindRepository
    {
        public T__RemindRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T__RemindRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT__RemindRepository : IRepository<T__Remind>
    {
    }
    
}
