using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_CustomerRepository : RepositoryBase<VINASICEntities,T_Customer> , IT_CustomerRepository
    {
        public T_CustomerRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_CustomerRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_CustomerRepository : IRepository<T_Customer>
    {
    }
    
}
