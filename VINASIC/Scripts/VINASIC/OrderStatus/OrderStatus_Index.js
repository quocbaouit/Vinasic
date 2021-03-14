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
VINASIC.namespace("OrderStatus");
VINASIC.OrderStatus = function () {
    var global = {
        UrlAction: {
            GetListOrderStatus: "/OrderStatus/GetOrderStatuss",
            SaveOrderStatus: "/OrderStatus/SaveOrderStatus",
            DeleteOrderStatus: "/OrderStatus/DeleteOrderStatus"
        },
        Element: {
            JtableOrderStatus: "jtableOrderStatus",
            PopupOrderStatus: "popup_OrderStatus",
            PopupSearch: "popup_SearchOrderStatus"
        },
        Data: {
            ModelOrderStatus: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListOrderStatus() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableOrderStatus).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(orderStatus) {
        var orderStatusViewModel = {
            Id: 0,
            Code: "",
            StatusName: "",
            Description: "",
            ColorCode: "#fdfd08"
        };
        if (orderStatus != null) {
            orderStatusViewModel = {
                Id: ko.observable(orderStatus.Id),
                StatusName: ko.observable(orderStatus.StatusName),
                Description: ko.observable(orderStatus.Description),
                ColorCode: ko.observable(orderStatus.ColorCode)
            };
        }
        return orderStatusViewModel;
    }
    function bindData(orderStatus) {
        global.Data.ModelOrderStatus = initViewModel(orderStatus);
        ko.applyBindings(global.Data.ModelOrderStatus);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupOrderStatus() {
        $("#" + global.Element.PopupOrderStatus).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteOrderStatus,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListOrderStatus();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupOrderStatus, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListOrderStatus() {
        $("#" + global.Element.JtableOrderStatus).jtable({
            title: "Trạng Thái Đơn Hàng",
            paging: true,
            pageSize: 10,
            pageSizeChangeOrderStatus: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListOrderStatus,
                createAction: global.Element.PopupOrderStatus,
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
                            showPopupOrderStatus();
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
                                realTimeHub.server.sendUpdateEvent("jtableOrderStatus");
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
    function saveOrderStatus() {
        if (checkValidate()) {
            $.ajax({
                url: global.UrlAction.SaveOrderStatus,
                type: 'post',
                data: ko.toJSON(global.Data.ModelOrderStatus),
                contentType: 'application/json',
                success: function (result) {
                    $('#loading').hide();
                    GlobalCommon.CallbackProcess(result, function () {
                        if (result.Result === "OK") {
                            bindData(null);
                            reloadListOrderStatus();
                            $("#" + global.Element.PopupOrderStatus).modal("hide");
                            toastr.success('Thành Công');
                        }
                    }, false, global.Element.PopupOrderStatus, true, true, function () {
                        var msg = GlobalCommon.GetErrorMessage(result);
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                    });
                }
            });
        }
        
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupOrderStatus() {
        $("#" + global.Element.PopupOrderStatus).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupOrderStatus + " button[save]").click(function () {
            saveOrderStatus();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableOrderStatus", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupOrderStatus + " button[cancel]").click(function () {
            $("#" + global.Element.PopupOrderStatus).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListOrderStatus = function () {
        reloadListOrderStatus();
    };
    this.initViewModel = function (orderStatus) {
        initViewModel(orderStatus);
    };
    this.bindData = function (orderStatus) {
        bindData(orderStatus);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListOrderStatus();
        reloadListOrderStatus();
        initPopupOrderStatus();       
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var orderStatus = new VINASIC.OrderStatus();
    orderStatus.Init();
});