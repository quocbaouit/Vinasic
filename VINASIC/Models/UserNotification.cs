using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VINASIC.Models
{
    public class UserNotification
    {
        public  List<int> UserId { get; set; }
        public string Notification { get; set; }
    }
}