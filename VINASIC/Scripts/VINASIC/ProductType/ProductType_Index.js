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
VINASIC.namespace("ProductType");
VINASIC.ProductType = function () {
    var global = {
        UrlAction: {
            GetListProductType: "/ProductType/GetProductTypes",
            SaveProductType: "/ProductType/SaveProductType",
            DeleteProductType: "/ProductType/DeleteProductType"
        },
        Element: {
            JtableProductType: "jtableProductType",
            PopupProductType: "popup_ProductType",
            PopupSearch: "popup_SearchProductType"
        },
        Data: {
            ModelProductType: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListProductType() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableProductType).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(productType) {
        var productTypeViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: ""
        };
        if (productType != null) {
            productTypeViewModel = {
                Id: ko.observable(productType.Id),
                Code: ko.observable(productType.Code),
                Name: ko.observable(productType.Name),
                Description: ko.observable(productType.Description)
            };
        }
        return productTypeViewModel;
    }
    function bindData(productType) {
        global.Data.ModelProductType = initViewModel(productType);
        ko.applyBindings(global.Data.ModelProductType);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupProductType() {
        $("#" + global.Element.PopupProductType).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteProductType,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListProductType();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupProductType, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListProductType() {
        $("#" + global.Element.JtableProductType).jtable({
            title: "Danh sách Loại Dịch Vụ",
            paging: true,
            pageSize: 10,
            pageSizeChangeProductType: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListProductType,
                createAction: global.Element.PopupProductType,
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
                Code: {
                    title: "Mã Loại",
                    width: "5%"
                },
                Name: {
                    visibility: "fixed",
                    title: "Tên Loại",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupProductType();
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
                                realTimeHub.server.sendUpdateEvent("jtableProductType");
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
    function saveProductType() {
        $.ajax({
            url: global.UrlAction.SaveProductType,
            type: 'post',
            data: ko.toJSON(global.Data.ModelProductType),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListProductType();
                        $("#" + global.Element.PopupProductType).modal("hide");
                        toastr.success('Thành Công');
                    }
                }, false, global.Element.PopupProductType, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupProductType() {
        $("#" + global.Element.PopupProductType).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupProductType + " button[save]").click(function () {
            saveProductType();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableProductType", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupProductType + " button[cancel]").click(function () {
            $("#" + global.Element.PopupProductType).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListProductType = function () {
        reloadListProductType();
    };
    this.initViewModel = function (productType) {
        initViewModel(productType);
    };
    this.bindData = function (productType) {
        bindData(productType);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListProductType();
        reloadListProductType();
        initPopupProductType();       
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var productType = new VINASIC.ProductType();
    productType.Init();
});