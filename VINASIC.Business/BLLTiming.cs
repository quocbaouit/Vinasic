using System;
using System.Collections.Generic;
using System.Linq;
using GPRO.Ultilities;
using Dynamic.Framework;
using Dynamic.Framework.Infrastructure.Data;
using Dynamic.Framework.Mvc;
using PagedList;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
using VINASIC.Data.Repositories;
using VINASIC.Object;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace VINASIC.Business
{
    public class BllTiming : IBllTiming
    {
        private readonly IT_TimingRepository _repTiming;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllTiming(IUnitOfWork<VINASICEntities> unitOfWork, IT_TimingRepository repTiming)
        {
            _unitOfWork = unitOfWork;
            _repTiming = repTiming;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }
        public List<ModelTiming> GetListProduct()
        {
            var timing = _repTiming.GetMany(c => !c.IsDeleted).Select(c => new ModelTiming()
            {
                Id = c.Id,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return timing;
        }

        public ResponseBase Create(ModelTiming obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {

                    var timing = new T_Timing();
                    Parse.CopyObject(obj, ref timing);
                    timing.CreatedDate = DateTime.Now.AddHours(14);
                    _repTiming.Add(timing);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Timing", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create Timing", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelTiming obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };

            T_Timing timing = _repTiming.Get(x => x.Id == obj.Id && !x.IsDeleted);
            if (timing != null)
            {
                timing.UpdatedDate = DateTime.Now.AddHours(14);
                timing.UpdatedUser = obj.UpdatedUser;
                _repTiming.Update(timing);
                SaveChange();
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateTiming", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
            }

            return result;
        }

        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var timing = _repTiming.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (timing != null)
            {
                timing.IsDeleted = true;
                timing.DeletedUser = userId;
                timing.DeletedDate = DateTime.Now.AddHours(14);
                _repTiming.Update(timing);
                SaveChange();
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Delete", Message = "Đối Tượng Đã Bị Xóa,Vui Lòng Kiểm Tra Lại" });
            }
            return responResult;
        }
        public List<ModelSelectItem> GetListTiming()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Timing----"}
            };
            listModelSelect.AddRange(_repTiming.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.EmployeeName }));
            return listModelSelect;
        }
        public PagedList<ModelTiming> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var timings = _repTiming.GetMany(c => !c.IsDeleted).Select(c => new ModelTiming()
            {
                Id = c.Id,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelTiming>(timings, pageNumber, pageSize);
        }
        public List<DiaryEvent> LoadAppointmentSummaryInDateRange(double start, double end,int id)
        {

            var fromDate = ConvertFromUnixTimestamp(start);
            var month = fromDate.AddDays(20).Month;
            var year = fromDate.AddDays(20).Year;
            var toDate = ConvertFromUnixTimestamp(end);
            List<DiaryEvent> result = new List<DiaryEvent>();
            var rslt = _repTiming.Get(x => x.TimingMonth == month && x.EmployeeId == id && x.TimingYear == year);
            if (rslt != null)
            {
                var Id = rslt.Id;
                foreach (PropertyInfo property in rslt.GetType().GetProperties())
                {
                    DiaryEvent rec = new DiaryEvent();

                    if (property.Name.Contains("Day"))
                    {
                        var data = property.GetValue(rslt, null);
                        if (data == null)
                        {
                            continue;
                        }
                        else
                        {
                            string number = Regex.Match(property.Name, @"\d+").Value;
                            int day = int.Parse(number);
                            var dateTimeScheduled = new DateTime(year, month, day);
                            rec.ID = Id + day;
                            string StringDate = string.Format("{0:yyyy-MM-dd}", dateTimeScheduled);
                            rec.StartDateString = StringDate; //ISO 8601 format
                            rec.EndDateString = StringDate;
                            //rec.SomeImportantKeyID = -1;
                            rec.Title = data.ToString();
                            result.Add(rec);
                        }

                    }
                }
            }
            return result;
        }
        public T_Timing ReCreateTimming(int empId, string empName, int year, int month)
        {
            T_Timing timing = new T_Timing();
            timing.EmployeeId = empId;
            timing.EmployeeName = empName;
            timing.TimingYear = year;
            timing.TimingMonth = month;
            timing.CreatedUser = 1;
            timing.CreatedDate = DateTime.Now.AddHours(14);
            _repTiming.Add(timing);
            SaveChange();
            return timing;
        }

        public double GetTimingForEmployee(int imployId)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var exitTiming = _repTiming.Get(x => x.TimingMonth == month && x.EmployeeId == imployId && x.TimingYear == year);
            if (exitTiming !=null)
            {
                double offDate = 0;
                if (exitTiming.Day1 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day1 == "1")
                {
                    offDate = offDate + 1;
                }

                if (exitTiming.Day2 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day2 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day3 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day3 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day4 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day4 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day5 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day5 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day6 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day6 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day7 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day7 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day8 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day8 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day9 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day9 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day10 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day10 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day11 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day11 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day12 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day12 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day13 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day13 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day14 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day14 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day15 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day15 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day16 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day16 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day17 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day17 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day18 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day18 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day19 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day19 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day20 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day20 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day21 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day21 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day22 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day22 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day23 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day23 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day24 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day24 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day25 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day25 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day26 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day26 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day27 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day27 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day28 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day28 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day29 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day29 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day30 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day30 == "1")
                {
                    offDate = offDate + 1;
                }
                if (exitTiming.Day31 == "0.5")
                {
                    offDate = offDate + 0.5;
                }
                if (exitTiming.Day31 == "1")
                {
                    offDate = offDate + 1;
                }
                return offDate;
            }
            return 0;        
        }
        public bool CreateNewEvent(string Title, string NewEventDate, string NewEventTime, string NewEventDuration,int id)
        {
            try
            {
                if(string.IsNullOrEmpty(Title))
                {
                    Title = null;
                }
                var fullDate = DateTime.ParseExact(NewEventDate + " " + NewEventTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var date = fullDate.Day;
                var month = fullDate.Month;
                var year = fullDate.Year;
                var exitTiming = _repTiming.Get(x => x.TimingMonth == month && x.EmployeeId == id && x.TimingYear == year);
                if (exitTiming == null)
                {
                    exitTiming = ReCreateTimming(id, "Employee", year, month);
                }
                var availbleAtribute = exitTiming.GetType().GetProperties().Where(x => x.Name == ("Day" + date.ToString())).FirstOrDefault();
                availbleAtribute.SetValue(exitTiming, Title);
                exitTiming.UpdatedDate = DateTime.Now.AddHours(14);
                exitTiming.UpdatedUser = 1;
                _repTiming.Update(exitTiming);
                SaveChange();


            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
}

