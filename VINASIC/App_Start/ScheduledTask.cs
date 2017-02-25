using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNet.SignalR;
using Quartz;
using Quartz.Impl;
using VINASIC.Hubs;
namespace VINASIC
{
    public class ScheduledTask
    {
        public static void Start()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            var job = JobBuilder.Create<EmailJob>().Build();

            //var trigger = TriggerBuilder.Create()
            //    .WithDailyTimeIntervalSchedule
            //      (s =>
            //         s.WithIntervalInHours(24)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
            //      )
            //    .Build();
            var trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(10)
            .RepeatForever())
            .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public class EmailJob : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                var context1 = GlobalHost.ConnectionManager.GetHubContext<RealTimeJTableDemoHub>();
                context1.Clients.All.GetUpdateEvent("jtableProductType", "quocbao", "Test");
                //context1.Clients.All.addMessage(message);
                //var real =new RealTimeJTableDemoHub();
                //real.SendUpdateEvent("JtableProductType","quocbao","test Schedule");
                //using (var message = new MailMessage("quocbao.uit@gmail.com", "quocbao.uit@gmail.com"))
                //{
                //    message.Subject = "Test";
                //    message.Body = "Test at " + DateTime.Now;
                //    using (var client = new SmtpClient
                //    {
                //        EnableSsl = true,
                //        Host = "smtp.gmail.com",
                //        Port = 587,
                //        Credentials = new NetworkCredential("quocbao.uit@gmail.com", "tranquocbaouit@@@")
                //    })
                //    {
                //        client.Send(message);
                //    }
                //}
            }
        }
    }
}