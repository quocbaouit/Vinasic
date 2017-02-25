using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_NotificationRepository : RepositoryBase<VINASICEntities,T_Notification> , IT_NotificationRepository
    {
        public T_NotificationRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_NotificationRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_NotificationRepository : IRepository<T_Notification>
    {
    }
    
}
