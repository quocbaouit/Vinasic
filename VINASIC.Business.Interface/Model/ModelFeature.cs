using System.Collections.Generic;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelFeature:T_Feature
    {
        public List<ModelPermission> Permissions { get; set; }
    }
}
