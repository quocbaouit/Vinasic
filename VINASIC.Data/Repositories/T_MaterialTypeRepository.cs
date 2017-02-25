using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_MaterialTypeRepository : RepositoryBase<VINASICEntities,T_MaterialType> , IT_MaterialTypeRepository
    {
        public T_MaterialTypeRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_MaterialTypeRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_MaterialTypeRepository : IRepository<T_MaterialType>
    {
    }
    
}
