using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_ProcessDetailRepository : RepositoryBase<VINASICEntities,T_ProcessDetail> , IT_ProcessDetailRepository
    {
        public T_ProcessDetailRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_ProcessDetailRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_ProcessDetailRepository : IRepository<T_ProcessDetail>
    {
    }
    
}
