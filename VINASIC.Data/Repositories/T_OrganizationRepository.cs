using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_OrganizationRepository : RepositoryBase<VINASICEntities,T_Organization> , IT_OrganizationRepository
    {
        public T_OrganizationRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_OrganizationRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_OrganizationRepository : IRepository<T_Organization>
    {
    }
    
}
