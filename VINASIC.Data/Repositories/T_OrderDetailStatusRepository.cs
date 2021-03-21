using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_OrderDetailStatusRepository : RepositoryBase<VINASICEntities,T_OrderDetailStatus> , IT_OrderDetailStatusRepository
    {
        public T_OrderDetailStatusRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_OrderDetailStatusRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_OrderDetailStatusRepository : IRepository<T_OrderDetailStatus>
    {
    }
    
}
