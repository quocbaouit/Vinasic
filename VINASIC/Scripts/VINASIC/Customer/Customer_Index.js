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
VINASIC.namespace("Customer");
VINASIC.Customer = function () {
    var global = {
        UrlAction: {
            GetListCustomer: "/Customer/GetCustomers",
            SaveCustomer: "/Customer/SaveCustomer",
            DeleteCustomer: "/Customer/DeleteCustomer"
        },
        Element: {
            JtableCustomer: "jtableCustomer",
            PopupCustomer: "popup_Customer",
            PopupSearch: "popup_SearchCustomer"
        },
        Data: {
            ModelCustomer: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListCustomer() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableCustomer).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(customer) {
        var customerViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: ""
        };
        if (customer != null) {
            customerViewModel = {
                Id: ko.observable(customer.Id),
                Name: ko.observable(customer.Name),
                Address: ko.observable(customer.Address),
                Mobile: ko.observable(customer.Mobile),
                Email: ko.observable(customer.Email),
                TaxCode: ko.observable(customer.TaxCode)
            };
        }
        return customerViewModel;
    }
    function bindData(customer) {
        global.Data.ModelCustomer = initViewModel(customer);
        ko.applyBindings(global.Data.ModelCustomer);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupCustomer() {
        $("#" + global.Element.PopupCustomer).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteCustomer,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListCustomer();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupCustomer, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListCustomer() {
        $("#" + global.Element.JtableCustomer).jtable({
            title: "Danh sách Khách Hàng",
            paging: true,
            pageSize: 10,
            pageSizeChangeCustomer: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListCustomer,
                createAction: global.Element.PopupCustomer,
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
                Name: {
                    visibility: "fixed",
                    title: "Tên Khách Hàng",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupCustomer();
                        });
                        return text;
                    }
                },
                Address: {
                    title: "Địa Chỉ",
                    width: "25%"
                },
                Mobile: {
                    title: "Số Điện Thoại",
                    width: "25%"
                },
                Email: {
                    title: "Email",
                    width: "25%"
                },
                TaxCode: {
                    title: "Mã Số Thuế",
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
                                realTimeHub.server.sendUpdateEvent("jtableCustomer");
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
        if ($('#Name').val().trim() === "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên Ngân Hàng.", function () { }, "Lỗi Nhập liệu");
            $("#Name").focus();
            return false;
        }
        return true;
    }
    /*End Check Validate */

    /*function Save */
    function saveCustomer() {
        $.ajax({
            url: global.UrlAction.SaveCustomer,
            type: 'post',
            data: ko.toJSON(global.Data.ModelCustomer),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListCustomer();
                        $("#" + global.Element.PopupCustomer).modal("hide");
                        toastr.success('Thanh Cong');
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtableCustomer", global.Data.ClientId, "Cập nhật Khách Hàng ");
                        $.connection.hub.start();
                    }
                }, false, global.Element.PopupCustomer, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupCustomer() {
        $("#" + global.Element.PopupCustomer).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupCustomer + " button[save]").click(function () {
            saveCustomer();
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableCustomer", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupCustomer + " button[cancel]").click(function () {
            $("#" + global.Element.PopupCustomer).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListCustomer = function () {
        reloadListCustomer();
    };
    this.initViewModel = function (customer) {
        initViewModel(customer);
    };
    this.bindData = function (customer) {
        bindData(customer);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListCustomer();
        reloadListCustomer();
        initPopupCustomer();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var customer = new VINASIC.Customer();
    customer.Init();
});