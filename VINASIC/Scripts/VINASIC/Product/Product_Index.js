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
VINASIC.namespace("Product");
VINASIC.Product = function () {
    var global = {
        UrlAction: {
            GetListProduct: "/Product/GetProducts",
            SaveProduct: "/Product/SaveProduct",
            DeleteProduct: "/Product/DeleteProduct"
        },
        Element: {
            JtableProduct: "jtableProduct",
            PopupProduct: "popup_Product",
            PopupSearch: "popup_SearchProduct"
        },
        Data: {
            ModelProduct: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListProduct() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableProduct).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(product) {
        var productViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: "",
            OrderIndex: 0,
            ProductPrice: 0,

        };
        if (product != null) {
            productViewModel = {
                Id: ko.observable(product.Id),
                Code: ko.observable(product.Code),
                Name: ko.observable(product.Name),
                Description: ko.observable(product.Description),
                OrderIndex: ko.observable(product.OrderIndex),
                ProductPrice: ko.observable(product.ProductPrice)
            };
        }
        return productViewModel;
    }
    function bindData(product) {
        global.Data.ModelProduct = initViewModel(product);
        ko.applyBindings(global.Data.ModelProduct);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupProduct() {
        $("#" + global.Element.PopupProduct).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteProduct,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListProduct();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupProduct, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */
    function initComboBoxProductType() {
        var url = "/Product/GetProductType";
        $.getJSON(url, function (datas) {
            $('#productType').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#productType').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#productType').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    function initComboBoxProductUnit() {
        var url = "/Product/GetProductUnit";
        $.getJSON(url, function (datas) {
            $('#productUnit').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#productUnit').append('<option value="' + datas[i].Text + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#productUnit').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    /*function Init List Using Jtable */
    function initListProduct() {
        $("#" + global.Element.JtableProduct).jtable({
            title: "Danh sách Dịch Vụ",
            paging: true,
            pageSize: 10,
            pageSizeChangeProduct: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListProduct,
                createAction: global.Element.PopupProduct,
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
                    title: "Mã Dịch Vụ",
                    width: "5%"
                },
                Name: {
                    visibility: "fixed",
                    title: "Tên Dịch Vụ",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            debugger;
                            $('#productType').val(data.record.ProductTypeId);
                            $('#productUnit').val(data.record.Unit);
                            var isShowDimCheck = data.record.Unit;
                            $('#IsShowDim').prop('checked', isShowDimCheck);
                            bindData(data.record);
                            showPopupProduct();
                        });
                        return text;
                    }
                },
                ProductTypeName: {
                    title: "Tên Danh Mục",
                    width: "25%"
                },
               
                ProductPrice: {
                    title: "Đơn Giá",
                    width: "15%"
                },
                Unit: {
                    title: "Đơn Vị Tính",
                    width: "15%"
                },
                Description: {
                    title: "Mô Tả",
                    width: "20%"
                },

                IsShowDim: {
                    title: "Hiển Thị Kích Thước",
                    width: "20%",
                    display: function (data) {
                        var elementDisplay = "";
                        if (data.record.IsShowDim) { elementDisplay = "<p>có</p>"; }
                        else {
                            elementDisplay = "<p>không</p>";
                        }
                        return elementDisplay;
                    }
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
                                realTimeHub.server.sendUpdateEvent("jtableProduct");
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
    function saveProduct() {
        debugger;
        global.Data.ModelProduct.ProductTypeId = $('#productType').val();
        global.Data.ModelProduct.Unit = $('#productUnit').val();
        global.Data.ModelProduct.IsShowDim = $('#IsShowDim').val() == 'on' ? true : false;
        $.ajax({
            url: global.UrlAction.SaveProduct,
            type: 'post',
            data: ko.toJSON(global.Data.ModelProduct),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListProduct();
                        $("#" + global.Element.PopupProduct).modal("hide");
                        toastr.success('Thàng Công');
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtableProduct", global.Data.ClientId, "Cập nhật Dịch Vụ");
                        $.connection.hub.start();
                    }
                }, false, global.Element.PopupProduct, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupProduct() {
        $("#" + global.Element.PopupProduct).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupProduct + " button[save]").click(function () {
            saveProduct();
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableProduct", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupProduct + " button[cancel]").click(function () {
            $("#" + global.Element.PopupProduct).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListProduct = function () {
        reloadListProduct();
    };
    this.initViewModel = function (product) {
        initViewModel(product);
    };
    this.bindData = function (product) {
        bindData(product);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initComboBoxProductType();
        initComboBoxProductUnit();
        initListProduct();
        reloadListProduct();
        initPopupProduct();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var product = new VINASIC.Product();
    product.Init();
});