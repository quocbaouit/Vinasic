using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_MaterialRepository : RepositoryBase<VINASICEntities,T_Material> , IT_MaterialRepository
    {
        public T_MaterialRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_MaterialRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_MaterialRepository : IRepository<T_Material>
    {
    }
    
}
