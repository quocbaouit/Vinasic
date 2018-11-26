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
VINASIC.namespace("SysStatus");
VINASIC.SysStatus = function () {
    var global = {
        UrlAction: {
            GetListSysStatus: "/SysStatus/GetSysStatuss",
            SaveSysStatus: "/SysStatus/SaveSysStatus",
            DeleteSysStatus: "/SysStatus/DeleteSysStatus"
        },
        Element: {
            JtableSysStatus: "jtableSysStatus",
            PopupSysStatus: "popup_SysStatus",
            PopupSearch: "popup_SearchSysStatus"
        },
        Data: {
            ModelSysStatus: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListSysStatus() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableSysStatus).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(sysStatus) {
        var sysStatusViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: ""
        };
        if (sysStatus != null) {
            sysStatusViewModel = {
                Id: ko.observable(sysStatus.Id),
                Code: ko.observable(sysStatus.Code),
                Name: ko.observable(sysStatus.Name),
                Description: ko.observable(sysStatus.Description)
            };
        }
        return sysStatusViewModel;
    }
    function bindData(sysStatus) {
        global.Data.ModelSysStatus = initViewModel(sysStatus);
        ko.applyBindings(global.Data.ModelSysStatus);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupSysStatus() {
        $("#" + global.Element.PopupSysStatus).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteSysStatus,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListSysStatus();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupSysStatus, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListSysStatus() {
        $("#" + global.Element.JtableSysStatus).jtable({
            title: "Danh sách Loại Dịch Vụ",
            paging: true,
            pageSize: 10,
            pageSizeChangeSysStatus: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListSysStatus,
                createAction: global.Element.PopupSysStatus,
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
                            showPopupSysStatus();
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
                                realTimeHub.server.sendUpdateEvent("jtableSysStatus");
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
    function saveSysStatus() {
        $.ajax({
            url: global.UrlAction.SaveSysStatus,
            type: 'post',
            data: ko.toJSON(global.Data.ModelSysStatus),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListSysStatus();
                        $("#" + global.Element.PopupSysStatus).modal("hide");
                        toastr.success('Thành Công');
                    }
                }, false, global.Element.PopupSysStatus, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupSysStatus() {
        $("#" + global.Element.PopupSysStatus).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupSysStatus + " button[save]").click(function () {
            saveSysStatus();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableSysStatus", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupSysStatus + " button[cancel]").click(function () {
            $("#" + global.Element.PopupSysStatus).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListSysStatus = function () {
        reloadListSysStatus();
    };
    this.initViewModel = function (sysStatus) {
        initViewModel(sysStatus);
    };
    this.bindData = function (sysStatus) {
        bindData(sysStatus);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListSysStatus();
        reloadListSysStatus();
        initPopupSysStatus();       
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var sysStatus = new VINASIC.SysStatus();
    sysStatus.Init();
});