using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_ProductTypeRepository : RepositoryBase<VINASICEntities,T_ProductType> , IT_ProductTypeRepository
    {
        public T_ProductTypeRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_ProductTypeRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_ProductTypeRepository : IRepository<T_ProductType>
    {
    }
    
}
