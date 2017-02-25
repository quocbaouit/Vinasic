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
VINASIC.namespace("ElementFormular");
VINASIC.ElementFormular = function () {
    var global = {
        UrlAction: {
            GetListElementFormular: "/ElementFormular/GetElementFormulars",
            SaveElementFormular: "/ElementFormular/SaveElementFormular",
            DeleteElementFormular: "/ElementFormular/DeleteElementFormular"
        },
        Element: {
            JtableElementFormular: "jtableElementFormular",
            PopupElementFormular: "popup_ElementFormular",
            PopupSearch: "popup_SearchElementFormular"
        },
        Data: {
            ModelElementFormular: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListElementFormular() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableElementFormular).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(elementFormular) {
        var elementFormularViewModel = {
            Id: 0,           
            Name: "",
            Description: "",
            DefaultValue: "",
            IsSystem: false
        };
        if (elementFormular != null) {
            elementFormularViewModel = {
                Id: ko.observable(elementFormular.Id),
                Name: ko.observable(elementFormular.Name),
                Description: ko.observable(elementFormular.Description),
                DefaultValue: ko.observable(elementFormular.DefaultValue),
                IsSystem: ko.observable(false)
            };
        }
        return elementFormularViewModel;
    }
    function bindData(elementFormular) {
        global.Data.ModelElementFormular = initViewModel(elementFormular);
        ko.applyBindings(global.Data.ModelElementFormular);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupElementFormular() {
        $("#" + global.Element.PopupElementFormular).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteElementFormular,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListElementFormular();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupElementFormular, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListElementFormular() {
        $("#" + global.Element.JtableElementFormular).jtable({
            title: "Danh sách Các Đại Lượng",
            paging: true,
            pageSize: 10,
            pageSizeChangeElementFormular: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListElementFormular,
                createAction: global.Element.PopupElementFormular,
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
                    title: "Tên",
                    width: "20%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupElementFormular();
                        });
                        return text;
                    }
                },
                Description: {
                    title: "Mô Tả",
                    width: "25%"
                },
                strDefaultValue: {
                    title: "Giá Trị Măc Định (VND)",
                    width: "20%"
                },
                IsSystem: {
                    title: "Là Đại Lượng Hệ Thống",
                    width: "20%",
                    display: function (data) {
                        var elementDisplay = "";
                        if (data.record.IsSystem)
                        { elementDisplay = "<input  type='checkbox' checked='checked' disabled/>"; }
                        else {
                            elementDisplay = "<input  type='checkbox' disabled />";
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
                                realTimeHub.server.sendUpdateEvent("jtableElementFormular");
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
    function saveElementFormular() {
        global.Data.ModelElementFormular.DefaultValue = $('#DefaultValue').val().replace(/[^0-9-.]/g, '');
        $.ajax({
            url: global.UrlAction.SaveElementFormular,
            type: 'post',
            data: ko.toJSON(global.Data.ModelElementFormular),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListElementFormular();
                        $("#" + global.Element.PopupElementFormular).modal("hide");
                        toastr.success('Thành Công');
                    }
                }, false, global.Element.PopupElementFormular, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupElementFormular() {
        $("#" + global.Element.PopupElementFormular).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupElementFormular + " button[save]").click(function () {
            saveElementFormular();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableElementFormular", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupElementFormular + " button[cancel]").click(function () {
            $("#" + global.Element.PopupElementFormular).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListElementFormular = function () {
        reloadListElementFormular();
    };
    this.initViewModel = function (elementFormular) {
        initViewModel(elementFormular);
    };
    this.bindData = function (elementFormular) {
        bindData(elementFormular);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListElementFormular();
        reloadListElementFormular();
        initPopupElementFormular();       
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var elementFormular = new VINASIC.ElementFormular();
    elementFormular.Init();
});