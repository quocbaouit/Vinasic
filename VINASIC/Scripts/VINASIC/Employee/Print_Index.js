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
            SavePrint: "/Employee/SavePrint",
            DeletePrint: "/Employee/DeletePrint"
        },
        Element: {
            JtablePrint: "jtablePrint",
            PopupPrint: "popup_Print"
        },
        Data: {
            ModelPrint: {},
            ModelConfig: {},
            ClientId: ""
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
    /*function show Popup*/
    function showPopupPrint() {
        $("#" + global.Element.PopupPrint).modal("show");
    }
    /*End*/
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
                        realTimeHub.server.sendUpdateEvent("jtableOrder", global.Data.ClientId, "Cập nhật in: " + returnString + "");
                        $.connection.hub.start();
                        toastr.success("Cập nhật Thành Công");
                    } else {
                        toastr.warning("Đã In Xong");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {
                    toastr.error(result.Message);
                });
            }
        });
    }

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
            title: "Danh sách file in ấn/gia công",
            paging: true,
            pageSize: 50,
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
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                CreatedDate: {
                    title: 'Ngày Tạo',
                    width: "10%",
                    type: 'date',
                    displayFormat: 'dd-mm-yy'
                },
                CustomerName: {
                    title: "Tên Khách Hàng",
                    width: "5%"
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
                    width: "5%"
                },
                Height: {
                    visibility: "fixed",
                    title: "Chiều Rộng",
                    width: "5%"
                },
                Quantity: {
                    visibility: "fixed",
                    title: "số Lượng",
                    width: "5%"
                },
                Description: {
                    title: "Mô Tả",
                    width: "10%"
                },
                EmployeeName: {
                    title: "Nhân Viên Kinh Doanh",
                    width: "10%"
                },
                StrPrintStatus: {
                    visibility: "fixed",
                    title: "Xử Lý",
                    width: "15%",
                    display: function (data) {
                        var text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\"Cập nhật Trạng Thái.\">" + data.record.StrPrintStatus + "</a>");
                        text.click(function () {
                            returnString = '<span data-id="' + data.record.OrderId + '" class="viewUpdateDetail">' + data.record.OrderId + '</br>' + data.record.CustomerName + ':' + data.record.Width + '*' + data.record.Height + '-NVKD:' + data.record.EmployeeName;
                            updateStatus(data.record.Id, data.record.DetailStatus, returnString);

                        });
                        return text;
                    }
                }
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
            url: global.UrlAction.SavePrint,
            type: 'post',
            data: ko.toJSON(global.Data.ModelPrint),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        toastr.success("Cập nhật Thành Công");
                    } else {
                        toastr.warning("Đã In Xong");
                    }
                }, false, global.Element.PopupPrint, true, true, function () {
                    toastr.error("Đã có lỗi xảy ra trong quá trình sử lý.");
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
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtablePrint", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
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
        $("#search").click(function () {
            reloadListPrint();
        });
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        initComboBoxBusiness1();
        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
        document.getElementById("datefrom").defaultValue = new Date(new Date() - 24 * 60 * 60 * 1000).toISOString().substring(0, 10);
        document.getElementById("dateto").defaultValue = new Date().toISOString().substring(0, 10);
        registerEvent();
        initListPrint();
        reloadListPrint();
        initPopupPrint();
    };
};
/*End Region*/
$(document).ready(function () {
    var print = new VINASIC.Print();
    print.Init();
});
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode === 59 || charCode === 46)
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
    { GlobalCommon.ShowMessageDialog("Vui lòng nhập số.", function () { }, "Lỗi Nhập liệu"); }
    return true;
}