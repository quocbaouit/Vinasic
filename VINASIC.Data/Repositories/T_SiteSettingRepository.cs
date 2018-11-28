using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_SiteSettingRepository : RepositoryBase<VINASICEntities,T_SiteSetting> , IT_SiteSettingRepository
    {
        public T_SiteSettingRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_SiteSettingRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_SiteSettingRepository : IRepository<T_SiteSetting>
    {
    }
    
}
