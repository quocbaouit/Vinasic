using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;


namespace VINASIC.Business.Interface.Enum
{

    public enum OrderStatus
    {
        [Description("Đơn hàng vừa được tạo")]
        Created = 0,
        [Description("Đang xử lý")]
        Inprogess = 1,
        [Description("Chưa giao hàng")]
        NotDelivery = 2,
        [Description("Đã giao hàng")]
        Delivery = 3,
        [Description("Đã thanh toán")]
        Paid = 4,
        [Description("Đã duyệt")]
        Approval = 5,
    }
    public enum DetailStatus
    {
        [Description("Không Xử Lý")]
        None = 0,
        [Description("Đang thiết kế")]
        Designing = 1,
        [Description("Đã thiết kế xong")]
        DesignCompleted = 2,
        [Description("Đang in ấn")]
        Printing = 3,
        [Description("Đã in xong")]
        PrintCompleted = 4,
        [Description("Đang gia công")]
        Addoning = 5,
        [Description("Đã gia công xong")]
        AddonCompleted = 6,
        [Description("Đã Xong.Đợi giao hàng")]
        Complete = 7,
    }
}
