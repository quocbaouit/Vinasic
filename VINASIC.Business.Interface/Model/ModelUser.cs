using Dynamic.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelUser : T_User
    {
        public List<ModelUserRole> UserRoles { get; set; }

        public DateTime HireDate { get; set; }
        public string Position { get; set; }

        public string PositionName { get; set; }

        public string stringRoleName { get; set; }

        public List<int> ListRoleId { get; set; } 
        public List<SalaryObj> SalaryObj { get; set; }
    }
    public class Subscription
    {
        public Guid Id { get; set; }
        public string endpoint { get; set; }
        public Keys keys { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string OsName { get; set; }
        public string OsVersion { get; set; }
        public DateTime DateCreated { get; set; }
        public bool DailyNotification { get; set; }
        public bool IndividualLeads { get; set; }
        public bool VendorContact { get; set; }
    }
    public class PushNotificationSubscribe
    {
        public Subscription Subscription { get; set; }
        public string oldEndPoint { get; set; }
        public int Type { get; set; }
    }

    public partial class Notification 
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Endpoint { get; set; }
        public Keys Keys { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string OsName { get; set; }
        public string OsVersion { get; set; }
        public bool Unsubscribed { get; set; }
    }

    public class SalaryObj
    {
        public Guid? Id { get; set; }
        [System.ComponentModel.DefaultValue(1)]
        public int? Index { get; set; }
        public string Content { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public double? Amount { get; set; }
        public string Unit { get; set; }
    }
}
