using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_PositionRepository : RepositoryBase<VINASICEntities,T_Position> , IT_PositionRepository
    {
        public T_PositionRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_PositionRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_PositionRepository : IRepository<T_Position>
    {
    }
    
}
