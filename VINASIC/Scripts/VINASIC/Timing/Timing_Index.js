if (typeof VINASIC == "undefined" || !VINASIC) {
    var VINASIC = {};
}
VINASIC.namespace = function () {
    var a = arguments,
        o = null,
        i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = ("" + a[i]).split(".");
        o = VINASIC;
        for (j = (d[0] === "VINASIC") ? 1 : 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
};
VINASIC.namespace("Timing");
VINASIC.Timing = function () {
    var global = {
        UrlAction: {
            GetListTiming: "/Timing/GetTimings",
            SaveTiming: "/Timing/SaveTiming",
            DeleteTiming: "/Timing/DeleteTiming"
        },
        Element: {
            JtableTiming: "jtableTiming",
            PopupTiming: "popupEventForm"
        },
        Data: {
            ModelTiming: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListTiming() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableTiming).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(timing) {
        var timingViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: ""
        };
        if (timing != null) {
            timingViewModel = {
                Id: ko.observable(timing.Id),
                Code: ko.observable(timing.Code),
                Name: ko.observable(timing.Name),
                Description: ko.observable(timing.Description)
            };
        }
        return timingViewModel;
    }
    function bindData(timing) {
        global.Data.ModelTiming = initViewModel(timing);
        ko.applyBindings(global.Data.ModelTiming);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupTiming() {
        $("#" + global.Element.PopupTiming).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteTiming,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListTiming();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupTiming, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initCalendar() {
        var sourceSummaryView = { url: '/Timing/GetDiarySummary/' };

        $('#calendar1').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month'
            },
            defaultView: 'month',
            editable: true,
            allDaySlot: false,
            selectable: true,
            slotMinutes: 15,
            //events: '/Timing/GetDiarySummary',
            events: {
                url: '/Timing/GetDiarySummary',
                type: 'POST',
                data: {
                    empId: 0,
                },
                error: function() {
                    alert('there was an error while fetching events!');
                },
                color: 'yellow',   // a non-ajax option
                textColor: 'black' // a non-ajax option
            },    
            monthNames: ['Tháng Một', 'Tháng Hai', 'Tháng Ba', 'Tháng Tư', 'Tháng Năm', 'Tháng Sáu', 'Tháng Bảy', 'Tháng Tám', 'Tháng Chín', 'Tháng Mười', 'Tháng Mười Một', 'Tháng Mười Hai'],
            monthNamesShort: ['Một', 'Hai', 'Ba', 'Bốn', 'Năm', 'Sáu', 'Bảy', 'Tám', 'Chín', 'Mười', 'Mười Một', 'Mười Hai'],
            dayNames: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],
            dayNamesShort: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],

            //dayClick: function (date, allDay, jsEvent, view) {
            //    $('#eventTitle').val("");
            //    $('#eventDate').val($.fullCalendar.formatDate(date, 'dd/MM/yyyy'));
            //    $('#eventTime').val($.fullCalendar.formatDate(date, 'HH:mm'));
            //    ShowEventPopup();
            //},
            eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
                    revertFunc();               
            },
            //viewRender: function (view, element) {
            //    $('#calendar1').fullCalendar('removeEventSource', sourceSummaryView);
            //    $('#calendar1').fullCalendar('removeEvents');
            //    $('#calendar1').fullCalendar('addEventSource', sourceSummaryView);
            //}

        });
    }
    /*End init List */

    /*function Check Validate */
    function checkValidate() {
        if ($('#Name').val().trim() === "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên Ngân Hàng.", function () { }, "Lỗi Nhập liệu");
            $("#Name").focus();
            return false;
        }
        return true;
    }
    /*End Check Validate */
    function ShowEventPopup() {
        
        $('#eventTitle').focus();
        showPopupTiming();
    }

    function ClearPopupFormValues() {
        $('#eventID').val("");
        $('#eventTitle').val("");
        $('#eventDateTime').val("");
        $('#eventDuration').val("");
    }

    function UpdateEvent(EventID, EventStart, EventEnd) {

        var dataRow = {
            'ID': EventID,
            'NewEventStart': EventStart,
            'NewEventEnd': EventEnd
        }

        $.ajax({
            type: 'POST',
            url: "/Timing/UpdateEvent",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(dataRow)
        });
    }

    /* Region Register and init bootrap Popup*/
    function initPopupTiming() {
        $("#" + global.Element.PopupTiming).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupTiming + " button[save]").click(function () {
        });
        $("#" + global.Element.PopupTiming + " button[cancel]").click(function () {
            $("#" + global.Element.PopupTiming).modal("hide");
        });
    }
    this.reloadListTiming = function () {
        reloadListTiming();
    };
    this.initViewModel = function (timing) {
        initViewModel(timing);
    };
    this.bindData = function (timing) {
        bindData(timing);
    };
    var registerEvent = function () {
        $(".fc-today-button").click(function () {
            var a = $("#calendar1").fullCalendar('getEventSources');
            alert('Clicked Today!');
        });

        $('#my-next-button').click(function () {
            var date = $("#calendar1").fullCalendar('getDate');
            var month_int = date.getMonth();
            toastr.sucess(month_int);
        });
        $("[cancel]").click(function () {
            bindData(null);
        });
        $('#btnPopupCancel').click(function () {
            ClearPopupFormValues();
            $('#popupEventForm').hide();
        });

        $('#btnPopupSave').click(function () {

            $("#" + global.Element.PopupTiming).modal("hide");

            var dataRow = {
                'Title': $('#eventTitle').val(),
                'NewEventDate': $('#eventDate').val(),
                'NewEventTime': $('#eventTime').val(),
                'NewEventDuration': $('#eventDuration').val()
            }

            ClearPopupFormValues();

            $.ajax({
                type: 'POST',
                url: "/Timing/SaveEvent",
                data: dataRow,
                success: function (response) {
                    if (response == 'True') {

                        $('#calendar1').fullCalendar('refetchEvents');
                        toastr.success('Chấm công thành công');
                    }
                    else {
                        toastr.warning('Chấm công thất bại');
                    }
                }
            });
        });
    };
    this.Init = function () {
        registerEvent();
        initCalendar();
    };
};
/*End Region*/
$(document).ready(function () {
    var timing = new VINASIC.Timing();
    timing.Init();
});