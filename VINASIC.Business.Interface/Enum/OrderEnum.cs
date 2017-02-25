using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;


namespace VINASIC.Business.Interface.Enum
{
    public enum DetailStatus
    {
        [Description("Không")]
        None = 0,
        [Description("Đang Xử Lý")]
        Waitting = 1,
        [Description("Hoàn Thành")]
        Completed = 2
    }
}
