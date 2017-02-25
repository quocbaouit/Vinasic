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
VINASIC.namespace("Material");
VINASIC.Material = function () {
    var global = {
        UrlAction: {
            GetListMaterial: "/Material/GetMaterials",
            SaveMaterial: "/Material/SaveMaterial",
            DeleteMaterial: "/Material/DeleteMaterial"
        },
        Element: {
            JtableMaterial: "jtableMaterial",
            PopupMaterial: "popup_Material",
            PopupSearch: "popup_SearchMaterial"
        },
        Data: {
            ModelMaterial: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListMaterial() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableMaterial).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(material) {
        var materialViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: "",
            Inventory: 0,
            MaterialTypeName: ""
        };
        if (material != null) {
            materialViewModel = {
                Id: ko.observable(material.Id),
                Code: ko.observable(material.Code),
                Name: ko.observable(material.Name),
                Description: ko.observable(material.Description),
                Inventory: ko.observable(material.Inventory),
                MaterialTypeName: ko.observable(material.MaterialTypeName)
            };
        }
        return materialViewModel;
    }
    function bindData(material) {
        global.Data.ModelMaterial = initViewModel(material);
        ko.applyBindings(global.Data.ModelMaterial);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupMaterial() {
        $("#" + global.Element.PopupMaterial).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteMaterial,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListMaterial();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupMaterial, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */
    function initComboBoxMaterialType() {
        var url = "/Material/GetMaterialType";
        $.getJSON(url, function (datas) {
            $('#materialType').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#materialType').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#materialType').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    /*function Init List Using Jtable */
    function initListMaterial() {
        $("#" + global.Element.JtableMaterial).jtable({
            title: "Danh sách Vật Tư",
            paging: true,
            pageSize: 10,
            pageSizeChangeMaterial: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListMaterial,
                createAction: global.Element.PopupMaterial,
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
                    title: "Mã Vật Tư",
                    width: "5%"
                },
                Name: {
                    visibility: "fixed",
                    title: "Tên Vật Tư",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            $('#materialType').val(data.record.MaterialTypeId);
                            bindData(data.record);
                            showPopupMaterial();
                        });
                        return text;
                    }
                },
                MaterialName: {
                    title: "Loại Vật Tư",
                    width: "25%"
                },
                Description: {
                    title: "Mô Tả",
                    width: "25%"
                },
                Inventory: {
                    title: "Tồn Kho",
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
                                realTimeHub.server.sendUpdateEvent("jtableMaterial");
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
    function saveMaterial() {
        global.Data.ModelMaterial.MaterialTypeId = $('#materialType').val();
        $.ajax({
            url: global.UrlAction.SaveMaterial,
            type: 'post',
            data: ko.toJSON(global.Data.ModelMaterial),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListMaterial();
                        $("#" + global.Element.PopupMaterial).modal("hide");
                        toastr.success('Thanh Cong');
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                                var realTimeHub = $.connection.realTimeJTableDemoHub;
                                realTimeHub.server.sendUpdateEvent("jtableMaterial", global.Data.ClientId, "Cập nhật Vật Tư");
                                $.connection.hub.start();
                    }
                }, false, global.Element.PopupMaterial, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupMaterial() {
        $("#" + global.Element.PopupMaterial).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupMaterial + " button[save]").click(function () {
            saveMaterial();
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableMaterial", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupMaterial + " button[cancel]").click(function () {
            $("#" + global.Element.PopupMaterial).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListMaterial = function () {
        reloadListMaterial();
    };
    this.initViewModel = function (material) {
        initViewModel(material);
    };
    this.bindData = function (material) {
        bindData(material);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        initComboBoxMaterialType();
        registerEvent();
        initListMaterial();
        reloadListMaterial();
        initPopupMaterial();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var material = new VINASIC.Material();
    material.Init();
});