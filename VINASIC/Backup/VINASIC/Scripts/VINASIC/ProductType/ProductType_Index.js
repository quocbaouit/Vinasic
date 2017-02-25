if (typeof VINASIC == 'undefined' || !VINASIC) {
    var VINASIC = {};
}

VINASIC.namespace = function () {
    var a = arguments,
        o = null,
        i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = ('' + a[i]).split('.');
        o = VINASIC;
        for (j = (d[0] == 'VINASIC') ? 1 : 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
}
VINASIC.namespace('ProductType');
VINASIC.ProductType = function () {
    var Global = {
        UrlAction: {
            GetListProductType: '/Exam/GetProductTypes',
            SaveProductType: '/Exam/SaveProductType',
            DeleteProductType: '/Exam/DeleteProductType',
        },
        Element: {
            JtableProductType: 'jtableProductType',
            PopupProductType: 'popup_ProductType',
            PopupSearch: 'popup_SearchProductType'
        },
        Data: {
            ModelProductType: {},
            ModelConfig: {},
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        InitListProductType();
        ReloadListProductType();
        InitPopupProductType();
        InitPopupSearch();
        BindData(null);
    }

    this.reloadListProductType = function () {
        ReloadListProductType();
    }

    this.initViewModel = function (ProductType) {
        InitViewModel(ProductType);
    }

    this.bindData = function (ProductType) {
        BindData(ProductType);
    }

    var RegisterEvent = function () {
        $('[cancel]').click(function () {
            BindData(null);
        });
    }

    function InitViewModel(ProductType) {
        var ProductTypeViewModel = {
            Id: 0,
            Code: '',
            Name: '',
            Description: '',
        };
        if (ProductType != null) {
            ProductTypeViewModel = {
                Id: ko.observable(ProductType.Id),
                Code: ko.observable(ProductType.Code),
                Name: ko.observable(ProductType.Name),
                Description: ko.observable(ProductType.Description),
            };
        }
        return ProductTypeViewModel;
    }
    function BindData(ProductType) {
        Global.Data.ModelProductType = InitViewModel(ProductType);
        ko.applyBindings(Global.Data.ModelProductType);
    }
    function SaveProductType() {
        $.ajax({
            url: Global.UrlAction.SaveProductType,
            type: 'post',
            data: ko.toJSON(Global.Data.ModelProductType),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        BindData(null);
                        ReloadListProductType();
                        $("#" + Global.Element.PopupProductType).modal("hide");
                        GlobalCommon.ShowConfirmDialog('Thành Công', function () {
                        }, function () { }, 'OK');
                    }
                }, false, Global.Element.PopupProductType, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }

    function InitListProductType() {
        $('#' + Global.Element.JtableProductType).jtable({
            title: 'Danh sách Loại Sản Phẩm',
            paging: true,
            pageSize: 10,
            pageSizeChangeProductType: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: Global.UrlAction.GetListProductType,
                createAction: Global.Element.PopupProductType,
                createObjDefault: InitViewModel(null),
                searchAction: Global.Element.PopupSearch,
            },
            messages: {
                addNewRecord: 'Thêm mới',
                searchRecord: 'Tìm kiếm',
                selectShow: 'Ẩn hiện cột'
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
                    width: "5%",
                },
                Name: {
                    visibility: 'fixed',
                    title: "Tên Loại",
                    width: "25%",
                    display: function (data) {
                        var text = $('<a class="clickable" title="Chỉnh sửa thông tin.">' + data.record.Name + '</a>');
                        text.click(function () {
                            BindData(data.record);
                            ShowPopupProductType();
                        });
                        return text;
                    }
                },
                Description: {
                    title: "Mô Tả",
                    width: "25%",
                },
                Delete: {
                    title: 'Xóa',
                    width: "5%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                Delete(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;

                    }
                }
            }
        });
    }

    function ReloadListProductType() {
        var keySearch = $('#txtSearch').val();
        $('#' + Global.Element.JtableProductType).jtable('load', { 'keyword': keySearch });
    }

    function Delete(Id) {
        $.ajax({
            url: Global.UrlAction.DeleteProductType,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadListProductType();
                    }
                }, false, Global.Element.PopupProductType, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    function InitPopupProductType() {
        $("#" + Global.Element.PopupProductType).modal({
            keyboard: false,
            show: false
        });
        $("#" + Global.Element.PopupProductType + ' button[save]').click(function () {
            if (CheckValidate()) {
                SaveProductType();
            }
        });
        $("#" + Global.Element.PopupProductType + ' button[cancel]').click(function () {
            $("#" + Global.Element.PopupProductType).modal("hide");
        });
    }

    function InitPopupSearch() {
        $("#" + Global.Element.PopupSearch).modal({
            keyboard: false,
            show: false
        });
        $("#" + Global.Element.PopupSearch + ' button[save]').click(function () {
            ReloadListProductType();
        });
        $("#" + Global.Element.PopupSearch + ' button[cancel]').click(function () {
            $("#" + Global.Element.PopupSearch).modal("hide");
        });
    }

    function ShowPopupProductType() {
        $('#' + Global.Element.PopupProductType).modal('show');
    }
    function CheckValidate() {
        if ($('#Name').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên Ngân Hàng.", function () { }, "Lỗi Nhập liệu");
            $('#Name').focus();
            return false;
        }
        return true;
    }
}
$(document).ready(function () {
    var ProductType = new VINASIC.ProductType();
    ProductType.Init();
})
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode == 59 || charCode == 46)
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
    { GlobalCommon.ShowMessageDialog("Vui lòng nhập số.", function () { }, "Lỗi Nhập liệu"); }
}