using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VINASIC.Object;
namespace VINASIC.Business.Interface.Model
{
   public class ModelMenuCategory:T_MenuCategory
    {
        public bool isHidden { get; set; }
        public bool isDefault { get; set; }
        public bool isConfigExits { get; set; }
        public List<ModelMenu> listMenu { get; set; }
    }
}
