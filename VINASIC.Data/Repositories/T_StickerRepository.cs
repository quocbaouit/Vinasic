using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_StickerRepository : RepositoryBase<VINASICEntities,T_Sticker> , IT_StickerRepository
    {
        public T_StickerRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_StickerRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_StickerRepository : IRepository<T_Sticker>
    {
    }
    
}
