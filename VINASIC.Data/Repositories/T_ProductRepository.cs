using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_ProductRepository : RepositoryBase<VINASICEntities,T_Product> , IT_ProductRepository
    {
        public T_ProductRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_ProductRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_ProductRepository : IRepository<T_Product>
    {
    }
    
}
