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
VINASIC.namespace("Design");
VINASIC.Design = function () {
    var global = {
        UrlAction: {
            GetListDesign: "/Employee/GetJobForDesign",
            SaveOrderDetail: "/Order/DesignUpdateOrderDetail",
            DeleteDesign: "/Employee/DeleteDesign"
        },
        Element: {
            JtableDesign: "jtableDesign",
            PopupDesign: "popup_Design"
        },
        Data: {
            ModelDesign: {},
            ModelConfig: {},
            ListEmployeePrint: [],
            ListEmployeeDesign: [],
            ListEmployeeAddon: [],
            ClientId: "",
            returnString:"",
            OrderId: 0,
            IdDetailStatus:0

        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListDesign() {
        var keySearch = $("#keyword").val();
        var fromDate = $("#datefrom").val();
        var toDate = $("#dateto").val();
        var employee = $("#cemployee1").val();
        $("#" + global.Element.JtableDesign).jtable("load", { 'keyword': keySearch, 'fromDate': fromDate, 'toDate': toDate, 'employee': employee });
    }
    /*function init model using knockout Js*/
    function initViewModel(design) {
        var designViewModel = {
            Id: 0,
            FileName: "",
            DesignDescription: ""
        };
        if (design != null) {
            designViewModel = {
                Id: ko.observable(design.Id),
                FileName: ko.observable(design.FileName),
                DesignDescription: ko.observable(design.DesignDescription)
            };
        }
        return designViewModel;
    }
    function bindData(design) {
        global.Data.ModelDesign = initViewModel(design);
        ko.applyBindings(global.Data.ModelDesign);
    }
    /*end function*/
    /*function show Popup*/
    function showPopupDesign() {
        $("#" + global.Element.PopupDesign).modal("show");
    }
    function updateDetailStatus(detailId, status, employeeUpdateId) {
        $.ajax({
            url: "/Order/UpdateDetailStatus?detailId=" + detailId + "&status=" + status + "&employeeId=" + employeeUpdateId,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListDesign();
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtableOrder", global.Data.ClientId, "Cập nhật thiết kế: " + global.Data.returnString + "");
                        $.connection.hub.start();
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    function updateStatus(id, status,returnString) {
        $.ajax({
            url: "/Employee/DesignUpdateOrderDeatail?id=" + id + "&status=" + status,
            type: 'post',
            data: ko.toJSON(global.Data.ModelOrder),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListDesign();
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtableOrder", global.Data.ClientId, "Cập nhật thiết kế: "+returnString+"");
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
            url: global.UrlAction.DeleteDesign,   
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListDesign();
                        toastr.success('Xoa Thanh Cong');
                    }
                }, false, global.Element.PopupDesign, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListDesign() {
        $("#" + global.Element.JtableDesign).jtable({
            title: "Danh sách file thiết kế",
            paging: true,
            pageSize: 10,
            pageSizeChangeDesign: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListDesign,
                createAction: global.Element.PopupDesign
            },
            messages: {
                selectShow: "Ẩn hiện cột"
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
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
                            debugger;
                            bindData(data.record);
                            showPopupDesign();
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
                Description: {
                    title: "Mô Tả",
                    width: "15%"
                },
                StrdesignStatus: {
                    visibility: "fixed",
                    title: "Xử Lý",
                    width: "20%",
                    display: function (data) {
                        //var text = $("<a href=\"#\" class=\"clickable\" title=\"Cập nhật Trạng Thái.\">" + data.record.StrdesignStatus + "</a>");
                        //text.click(function () {
                        //    returnString = '<span data-id="'+data.record.OrderId+'" class="viewUpdateDetail">' + data.record.OrderId + '</br>' + data.record.CustomerName + ':' + data.record.Width + '*' + data.record.Height + '-NVKD:' + data.record.EmployeeName;
                        //    updateStatus(data.record.Id, data.record.DetailStatus, returnString);
                        //});
                        var text = "";
                        var arrayNVIN = global.Data.ListEmployeePrint;
                        var arrayNVGC = global.Data.ListEmployeeAddon;
                        var textNVTK = '';
                        var textNVIN = '';
                        var textNVGC = '';
                        var strStatus = '';
                        strStatus = getOrderDetailStatus(data.record.DetailStatus);
                        if (data.record.DetailStatus == 3) {
                            if (data.record.PrintUser == null) { strStatus = 'Đã Thiết Kế Xong'; }
                            else { strStatus = 'Đã Chuyển Cho In Ấn:' + data.record.PrintView; }
                        }
                        if (data.record.DetailStatus == 5) { strStatus = 'Đã Chuyển Cho Gia Công:' + data.record.AddOnView; }
                        for (var i = 0; i < arrayNVIN.length; i++) {
                            textNVIN = textNVIN + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVIN[i].Id + ' class="detailstatus3" href="#">' + arrayNVIN[i].Name + '</a></li>'
                        };
                        for (var i = 0; i < arrayNVGC.length; i++) {
                            textNVGC = textNVGC + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVGC[i].Id + ' class="detailstatus5" href="#">' + arrayNVGC[i].Name + '</a></li>'
                        };
                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" data-target="#" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chi tiết đơn hàng.\">' + strStatus + '</a></span></button><ul class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"><li class="dropdown"><a tabindex="-1" href="#" class="detailstatus1" href="javascript:void(0)">Đang thiết kế</a></li><li class="dropdown"><a tabindex="-1" href="#" class=" detailstatus2" href="javascript:void(0)">Đã thiết kế xong</a></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">Chuyển cho in ấn</a><ul class="dropdown-menu">' + textNVIN + '</ul></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">chuyển cho gia công</a><ul class="dropdown-menu">' + textNVGC + '</ul></li></ul></div>');
                        text.click(function () {
                            global.Data.returnString = '<span data-id="' + data.record.OrderId + '" class="viewUpdateDetail">' + data.record.OrderId + '</br>' + data.record.CustomerName + ':' + data.record.Width + '*' + data.record.Height + '-NVKD:' + data.record.EmployeeName;
                            global.Data.OrderId = data.record.OrderId;
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
    function saveDesign() {
        $.ajax({
            url: global.UrlAction.SaveOrderDetail,
            type: 'post',
            data: ko.toJSON(global.Data.ModelDesign),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        $("#" + global.Element.PopupDesign).modal("hide");
                        reloadListDesign();
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupDesign, true, true, function () {
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
    function initPopupDesign() {
        $("#" + global.Element.PopupDesign).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupDesign + " button[save]").click(function () {
            saveDesign();
        });
        $("#" + global.Element.PopupDesign + " button[cancel]").click(function () {
            $("#" + global.Element.PopupDesign).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListDesign = function () {
        reloadListDesign();
    };
    var registerEvent = function () {
        $("[search]").click(function () {
            reloadListDesign();
        });
        $("#search").click(function () {
            reloadListDesign();
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

    };
    this.Init = function () {
        $.ajax({
            url: "/Employee/GetSimpleEmployee",
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == 'OK') {
                        global.Data.ListEmployeeDesign = result.Records.designUser;
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
            initListDesign();
            reloadListDesign();
            initPopupDesign();
        });        
    };  
};
this.initViewModel = function (design) {
    initViewModel(design);
};
this.bindData = function (design) {
    bindData(design);
};
var registerEvent = function () {
    $("[cancel]").click(function () {
        bindData(null);
    });
};
/*End Region*/
$(document).ready(function () {
    var employeeUpdateId = 0;
    var design = new VINASIC.Design();
    design.Init();
});
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode === 59 || charCode === 46)
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
    { GlobalCommon.ShowMessageDialog("Vui lòng nhập số.", function () { }, "Lỗi Nhập liệu"); }
    return true;
}
function GetdataId(obj) {
    employeeUpdateId = $(obj).data("id");
}