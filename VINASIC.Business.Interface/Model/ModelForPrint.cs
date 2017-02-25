using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelForPrint : T_OrderDetail
    {
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public string StrPrintStatus { get; set; }
    }
}
