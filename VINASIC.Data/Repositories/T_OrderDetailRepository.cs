using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_OrderDetailRepository : RepositoryBase<VINASICEntities,T_OrderDetail> , IT_OrderDetailRepository
    {
        public T_OrderDetailRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_OrderDetailRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_OrderDetailRepository : IRepository<T_OrderDetail>
    {
    }
    
}
