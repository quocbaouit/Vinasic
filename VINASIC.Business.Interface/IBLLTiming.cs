using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllTiming
    {
        ResponseBase Create(ModelTiming obj);
        ResponseBase Update(ModelTiming obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelTiming> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListTiming();
        List<ModelTiming> GetListProduct();
        List<DiaryEvent> LoadAppointmentSummaryInDateRange(double start, double end, int id);

        bool CreateNewEvent(string Title, string NewEventDate, string NewEventTime, string NewEventDuration, int id);
        double GetTimingForEmployee(int imployId);
    }
}
