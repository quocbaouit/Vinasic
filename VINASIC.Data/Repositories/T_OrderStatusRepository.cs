using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_OrderStatusRepository : RepositoryBase<VINASICEntities,T_OrderStatus> , IT_OrderStatusRepository
    {
        public T_OrderStatusRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_OrderStatusRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_OrderStatusRepository : IRepository<T_OrderStatus>
    {
    }
    
}
