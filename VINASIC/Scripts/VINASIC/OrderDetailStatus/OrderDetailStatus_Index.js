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
VINASIC.namespace("OrderDetailStatus");
VINASIC.OrderDetailStatus = function () {
    var global = {
        UrlAction: {
            GetListOrderDetailStatus: "/OrderDetailStatus/GetOrderDetailStatuss",
            SaveOrderDetailStatus: "/OrderDetailStatus/SaveOrderDetailStatus",
            DeleteOrderDetailStatus: "/OrderDetailStatus/DeleteOrderDetailStatus"
        },
        Element: {
            JtableOrderDetailStatus: "jtableOrderDetailStatus",
            PopupOrderDetailStatus: "popup_OrderDetailStatus",
            PopupSearch: "popup_SearchOrderDetailStatus"
        },
        Data: {
            ModelOrderDetailStatus: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListOrderDetailStatus() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableOrderDetailStatus).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(orderDetailStatus) {
        var orderDetailStatusViewModel = {
            Id: 0,
            Code: "",
            StatusName: "",
            Description: "",
            ColorCode: "#fdfd08"
        };
        if (orderDetailStatus != null) {
            orderDetailStatusViewModel = {
                Id: ko.observable(orderDetailStatus.Id),
                StatusName: ko.observable(orderDetailStatus.StatusName),
                Description: ko.observable(orderDetailStatus.Description),
                ColorCode: ko.observable(orderDetailStatus.ColorCode)
            };
        }
        return orderDetailStatusViewModel;
    }
    function bindData(orderDetailStatus) {
        global.Data.ModelOrderDetailStatus = initViewModel(orderDetailStatus);
        ko.applyBindings(global.Data.ModelOrderDetailStatus);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupOrderDetailStatus() {
        $("#" + global.Element.PopupOrderDetailStatus).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteOrderDetailStatus,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListOrderDetailStatus();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupOrderDetailStatus, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListOrderDetailStatus() {
        $("#" + global.Element.JtableOrderDetailStatus).jtable({
            title: "Trạng Thái Thiết Kế",
            paging: true,
            pageSize: 10,
            pageSizeChangeOrderDetailStatus: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListOrderDetailStatus,
                createAction: global.Element.PopupOrderDetailStatus,
                createObjDefault: initViewModel(null),
                searchAction: global.Element.PopupSearch
            },
            messages: {
                addNewRecord: "Thêm mới",
                searchRecord: "Tìm kiếm",
                selectShow: "Ẩn hiện cột"
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                StatusName: {
                    visibility: "fixed",
                    title: "Tên",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.StatusName + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupOrderDetailStatus();
                        });
                        return text;
                    }
                },
                Description: {
                    title: "Mô Tả",
                    width: "25%"
                },
                Delete: {
                    title: "Xóa",
                    width: "5%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog("Bạn có chắc chắn muốn xóa?", function () {
                                deleteRow(data.record.Id);
                                var realTimeHub = $.connection.realTimeJTableDemoHub;
                                realTimeHub.server.sendUpdateEvent("jtableOrderDetailStatus");
                                $.connection.hub.start();
                            }, function () { }, "Đồng ý", "Hủy bỏ", "Thông báo");
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

        if ($('#StatusName').val().trim() === "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên.", function () { }, "Lỗi Nhập liệu");
            $("#StatusName").focus();
            return false;
        }
        return true;
    }
    /*End Check Validate */

    /*function Save */
    function saveOrderDetailStatus() {
        if (checkValidate()) {
            $.ajax({
                url: global.UrlAction.SaveOrderDetailStatus,
                type: 'post',
                data: ko.toJSON(global.Data.ModelOrderDetailStatus),
                contentType: 'application/json',
                success: function (result) {
                    $('#loading').hide();
                    GlobalCommon.CallbackProcess(result, function () {
                        if (result.Result === "OK") {
                            bindData(null);
                            reloadListOrderDetailStatus();
                            $("#" + global.Element.PopupOrderDetailStatus).modal("hide");
                            toastr.success('Thành Công');
                        }
                    }, false, global.Element.PopupOrderDetailStatus, true, true, function () {
                        var msg = GlobalCommon.GetErrorMessage(result);
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                    });
                }
            });
        }
        
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupOrderDetailStatus() {
        $("#" + global.Element.PopupOrderDetailStatus).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupOrderDetailStatus + " button[save]").click(function () {
            saveOrderDetailStatus();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableOrderDetailStatus", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupOrderDetailStatus + " button[cancel]").click(function () {
            $("#" + global.Element.PopupOrderDetailStatus).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListOrderDetailStatus = function () {
        reloadListOrderDetailStatus();
    };
    this.initViewModel = function (orderDetailStatus) {
        initViewModel(orderDetailStatus);
    };
    this.bindData = function (orderDetailStatus) {
        bindData(orderDetailStatus);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListOrderDetailStatus();
        reloadListOrderDetailStatus();
        initPopupOrderDetailStatus();       
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var orderDetailStatus = new VINASIC.OrderDetailStatus();
    orderDetailStatus.Init();
});