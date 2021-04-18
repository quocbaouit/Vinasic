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
VINASIC.namespace("Print");
VINASIC.Print = function () {
    var global = {
        UrlAction: {
            GetListPrint: "/Employee/GetJobForPrint",
            SaveOrderDetail: "/Order/PrintUpdateOrderDetail",
            DeletePrint: "/Employee/DeletePrint"
        },
        Element: {
            JtablePrint: "jtablePrint",
            PopupPrint: "popup_Print"
        },
        Data: {
            ModelPrint: {},
            ModelConfig: {},
            ListEmployeePrint: [],
            ListEmployeePrint: [],
            ListEmployeeAddon: [],
            ClientId: "",
            returnString: "",
            OrderId: 0,
            IdDetailStatus: 0

        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListPrint() {
        var keySearch = $("#keyword").val();
        var fromDate = $("#datefrom").val();
        var toDate = $("#dateto").val();
        var employee = $("#cemployee1").val();
        $("#" + global.Element.JtablePrint).jtable("load", { 'keyword': keySearch, 'fromDate': fromDate, 'toDate': toDate, 'employee': employee });
    }
    /*function init model using knockout Js*/
    function initViewModel(print) {
        var printViewModel = {
            Id: 0,
            FileName: "",
            PrintDescription: ""
        };
        if (print != null) {
            printViewModel = {
                Id: ko.observable(print.Id),
                FileName: ko.observable(print.FileName),
                PrintDescription: ko.observable(print.PrintDescription)
            };
        }
        return printViewModel;
    }
    function bindData(print) {
        global.Data.ModelPrint = initViewModel(print);
        ko.applyBindings(global.Data.ModelPrint);
    }
    /*end function*/
    /*function show Popup*/
    function showPopupPrint() {
        $("#" + global.Element.PopupPrint).modal("show");
    }
    function updateDetailStatus(detailId, status, employeeUpdateId) {
        $.ajax({
            url: "/Order/EmployeeUpdateDetailStatus?detailId=" + detailId + "&status=" + status + "&employeeId=" + employeeUpdateId + "&updateType=" + 2,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListPrint();
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtablePrint", global.Data.ClientId, "Cập nhật in ấn: " + global.Data.returnString + "");
                        $.connection.hub.start();
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    function updateStatus(id, status, returnString) {
        $.ajax({
            url: "/Employee/PrintUpdateOrderDeatail?id=" + id + "&status=" + status,
            type: 'post',
            data: ko.toJSON(global.Data.ModelOrder),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListPrint();
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtableOrder", global.Data.ClientId, "Cập nhật thiết kế: " + returnString + "");
                        $.connection.hub.start();
                        toastr.success("Cập nhật Thành Công");
                    } else {
                        toastr.warning("Đã Thiết Kế Xong");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeletePrint,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListPrint();
                        toastr.success('Xoa Thanh Cong');
                    }
                }, false, global.Element.PopupPrint, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListPrint() {
        $("#" + global.Element.JtablePrint).jtable({
            title: "Danh sách file thiết kế",
            paging: true,
            pageSize: 10,
            pageSizeChangePrint: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListPrint,
                createAction: global.Element.PopupPrint
            },
            messages: {
                selectShow: "Ẩn hiện cột"
            },
            fields: {
                Id: {
                    title: 'Key',
                    width: "5%",
                    key: true,
                    create: false,
                    edit: false,
                    list: true
                },
                CreatedDate: {
                    title: 'Ngày Tạo',
                    width: "5%",
                    type: 'date',
                    displayFormat: 'dd-mm-yy'
                },
                CustomerName: {
                    title: "Tên Khách Hàng",
                    width: "15%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.CustomerName + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupPrint();
                        });
                        return text;
                    }
                },
                CommodityName: {
                    title: "Dịch Vụ",
                    width: "15%"
                },
                FileName: {
                    title: "Tên File",
                    width: "15%"
                },
                Width: {
                    visibility: "fixed",
                    title: "Chiều dài",
                    width: "2%"
                },
                Height: {
                    visibility: "fixed",
                    title: "Chiều Rộng",
                    width: "2%"
                },
                Quantity: {
                    visibility: "fixed",
                    title: "số Lượng",
                    width: "2%"
                },
                //Description: {
                //    title: "Mô Tả",
                //    width: "15%"
                //},
                strJob: {
                    visibility: 'fixed',
                    title: "Công Việc",
                    width: "10%",
                    display: function (data) {
                        var text = $('<a href="javascript:void(0)" class="clickable"  data-target="#popup_Print" title="">' + "Chi Tiết" + '</a>');
                        text.click(function () {
                            $("#view-content").empty();
                            $("#view-content").append(data.record.PrintDescription);
                            showPopupPrint();
                        });
                        return text;
                    }
                },
                //                StrprintStatus: {
                //                    visibility: "fixed",
                //                    title: "Xử Lý",
                //                    width: "20%",
                //                    display: function (data) {
                //                        //var text = $("<a href=\"#\" class=\"clickable\" title=\"Cập nhật Trạng Thái.\">" + data.record.StrprintStatus + "</a>");
                //                        //text.click(function () {
                //                        //    returnString = '<span data-id="'+data.record.OrderId+'" class="viewUpdateDetail">' + data.record.OrderId + '</br>' + data.record.CustomerName + ':' + data.record.Width + '*' + data.record.Height + '-NVKD:' + data.record.EmployeeName;
                //                        //    updateStatus(data.record.Id, data.record.DetailStatus, returnString);
                //                        //});
                //                        var text = "";
                //                        var arrayNVIN = global.Data.ListEmployeePrint;
                //                        var arrayNVGC = global.Data.ListEmployeeAddon;
                //                        var textNVTK = '';
                //                        var textNVIN = '';
                //                        var textNVGC = '';
                //                        var strStatus = '';
                //                        strStatus = getOrderDetailStatus(data.record.DetailStatus);
                //                        if (data.record.DetailStatus == 3) {
                //                            if (data.record.PrintUser == null) { strStatus = 'Đã Thiết Kế Xong'; }
                //                            else {
                //                                strStatus = 'Đã Thiết Kế Xong';
                //                                /*strStatus = 'Đã Chuyển Cho In Ấn:' + data.record.PrintView;*/
                //}
                //                        }
                //                        if (data.record.DetailStatus == 5) { strStatus = 'Đã Chuyển Cho Gia Công:' + data.record.AddOnView; }
                //                        for (var i = 0; i < arrayNVIN.length; i++) {
                //                            textNVIN = textNVIN + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVIN[i].Id + ' class="detailstatus3" href="#">' + arrayNVIN[i].Name + '</a></li>'
                //                        };
                //                        for (var i = 0; i < arrayNVGC.length; i++) {
                //                            textNVGC = textNVGC + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVGC[i].Id + ' class="detailstatus5" href="#">' + arrayNVGC[i].Name + '</a></li>'
                //                        };
                //                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" data-target="#" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chi tiết đơn hàng.\">' + strStatus + '</a></span></button><ul class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"><li class="dropdown"><a tabindex="-1" href="#" class="detailstatus1" href="javascript:void(0)">Đang thiết kế</a></li><li class="dropdown"><a tabindex="-1" href="#" class=" detailstatus2" href="javascript:void(0)">Đã thiết kế xong</a></li></ul></div>');
                //                        text.click(function () {
                //                            global.Data.returnString = '<span data-id="' + data.record.OrderId + '" class="viewUpdateDetail">' + data.record.OrderId + '</br>' + data.record.CustomerName + ':' + data.record.Width + '*' + data.record.Height + '-NVKD:' + data.record.EmployeeName;
                //                            global.Data.OrderId = data.record.OrderId;
                //                            global.Data.IdDetailStatus = data.record.Id;
                //                        });

                //                        return text;
                //                    }
                //                },
                strDetailStatus: {
                    visibility: 'fixed',
                    title: "Xử Lý",
                    width: "10%",
                    display: function (data) {

                        var text = "";
                        var strStatus = data.record.DetailStatusName;
                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Cập nhật trạng thái đơn hàng.\">' + strStatus + '</a>' + resultDetailStatusList + '</div>');
                        text.click(function (e) {
                            global.Data.OrderId = data.record.Id;
                            global.Data.IdDetailStatus = data.record.Id;
                        });
                        return text;
                    }
                },
                EmployeeName: {
                    title: "Nhân Viên Kinh Doanh",
                    width: "20%"
                },
            }
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

    /*function Save */
    function savePrint() {
        $.ajax({
            url: global.UrlAction.SaveOrderDetail,
            type: 'post',
            data: ko.toJSON(global.Data.ModelPrint),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        $("#" + global.Element.PopupPrint).modal("hide");
                        reloadListPrint();
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupPrint, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    function initComboBoxBusiness1() {
        var url = "/Order/GetCustomerByOrganization?shortName=PKD";
        $.getJSON(url, function (datas) {
            $('#cemployee1').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#cemployee1').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
                return datas[0].Value;
            }
            else {
                $('#cemployee1').append('<option value="0">Không Có Dữ Liệu </option>');
                return 0;
            }
        });
    }

    /* Region Register and init bootrap Popup*/
    function initPopupPrint() {
        $("#" + global.Element.PopupPrint).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupPrint + " button[save]").click(function () {
            savePrint();
        });
        $("#" + global.Element.PopupPrint + " button[cancel]").click(function () {
            $("#" + global.Element.PopupPrint).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListPrint = function () {
        reloadListPrint();
    };
    var registerEvent = function () {
        $("[search]").click(function () {
            reloadListPrint();
        });
        $("#search").click(function () {
            reloadListPrint();
        });
        $("body").delegate(".detailstatus1", "click", function (event) {
            event.preventDefault();
            updateDetailStatus(global.Data.IdDetailStatus, 2, 0);
        });
        $("body").delegate(".detailstatus2", "click", function (event) {
            event.preventDefault();
            updateDetailStatus(global.Data.IdDetailStatus, 3, 0);
        });
        $("body").delegate(".detailstatus3", "click", function (event) {
            event.preventDefault();
            updateDetailStatus(global.Data.IdDetailStatus, 3, employeeUpdateId);
        });
        $("body").delegate(".detailstatus5", "click", function (event) {
            event.preventDefault();
            updateDetailStatus(global.Data.IdDetailStatus, 5, employeeUpdateId);
        });
        $("body").delegate(".orderDetailstatus", "click", function (event) {
            var statusId = $(this).attr("data-id");
            event.preventDefault();
            updateDetailStatus(global.Data.IdDetailStatus, statusId, 1);
        });

    };
    this.Init = function () {
        $.ajax({
            url: "/Employee/GetSimpleEmployee",
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == 'OK') {
                        global.Data.ListEmployeePrint = result.Records.printUser;
                        global.Data.ListEmployeePrint = result.Records.printingUser;
                        global.Data.ListEmployeeAddon = result.Records.addOnUser;
                    }

                }, false, global.Element.PopupOrder, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
        setTimeout(function () {
            initComboBoxBusiness1();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            document.getElementById("datefrom").defaultValue = new Date(new Date() - 24 * 60 * 60 * 1000).toISOString().substring(0, 10);
            document.getElementById("dateto").defaultValue = new Date().toISOString().substring(0, 10);
            registerEvent();
            initListPrint();
            reloadListPrint();
            initPopupPrint();
        });
    };
};
this.initViewModel = function (print) {
    initViewModel(print);
};
this.bindData = function (print) {
    bindData(print);
};
var registerEvent = function () {
    $("[cancel]").click(function () {
        bindData(null);
    });
};
/*End Region*/
$(document).ready(function () {
    var employeeUpdateId = 0;
    var print = new VINASIC.Print();
    print.Init();
});
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode === 59 || charCode === 46)
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) { GlobalCommon.ShowMessageDialog("Vui lòng nhập số.", function () { }, "Lỗi Nhập liệu"); }
    return true;
}
function GetdataId(obj) {
    employeeUpdateId = $(obj).data("id");
}