using System.Collections.Generic;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelEmployee : T_User
    {
        public string PositionName { get; set; }
    }
    public class ModelSimpleEmployee
    {
       public List<SimpleEmployee> designUser { get; set; }
        public List<SimpleEmployee> printingUser { get; set; }
        public List<SimpleEmployee>  addOnUser { get; set; }
    }
    public class SimpleEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

