using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_StockInRepository : RepositoryBase<VINASICEntities,T_StockIn> , IT_StockInRepository
    {
        public T_StockInRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_StockInRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_StockInRepository : IRepository<T_StockIn>
    {
    }
    
}
