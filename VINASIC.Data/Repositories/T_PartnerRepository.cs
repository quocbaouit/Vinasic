using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_PartnerRepository : RepositoryBase<VINASICEntities,T_Partner> , IT_PartnerRepository
    {
        public T_PartnerRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_PartnerRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_PartnerRepository : IRepository<T_Partner>
    {
    }
    
}
