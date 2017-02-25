using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_ElementFormularRepository : RepositoryBase<VINASICEntities,T_ElementFormular> , IT_ElementFormularRepository
    {
        public T_ElementFormularRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_ElementFormularRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_ElementFormularRepository : IRepository<T_ElementFormular>
    {
    }
    
}
