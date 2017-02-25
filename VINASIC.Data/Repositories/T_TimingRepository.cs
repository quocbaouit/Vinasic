using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_TimingRepository : RepositoryBase<VINASICEntities,T_Timing> , IT_TimingRepository
    {
        public T_TimingRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_TimingRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_TimingRepository : IRepository<T_Timing>
    {
    }
    
}
