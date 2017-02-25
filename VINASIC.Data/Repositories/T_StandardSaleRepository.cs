using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_StandardSaleRepository : RepositoryBase<VINASICEntities,T_StandardSale> , IT_StandardSaleRepository
    {
        public T_StandardSaleRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_StandardSaleRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_StandardSaleRepository : IRepository<T_StandardSale>
    {
    }
    
}
