using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_MenuRepository : RepositoryBase<VINASICEntities,T_Menu> , IT_MenuRepository
    {
        public T_MenuRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_MenuRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_MenuRepository : IRepository<T_Menu>
    {
    }
    
}
