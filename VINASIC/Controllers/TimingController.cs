using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VINASIC.Business.Interface;

namespace VINASIC.Controllers
{
    public class TimingController : BaseController
    {
        //
        // GET: /Timing/
        private readonly IBllTiming _bllTiming;
        public TimingController(IBllTiming bllTiming)
        {
            _bllTiming = bllTiming;
        }
        public ActionResult Index()
        {
            return View();
        }

        //public void UpdateEvent(int id, string NewEventStart, string NewEventEnd)
        //{
        //    DiaryEvent.UpdateDiaryEvent(id, NewEventStart, NewEventEnd);
        //}


        public bool SaveEvent(string Title, string NewEventDate, string NewEventTime, string NewEventDuration,int EmpId)
        {
            return _bllTiming.CreateNewEvent(Title, NewEventDate, NewEventTime, NewEventDuration, EmpId);
        }

        public JsonResult GetDiarySummary(double start, double end,int empId)
        {
            if (empId == 0)
            {
                empId = UserContext.UserID;
            }
            var ApptListForDate = _bllTiming.LoadAppointmentSummaryInDateRange(start, end, empId);
            var eventList = from e in ApptListForDate
                            select new
                            {
                                id = e.ID,
                                title = e.Title,
                                start = e.StartDateString,
                                end = e.EndDateString,
                                someKey = e.SomeImportantKeyID,
                                allDay = true
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
    }
}
