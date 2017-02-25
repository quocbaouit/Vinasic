using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_PaymentVoucherRepository : RepositoryBase<VINASICEntities,T_PaymentVoucher> , IT_PaymentVoucherRepository
    {
        public T_PaymentVoucherRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_PaymentVoucherRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_PaymentVoucherRepository : IRepository<T_PaymentVoucher>
    {
    }
    
}
