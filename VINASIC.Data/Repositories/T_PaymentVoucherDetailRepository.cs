using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Data.Repositories
{
    public partial class T_PaymentVoucherDetailRepository : RepositoryBase<VINASICEntities,T_PaymentVoucherDetail> , IT_PaymentVoucherDetailRepository
    {
        public T_PaymentVoucherDetailRepository(VINASICEntities dataContext) : base(dataContext)
    	{
    	
    	}
    
    	public T_PaymentVoucherDetailRepository(IUnitOfWork<VINASICEntities> unitOfWork) : base(unitOfWork.DataContext)
    	{
    	
    	}
        
    }
    
    public interface IT_PaymentVoucherDetailRepository : IRepository<T_PaymentVoucherDetail>
    {
    }
    
}
