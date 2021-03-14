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
VINASIC.namespace("ProductUnit");
VINASIC.ProductUnit = function () {
    var global = {
        UrlAction: {
            GetListProductUnit: "/ProductUnit/GetProductUnits",
            SaveProductUnit: "/ProductUnit/SaveProductUnit",
            DeleteProductUnit: "/ProductUnit/DeleteProductUnit"
        },
        Element: {
            JtableProductUnit: "jtableProductUnit",
            PopupProductUnit: "popup_ProductUnit",
            PopupSearch: "popup_SearchProductUnit"
        },
        Data: {
            ModelProductUnit: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListProductUnit() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableProductUnit).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(productUnit) {
        var productUnitViewModel = {
            Id: 0,
            IsShowDim: false,
            Name: "",
            Description: ""
        };
        if (productUnit != null) {
            productUnitViewModel = {
                Id: ko.observable(productUnit.Id),
                IsShowDim: ko.observable(productUnit.IsShowDim),
                Name: ko.observable(productUnit.Name),
                Description: ko.observable(productUnit.Description)
            };
        }
        return productUnitViewModel;
    }
    function bindData(productUnit) {
        global.Data.ModelProductUnit = initViewModel(productUnit);
        ko.applyBindings(global.Data.ModelProductUnit);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupProductUnit() {
        $("#" + global.Element.PopupProductUnit).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteProductUnit,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListProductUnit();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupProductUnit, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListProductUnit() {
        $("#" + global.Element.JtableProductUnit).jtable({
            title: "Danh sách đơn vị tính",
            paging: true,
            pageSize: 10,
            pageSizeChangeProductUnit: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListProductUnit,
                createAction: global.Element.PopupProductUnit,
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
                IsShowDim: {
                    title: "Hiển Thị Kích Thước",
                    width: "5%",
                    display: function (data) {
                        var elementDisplay = "";
                        if (data.record.IsShowDim) { elementDisplay = "<input  type='checkbox' checked='checked' disabled/>"; }
                        else {
                            elementDisplay = "<input  type='checkbox' disabled />";
                        }
                        return elementDisplay;
                    }
                },
                Name: {
                    visibility: "fixed",
                    title: "Tên",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupProductUnit();
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
                                realTimeHub.server.sendUpdateEvent("jtableProductUnit");
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
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên.", function () { }, "Lỗi Nhập liệu");
            $("#Name").focus();
            return false;
        }
        return true;
    }
    /*End Check Validate */

    /*function Save */
    function saveProductUnit() {
        if (checkValidate) {
            $.ajax({
                url: global.UrlAction.SaveProductUnit,
                type: 'post',
                data: ko.toJSON(global.Data.ModelProductUnit),
                contentType: 'application/json',
                success: function (result) {
                    $('#loading').hide();
                    GlobalCommon.CallbackProcess(result, function () {
                        if (result.Result === "OK") {
                            bindData(null);
                            reloadListProductUnit();
                            $("#" + global.Element.PopupProductUnit).modal("hide");
                            toastr.success('Thành Công');
                        }
                    }, false, global.Element.PopupProductUnit, true, true, function () {
                        var msg = GlobalCommon.GetErrorMessage(result);
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                    });
                }
            });
        }
        
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupProductUnit() {
        $("#" + global.Element.PopupProductUnit).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupProductUnit + " button[save]").click(function () {
            saveProductUnit();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableProductUnit", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupProductUnit + " button[cancel]").click(function () {
            $("#" + global.Element.PopupProductUnit).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListProductUnit = function () {
        reloadListProductUnit();
    };
    this.initViewModel = function (productUnit) {
        initViewModel(productUnit);
    };
    this.bindData = function (productUnit) {
        bindData(productUnit);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListProductUnit();
        reloadListProductUnit();
        initPopupProductUnit();       
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var productUnit = new VINASIC.ProductUnit();
    productUnit.Init();
});