using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_QuittanceRepository : RepositoryBase<VINASICEntities,T_Quittance> , IT_QuittanceRepository
    {
        public T_QuittanceRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_QuittanceRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_QuittanceRepository : IRepository<T_Quittance>
    {
    }
    
}
