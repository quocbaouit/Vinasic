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
VINASIC.namespace("Sticker");
VINASIC.Sticker = function () {
    var global = {
        UrlAction: {
            GetListSticker: "/StickerCms/GetStickerCms",
            SaveSticker: "/StickerCms/SaveStickerCms",
            DeleteSticker: "/StickerCms/DeleteStickerCms"
        },
        Element: {
            JtableSticker: "jtableSticker",
            PopupSticker: "popup_Sticker",
            PopupSearch: "popup_SearchSticker"
        },
        Data: {
            ModelSticker: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListSticker() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableSticker).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(sticker) {
        var stickerViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: ""
        };
        if (sticker != null) {
            stickerViewModel = {
                Id: ko.observable(sticker.Id),
                Code: ko.observable(sticker.Code),
                Name: ko.observable(sticker.Name),
                Description: ko.observable(sticker.Description)
            };
        }
        return stickerViewModel;
    }
    function bindData(sticker) {
        global.Data.ModelSticker = initViewModel(sticker);
        ko.applyBindings(global.Data.ModelSticker);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupSticker() {
        $("#" + global.Element.PopupSticker).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteSticker,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListSticker();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupSticker, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListSticker() {
        $("#" + global.Element.JtableSticker).jtable({
            title: "Danh sách Loại Dịch Vụ",
            paging: true,
            pageSize: 10,
            pageSizeChangeSticker: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListSticker,
                createAction: global.Element.PopupSticker,
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
                            showPopupSticker();
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
                                realTimeHub.server.sendUpdateEvent("jtableSticker");
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
    function saveSticker() {
        $.ajax({
            url: global.UrlAction.SaveSticker,
            type: 'post',
            data: ko.toJSON(global.Data.ModelSticker),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListSticker();
                        $("#" + global.Element.PopupSticker).modal("hide");
                        toastr.success('Thành Công');
                    }
                }, false, global.Element.PopupSticker, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupSticker() {
        $("#" + global.Element.PopupSticker).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupSticker + " button[save]").click(function () {
            saveSticker();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableSticker", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupSticker + " button[cancel]").click(function () {
            $("#" + global.Element.PopupSticker).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListSticker = function () {
        reloadListSticker();
    };
    this.initViewModel = function (sticker) {
        initViewModel(sticker);
    };
    this.bindData = function (sticker) {
        bindData(sticker);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListSticker();
        reloadListSticker();
        initPopupSticker();       
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var sticker = new VINASIC.Sticker();
    sticker.Init();
});