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
VINASIC.namespace("MaterialType");
VINASIC.MaterialType = function () {
    var global = {
        UrlAction: {
            GetListMaterialType: "/MaterialType/GetMaterialTypes",
            SaveMaterialType: "/MaterialType/SaveMaterialType",
            DeleteMaterialType: "/MaterialType/DeleteMaterialType"
        },
        Element: {
            JtableMaterialType: "jtableMaterialType",
            PopupMaterialType: "popup_MaterialType",
            PopupSearch: "popup_SearchMaterialType"
        },
        Data: {
            ModelMaterialType: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListMaterialType() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableMaterialType).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(materialType) {
        var materialTypeViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: ""
        };
        if (materialType != null) {
            materialTypeViewModel = {
                Id: ko.observable(materialType.Id),
                Code: ko.observable(materialType.Code),
                Name: ko.observable(materialType.Name),
                Description: ko.observable(materialType.Description)
            };
        }
        return materialTypeViewModel;
    }
    function bindData(materialType) {
        global.Data.ModelMaterialType = initViewModel(materialType);
        ko.applyBindings(global.Data.ModelMaterialType);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupMaterialType() {
        $("#" + global.Element.PopupMaterialType).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteMaterialType,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListMaterialType();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupMaterialType, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListMaterialType() {
        $("#" + global.Element.JtableMaterialType).jtable({
            title: "Danh sách Loại Dịch Vụ",
            paging: true,
            pageSize: 10,
            pageSizeChangeMaterialType: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListMaterialType,
                createAction: global.Element.PopupMaterialType,
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
                            showPopupMaterialType();
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
                                realTimeHub.server.sendUpdateEvent("jtableMaterialType");
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
    function saveMaterialType() {
        $.ajax({
            url: global.UrlAction.SaveMaterialType,
            type: 'post',
            data: ko.toJSON(global.Data.ModelMaterialType),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListMaterialType();
                        $("#" + global.Element.PopupMaterialType).modal("hide");
                        toastr.success('Thanh Cong');
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtableMaterialType", global.Data.ClientId, "Cập nhật Loại Vật Tư");
                        $.connection.hub.start();
                    }
                }, false, global.Element.PopupMaterialType, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupMaterialType() {
        $("#" + global.Element.PopupMaterialType).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupMaterialType + " button[save]").click(function () {
            saveMaterialType();
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableMaterialType", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupMaterialType + " button[cancel]").click(function () {
            $("#" + global.Element.PopupMaterialType).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListMaterialType = function () {
        reloadListMaterialType();
    };
    this.initViewModel = function (materialType) {
        initViewModel(materialType);
    };
    this.bindData = function (materialType) {
        bindData(materialType);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListMaterialType();
        reloadListMaterialType();
        initPopupMaterialType();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var materialType = new VINASIC.MaterialType();
    materialType.Init();
});