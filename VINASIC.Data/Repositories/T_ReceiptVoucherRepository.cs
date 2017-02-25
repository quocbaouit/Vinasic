using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_ReceiptVoucherRepository : RepositoryBase<VINASICEntities,T_ReceiptVoucher> , IT_ReceiptVoucherRepository
    {
        public T_ReceiptVoucherRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_ReceiptVoucherRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_ReceiptVoucherRepository : IRepository<T_ReceiptVoucher>
    {
    }
    
}
