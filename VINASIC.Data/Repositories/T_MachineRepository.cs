using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_MachineRepository : RepositoryBase<VINASICEntities,T_Machine> , IT_MachineRepository
    {
        public T_MachineRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_MachineRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_MachineRepository : IRepository<T_Machine>
    {
    }
    
}
