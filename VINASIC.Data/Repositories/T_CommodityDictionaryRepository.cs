using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_CommodityDictionaryRepository : RepositoryBase<VINASICEntities,T_CommodityDictionary> , IT_CommodityDictionaryRepository
    {
        public T_CommodityDictionaryRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_CommodityDictionaryRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_CommodityDictionaryRepository : IRepository<T_CommodityDictionary>
    {
    }
    
}
