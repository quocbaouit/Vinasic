using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
//using System.Web.Script.Serialization;

//using log4net;
//using Tiptopweb.Yowsi.Shared.Common;
using WebPush;
//using Tiptopweb.Yowsi.Shared.Poco;
//using ServiceStack;
//using ServiceStack.Data;
//using ServiceStack.OrmLite;

namespace Dynamic.Framework
{
    public class Keys
    {
        public string auth { get; set; }
        public string p256dh { get; set; }
    }
    public class NotificationParam
    {
        public Keys Keys { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string OsName { get; set; }
        public string OsVersion { get; set; }
        public DateTime DateCreated { get; set; }
        public bool DailyNotification { get; set; }
        public bool IndividualLeads { get; set; }
        public bool VendorContact { get; set; }
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
    public class NotificationMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
    }

    public static class PushNotificationHelper
    {
        // Send GMC (Google Messaging Cloud) using Firebase Cloud Messaging
        // we use Web Push C# library
        // https://github.com/web-push-libs/web-push-csharp

        // Send to all the Endpoints
        public static void SendNotification(List<Subscription> endpoint, string strTitle, string strMessage, string stringUrl,int SendId)
        {
            List<Subscription> listInvalidEnpoint = new List<Subscription>();
            try
            {
                if (!endpoint.Any()) return;

                var wpClient = new WebPushClient();

                var notificationMessage = new NotificationMessage
                {
                    Title = strTitle,
                    Message = strMessage,
                    Icon = string.Empty,
                    Url = stringUrl
                };

                // List of Subscriptions for this Company
                // we can have a Subscription for many devices, we have a list of Endpoints
                foreach (Subscription subscription in endpoint)
                {                    
                    try
                    {
                        var pushSubscription = new PushSubscription()
                        {
                            Endpoint = subscription.endpoint,
                            Auth = subscription.keys.auth,
                            P256DH = subscription.keys.p256dh
                        };

                        // payload as a string
                        var payload = new JavaScriptSerializer().Serialize(notificationMessage);

                        var options = new Dictionary<string, object>
                        {
                            ["gcmAPIKey"] = "AIzaSyAWBCODyfd-6OttGn1YdFNF3SIK-U9HdVY",
                            ["TTL"] = 24 * 3600   // 1 day in seconds
                        };

                        wpClient.SendNotification(pushSubscription, payload, options);
                    }
                    catch (Exception ex)
                    {
                        listInvalidEnpoint.Add(subscription);                    
                    }
                }
               // RemoveInvalidEnpointFromCompany(company,listInvalidEnpoint);
            }
            catch (Exception ex)
            {
            }
        }

        // Send to a Specific Endpoint
        public static void SendNotification( Subscription subscription, string strTitle, string strMessage, string stringUrl)
        {
            try
            {
                var wpClient = new WebPushClient();
                var notificationMessage = new NotificationMessage
                {
                    Title = strTitle,
                    Message = strMessage,
                    Icon = string.Empty,
                    Url = stringUrl
                };

                var pushSubscription = new PushSubscription()
                {
                    Endpoint = subscription.endpoint,
                    Auth = subscription.keys.auth,
                    P256DH = subscription.keys.p256dh
                };

                // payload as a string
                var payload = new JavaScriptSerializer().Serialize(notificationMessage);

                var options = new Dictionary<string, object>
                {
                    ["gcmAPIKey"] = "AIzaSyAWBCODyfd-6OttGn1YdFNF3SIK-U9HdVY",
                    ["TTL"] = 24 * 3600   // 1 day in seconds
                };

                wpClient.SendNotification(pushSubscription, payload, options);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendNotificationToCustomer(List<Subscription>endpoints, string strTitle, string strMessage, string stringUrl)
        {
            List<Subscription> listInvalidEnpoint = new List<Subscription>();
            try
            {
                if (!endpoints.Any()) return;

                var wpClient = new WebPushClient();

                var notificationMessage = new NotificationMessage
                {
                    Title = strTitle,
                    Message = strMessage,
                    Icon = string.Empty,
                    Url = stringUrl
                };
                foreach (Subscription subscription in endpoints)
                {
                    if (!subscription.VendorContact) continue;
                    try
                    {
                        var pushSubscription = new PushSubscription()
                        {
                            Endpoint = subscription.endpoint,
                            Auth = subscription.keys.auth,
                            P256DH = subscription.keys.p256dh
                        };

                        // payload as a string
                        var payload = new JavaScriptSerializer().Serialize(notificationMessage);

                        var options = new Dictionary<string, object>
                        {
                            ["gcmAPIKey"] = "AIzaSyAWBCODyfd-6OttGn1YdFNF3SIK-U9HdVY",
                            ["TTL"] = 24 * 3600   // 1 day in seconds
                        };

                        wpClient.SendNotification(pushSubscription, payload, options);
                    }
                    catch (Exception ex)
                    {
                        listInvalidEnpoint.Add(subscription);
                        //mLog.Error("Error sending Web push Notification: ", ex);

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
