using System;

namespace VINASIC.Business.Interface.Model
{
    public class ModelCountLoginFail
    {
        public string UserName { get; set; }
        public int Count { get; set; }
        public DateTime TimeLock { get; set; }
        public Boolean isCaptcha { get; set; }
    }
}
