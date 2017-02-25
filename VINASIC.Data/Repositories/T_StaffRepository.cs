using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_StaffRepository : RepositoryBase<VINASICEntities,T_Staff> , IT_StaffRepository
    {
        public T_StaffRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_StaffRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_StaffRepository : IRepository<T_Staff>
    {
    }
    
}
