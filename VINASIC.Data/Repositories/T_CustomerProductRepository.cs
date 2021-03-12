using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_CustomerProductRepository : RepositoryBase<VINASICEntities,T_CustomerProduct> , IT_CustomerProductRepository
    {
        public T_CustomerProductRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_CustomerProductRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_CustomerProductRepository : IRepository<T_CustomerProduct>
    {
    }
    
}
