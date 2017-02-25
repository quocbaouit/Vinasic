using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelPermission : T_Permission
    {
        public int moduleId { get; set; }

        public string FeatureName { get; set; }

        public string AlterFeatureName { get; set; }
        public bool Selected { get; set; }
    }
}
