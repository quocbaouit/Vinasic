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
VINASIC.namespace("Permission");
VINASIC.Permission = function () {
    var global = {
        UrlAction: {
            GetListPermission: "/Permission/GetPermissions",
            SavePermission: "/Permission/SavePermission",
            DeletePermission: "/Permission/DeletePermission"
        },
        Element: {
            JtablePermission: "jtablePermission",
            PopupPermission: "popup_Permission",
            PopupSearch: "popup_SearchPermission"
        },
        Data: {
            ModelPermission: {},
            ModelConfig: {},
            ClientId: "",
            listSelectPermision: []
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListPermission() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtablePermission).jtable("load", { 'keyword': keySearch });
    }

    function removeItemInArray(arr, id) {
        if (typeof (arr) != "undefined") {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] === id) {
                    arr.splice(i, 1);
                    break;
                }
            };
        }
    }
    /*function init model using knockout Js*/
    function initViewModel(permission) {
        var permissionViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: ""
        };
        if (permission != null) {
            permissionViewModel = {
                Id: ko.observable(permission.Id),
                Code: ko.observable(permission.Code),
                Name: ko.observable(permission.Name),
                Description: ko.observable(permission.Description)
            };
        }
        return permissionViewModel;
    }
    function bindData(permission) {
        global.Data.ModelPermission = initViewModel(permission);
        ko.applyBindings(global.Data.ModelPermission);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupPermission() {
        $("#" + global.Element.PopupPermission).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeletePermission,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListPermission();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupPermission, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListPermission() {
        $("#" + global.Element.JtablePermission).jtable({
            title: "Danh sách Quyền Hệ Thống",
            paging: false,
            pageSize: 1000,
            pageSizeChangePermission: false,
            sorting: false,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListPermission,
                createObjDefault: initViewModel(null)
            },
            messages: {
                selectShow: "Ẩn hiện cột"
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                FeatureName: {
                    visibility: "fixed",
                    title: "Chức Năng",
                    width: "20",
                    display: function (data) {
                        var text = "";
                        if (data.record.PermissionName === null) {

                            text = $("<br/><hr/><a style=\"color: red\" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.FeatureName + "</a>");
                        }
                        else {
                            text = $("");
                        }
                        //var text = $("<br/><hr/><a style=\"color: red\" href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.FeatureName + "</a>");
                        text.click(function () {
                            //bindData(data.record);
                            //showPopupPermission();
                        });
                        return text;
                    }
                },
                Select: {
                    title: 'Chọn',
                    width: '2%',
                    list: true,
                    display: function (data) {
                        var text = "";
                        if (data.record.PermissionName != null) {
                            if (data.record.Selected == true) {
                                text = $("<input id=check" + data.record.Id + " type=\"checkbox\" checked>");
                            }
                            else {
                                text = $("<input id=check" + data.record.Id + " type=\"checkbox\">");
                            }
                        }
                        else {
                            text = $("<hr/>");
                        }
                        text.click(function () {
                            var id = "check" + data.record.Id;
                            var isCheck = document.getElementById(id).checked;
                            if (isCheck == true) {
                                toastr.success("Selected");
                                global.Data.listSelectPermision.push(data.record.Id);
                            }
                            else {
                                toastr.warning("DeSelected");
                                removeItemInArray(global.Data.listSelectPermision, data.record.Id)
                            }

                        });
                        return text;
                    }
                },
                PermissionName: {
                    title: "Tên Quyền",
                    width: "20%",
                    display: function (data) {
                        var elementDisplay = "";
                        if (data.record.PermissionName === null)
                        { elementDisplay = "<hr/>"; }
                        else {
                            elementDisplay = "<p>" + data.record.PermissionName + "</p>";
                        }
                        return elementDisplay;
                    }
                },
                Description: {
                    title: "Mô Tả",
                    width: "20%",
                    display: function (data) {
                        var elementDisplay = "";
                        if (data.record.PermissionName === null)
                        { elementDisplay = "<hr/>"; }
                        else {
                            elementDisplay = "<p>" + data.record.Description + "</p>";
                        }
                        return elementDisplay;
                    }
                },
                Url: {
                    title: "Đường Dẫn",
                    width: "25%",
                    display: function (data) {
                        var elementDisplay = "";
                        if (data.record.PermissionName === null)
                        { elementDisplay = "<hr/>"; }
                        else {
                            elementDisplay = "<p>" + data.record.Url + "</p>";
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
                                realTimeHub.server.sendUpdateEvent("jtablePermission");
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
    function savePermission() {
        $.ajax({
            url: global.UrlAction.SavePermission,
            type: 'post',
            data: ko.toJSON(global.Data.ModelPermission),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListPermission();
                        $("#" + global.Element.PopupPermission).modal("hide");
                        toastr.success('Thanh Cong');
                    }
                }, false, global.Element.PopupPermission, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupPermission() {
        $("#" + global.Element.PopupPermission).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupPermission + " button[save]").click(function () {
            savePermission();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtablePermission", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupPermission + " button[cancel]").click(function () {
            $("#" + global.Element.PopupPermission).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListPermission = function () {
        reloadListPermission();
    };
    this.initViewModel = function (permission) {
        initViewModel(permission);
    };
    this.bindData = function (permission) {
        bindData(permission);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListPermission();
        reloadListPermission();
        //initPopupPermission();
        //bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var permission = new VINASIC.Permission();
    permission.Init();
});