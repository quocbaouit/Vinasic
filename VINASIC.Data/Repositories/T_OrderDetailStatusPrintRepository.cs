using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_OrderDetailStatusPrintRepository : RepositoryBase<VINASICEntities,T_OrderDetailStatusPrint> , IT_OrderDetailStatusPrintRepository
    {
        public T_OrderDetailStatusPrintRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_OrderDetailStatusPrintRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_OrderDetailStatusPrintRepository : IRepository<T_OrderDetailStatusPrint>
    {
    }
    
}
