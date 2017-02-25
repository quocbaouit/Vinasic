using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_FormularRepository : RepositoryBase<VINASICEntities,T_Formular> , IT_FormularRepository
    {
        public T_FormularRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_FormularRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_FormularRepository : IRepository<T_Formular>
    {
    }
    
}
