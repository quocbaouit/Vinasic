using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_MenuCategoryRepository : RepositoryBase<VINASICEntities,T_MenuCategory> , IT_MenuCategoryRepository
    {
        public T_MenuCategoryRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_MenuCategoryRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_MenuCategoryRepository : IRepository<T_MenuCategory>
    {
    }
    
}
