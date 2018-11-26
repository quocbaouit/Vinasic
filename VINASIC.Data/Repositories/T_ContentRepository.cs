using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_ContentRepository : RepositoryBase<VINASICEntities,T_Content> , IT_ContentRepository
    {
        public T_ContentRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_ContentRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_ContentRepository : IRepository<T_Content>
    {
    }
    
}
