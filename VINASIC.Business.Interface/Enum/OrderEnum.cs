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
        [Description("Chưa thiết kế")]
        waitDesign = 1,
        [Description("Đang thiết kế")]
        Designing = 2,
        [Description("Đã thiết kế xong. Đợi in")]
        DesignCompleted = 3,
        [Description("Đang in ấn")]
        Printing = 4,
        [Description("Đã in xong. Đợi gia công")]
        PrintCompleted = 5,
        [Description("Đang gia công")]
        Addoning = 6,
        [Description("Đã xong")]
        AddonCompleted = 7,     
    }
}
