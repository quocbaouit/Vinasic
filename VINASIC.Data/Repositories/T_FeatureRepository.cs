using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_FeatureRepository : RepositoryBase<VINASICEntities,T_Feature> , IT_FeatureRepository
    {
        public T_FeatureRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_FeatureRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_FeatureRepository : IRepository<T_Feature>
    {
    }
    
}
