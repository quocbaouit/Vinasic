using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_UserProductRepository : RepositoryBase<VINASICEntities,T_UserProduct> , IT_UserProductRepository
    {
        public T_UserProductRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_UserProductRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_UserProductRepository : IRepository<T_UserProduct>
    {
    }
    
}
