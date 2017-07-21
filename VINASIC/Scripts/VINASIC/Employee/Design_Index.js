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
            SaveDesign: "/Employee/SaveDesign",
            DeleteDesign: "/Employee/DeleteDesign"
        },
        Element: {
            JtableDesign: "jtableDesign",
            PopupDesign: "popup_Design"
        },
        Data: {
            ModelDesign: {},
            ModelConfig: {},
            ClientId: ""
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
    /*function show Popup*/
    function showPopupDesign() {
        $("#" + global.Element.PopupDesign).modal("show");
    }

    function updateStatus(id, status) {
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
                        realTimeHub.server.sendUpdateEvent("jtableOrder", global.Data.ClientId, "Cập nhật");
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
            title: "Danh sách Loại Dịch Vụ",
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
                    width: "15%"
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
                DesignDescription: {
                    title: "Mô Tả",
                    width: "15%"
                },
                EmployeeName: {
                    title: "Nhân Viên Kinh Doanh",
                    width: "20%"
                },
                StrdesignStatus: {
                    visibility: "fixed",
                    title: "Xử Lý",
                    width: "20%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Cập nhật Trạng Thái.\">" + data.record.StrdesignStatus + "</a>");
                        text.click(function () {
                            updateStatus(data.record.Id, data.record.DetailStatus);
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
    function saveDesign() {
        $.ajax({
            url: global.UrlAction.SaveDesign,
            type: 'post',
            data: ko.toJSON(global.Data.ModelDesign),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtableOrder", global.Data.ClientId, "Cập nhật");
                        $.connection.hub.start();

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
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableDesign", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
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
    };
    this.Init = function () {
        initComboBoxBusiness1();
        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
        document.getElementById("datefrom").defaultValue = new Date(new Date() - 24 * 60 * 60 * 1000).toISOString().substring(0, 10);
        document.getElementById("dateto").defaultValue = new Date().toISOString().substring(0, 10);
        registerEvent();
        initListDesign();
        reloadListDesign();
        initPopupDesign();
    };
};
/*End Region*/
$(document).ready(function () {
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