using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_FormularDetailRepository : RepositoryBase<VINASICEntities,T_FormularDetail> , IT_FormularDetailRepository
    {
        public T_FormularDetailRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_FormularDetailRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_FormularDetailRepository : IRepository<T_FormularDetail>
    {
    }
    
}
