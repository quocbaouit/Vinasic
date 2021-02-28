using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_OrderDepositRepository : RepositoryBase<VINASICEntities,T_OrderDeposit> , IT_OrderDepositRepository
    {
        public T_OrderDepositRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_OrderDepositRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_OrderDepositRepository : IRepository<T_OrderDeposit>
    {
    }
    
}
