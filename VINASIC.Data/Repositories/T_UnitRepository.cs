using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_UnitRepository : RepositoryBase<VINASICEntities,T_Unit> , IT_UnitRepository
    {
        public T_UnitRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_UnitRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_UnitRepository : IRepository<T_Unit>
    {
    }
    
}
