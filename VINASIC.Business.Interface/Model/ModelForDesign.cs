using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
   public class ModelForDesign:T_OrderDetail
    {
       public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public string StrdesignStatus { get; set; }

       public int CreateForUser { get; set; }
    }
}
