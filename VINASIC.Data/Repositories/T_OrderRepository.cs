using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_OrderRepository : RepositoryBase<VINASICEntities,T_Order> , IT_OrderRepository
    {
        public T_OrderRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_OrderRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_OrderRepository : IRepository<T_Order>
    {
    }
    
}
