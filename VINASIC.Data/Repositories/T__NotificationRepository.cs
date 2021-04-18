using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T__NotificationRepository : RepositoryBase<VINASICEntities,T__Notification> , IT__NotificationRepository
    {
        public T__NotificationRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T__NotificationRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT__NotificationRepository : IRepository<T__Notification>
    {
    }
    
}
