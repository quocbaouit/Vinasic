using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VINASIC.Business.Interface.Model
{
    public class ModelSelectItem
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public int Data { get; set; }
        public string Code { get; set; }
        public bool IsDefault { get; set; }
        public int Type { get; set; }
    }
}
