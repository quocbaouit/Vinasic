using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    //public partial class T_StockInDetailRepository : RepositoryBase<VINASICEntities,T_StockInDetail> , IT_StockInDetailRepository
    //{
    //    public T_StockInDetailRepository(VINASICEntities dataContext) : base(dataContext)
    //	{
    	
    //	}
    
    //	public T_StockInDetailRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    //	{
    	
    //	}
        
    //}
    
    public interface IT_StockInDetailRepository : IRepository<T_StockInDetail>
    {
    }
    
}
