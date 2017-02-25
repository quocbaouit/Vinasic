using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_RoLeRepository : RepositoryBase<VINASICEntities,T_RoLe> , IT_RoLeRepository
    {
        public T_RoLeRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_RoLeRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_RoLeRepository : IRepository<T_RoLe>
    {
    }
    
}
